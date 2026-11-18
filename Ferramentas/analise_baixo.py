# -*- coding: utf-8 -*-
"""
Detecta as notas do BAIXO de um mp3: instante do ataque (ms) e altura (nota MIDI).
Usado por gerar_mapa.py. Precisa de: ffmpeg, numpy, scipy.
"""
import subprocess, numpy as np
from scipy.signal import find_peaks

SR = 22050
NFFT = 2048          # janela de ~93 ms
ZP = 8192            # zero-padding (resolucao de frequencia ~2.7 Hz)
HOP = 128            # ~5.8 ms por quadro
FMIN, FMAX = 40.0, 210.0


def carregar(mp3):
    raw = subprocess.run(
        ["ffmpeg", "-loglevel", "error", "-i", mp3, "-ac", "1", "-ar", str(SR), "-f", "f32le", "-"],
        stdout=subprocess.PIPE, check=True).stdout
    return np.frombuffer(raw, dtype=np.float32)


def espectrograma(x):
    """Magnitudes ate ~2 kHz, calculadas em blocos (economiza memoria)."""
    n = 1 + (len(x) - NFFT) // HOP
    jan = np.hanning(NFFT).astype(np.float32)
    kmax = int(2000.0 / (SR / ZP)) + 1
    saida = np.zeros((n, kmax), dtype=np.float32)
    bloco = 1500
    for i in range(0, n, bloco):
        j = min(n, i + bloco)
        idx = np.arange(NFFT)[None, :] + HOP * np.arange(i, j)[:, None]
        saida[i:j] = np.abs(np.fft.rfft(x[idx] * jan, ZP, axis=1))[:, :kmax]
    return saida


def analisar(mp3):
    """Devolve lista de dicts: t (ms), midi (float), forca (0..1)."""
    x = carregar(mp3)
    mag = espectrograma(x)
    freqs = np.fft.rfftfreq(ZP, 1.0 / SR)[:mag.shape[1]]
    ms_quadro = HOP / SR * 1000.0

    # grade de candidatos a f0 (1/4 de semitom)
    cand = FMIN * 2 ** (np.arange(0, int(np.log2(FMAX / FMIN) * 48) + 1) / 48.0)
    sal = np.zeros((mag.shape[0], len(cand)), dtype=np.float32)
    for h, peso in zip((1, 2, 3, 4), (1.0, 0.8, 0.6, 0.4)):
        bins = np.round(cand * h / (SR / ZP)).astype(int)
        bins = np.clip(bins, 0, mag.shape[1] - 1)
        sal += peso * mag[:, bins]
    f0 = cand[np.argmax(sal, axis=1)]
    sal_max = sal.max(axis=1)

    # energia de graves e fluxo (aumento de energia) na faixa do baixo
    faixa = (freqs >= 40) & (freqs <= 320)
    e = np.log1p(mag[:, faixa].sum(axis=1) * 0.02)
    fluxo = np.maximum(0.0, e - np.concatenate(([e[0]], e[:-1])))
    # ataque tambem na faixa de "estalo" do baixo (harmonicos altos), ajuda a achar o inicio exato
    f2 = (freqs >= 400) & (freqs <= 1800)
    e2 = np.log1p(mag[:, f2].sum(axis=1) * 0.02)
    fluxo2 = np.maximum(0.0, e2 - np.concatenate(([e2[0]], e2[:-1])))
    onset = fluxo + 0.5 * fluxo2
    k = 3
    onset = np.convolve(onset, np.ones(k) / k, mode="same")

    thr = max(np.percentile(onset, 75), 1e-4)
    pk, props = find_peaks(onset, height=thr, distance=int(70 / ms_quadro))
    midi_all = 69 + 12 * np.log2(f0 / 440.0)

    notas = []
    for p in pk:
        a, b = p + int(25 / ms_quadro), p + int(150 / ms_quadro)
        if b >= len(f0):
            continue
        seg = midi_all[a:b]
        seg_sal = sal_max[a:b]
        mediana = np.median(seg)
        # nota de baixo = altura estavel logo depois do ataque (bumbo desliza e some rapido)
        estavel = np.mean(np.abs(seg - mediana) < 0.8)
        nivel = seg_sal.mean() / (np.percentile(sal_max, 90) + 1e-9)
        notas.append(dict(t=p * ms_quadro + NFFT / 2 / SR * 1000.0,
                          midi=float(mediana), estavel=float(estavel),
                          nivel=float(nivel), forca=float(props["peak_heights"][list(pk).index(p)])))
    refinar(x, notas)
    return notas, len(x) / SR


def refinar(x, notas):
    """Ajusta o tempo de cada ataque olhando o sinal filtrado (40-400 Hz): acha onde o volume sobe mais rapido."""
    from scipy.signal import butter, sosfiltfilt
    sos = butter(3, [40, 400], btype="band", fs=SR, output="sos")
    y = np.abs(sosfiltfilt(sos, x))
    k = int(0.006 * SR)
    env = np.convolve(y, np.ones(k) / k, mode="same")
    d = np.diff(env, prepend=env[0])
    for n in notas:
        c = int(n["t"] / 1000.0 * SR)
        a, b = max(0, c - int(0.050 * SR)), min(len(d), c + int(0.050 * SR))
        if b - a < 10:
            continue
        # primeiro ponto em que a subida passa de 60% da subida maxima da janela (inicio do ataque)
        seg = d[a:b]
        lim = 0.6 * seg.max()
        i = int(np.argmax(seg >= lim)) if lim > 0 else int(np.argmax(seg))
        n["t"] = (a + i) / SR * 1000.0
