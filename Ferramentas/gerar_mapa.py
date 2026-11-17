# -*- coding: utf-8 -*-
"""
Gera os mapas (.json) de uma musica a partir das notas REAIS do baixo do mp3.

Uso:   python gerar_mapa.py DarkNecessities            (grava em ../BassVillainApp/Mapas)
       python gerar_mapa.py DarkNecessities --relatorio (so compara os mapas atuais com o baixo)
Precisa de: python3, numpy, scipy e ffmpeg no PATH.

Como funciona
  1. analise_baixo.py acha cada ataque de nota do baixo (tempo em ms e altura da nota).
  2. A nota que sobe/desce de altura vai para uma pista mais a direita/esquerda
     (mesma altura = mesma pista), como no Guitar Hero.
  3. Facil, Medio e Dificil usam as mesmas notas; so muda quantas entram
     (as mais fortes primeiro, respeitando um intervalo minimo).
"""
import sys, os, json
import numpy as np
from analise_baixo import analisar

AQUI = os.path.dirname(os.path.abspath(__file__))
PROJ = os.path.join(AQUI, "..", "BassVillainApp")

# intervalo minimo entre notas (ms): Facil, Medio, Dificil
GAPS = {"Facil": 520, "Medio": 270, "Dificil": 150}
PRIMEIRA_NOTA_MS = 1600   # as notas levam 1500 ms para cair
JANELA_CONTORNO_MS = 2500


def notas_validas(notas):
    ok = [n for n in notas if n["estavel"] >= 0.6 and n["nivel"] >= 0.25 and n["t"] >= PRIMEIRA_NOTA_MS]
    ok.sort(key=lambda n: n["t"])
    return ok


def escolher(ok):
    """Conjuntos encaixados: Facil dentro de Medio dentro de Dificil."""
    por_forca = sorted(ok, key=lambda n: -(n["forca"] * (0.5 + n["nivel"])))
    escolhidas, resultado = [], {}
    for nome in ("Facil", "Medio", "Dificil"):
        gap = GAPS[nome]
        for n in por_forca:
            if n in escolhidas:
                continue
            if all(abs(n["t"] - m["t"]) >= gap for m in escolhidas):
                escolhidas.append(n)
        resultado[nome] = sorted(escolhidas, key=lambda n: n["t"])
    return resultado


def pistas_pelo_contorno(todas, alvo):
    """Pista (0..3) de cada nota alvo = posicao da altura dela entre as notas vizinhas (+-2,5 s)."""
    tempos = np.array([n["t"] for n in todas])
    alturas = np.array([n["midi"] for n in todas])
    saida = []
    for n in alvo:
        viz = alturas[np.abs(tempos - n["t"]) <= JANELA_CONTORNO_MS]
        menores = np.sum(viz < n["midi"] - 0.5)
        iguais = np.sum(np.abs(viz - n["midi"]) <= 0.5)
        pos = (menores + iguais * 0.30) / max(1, len(viz))
        saida.append(min(3, int(pos * 4)))
    return saida


def quebrar_repeticoes(pistas, max_igual=6):
    """Baixo tocando a mesma nota muitas vezes (pedal) vira um 'galope' (estilo Barracuda):
    a cada 4 notas iguais seguidas, a 4a sobe uma pista (ou desce, se ja estiver na ultima)."""
    saida = list(pistas)
    inicio = 0
    while inicio < len(saida):
        fim = inicio
        while fim + 1 < len(saida) and pistas[fim + 1] == pistas[inicio]:
            fim += 1
        if fim - inicio + 1 > max_igual:
            for k in range(inicio, fim + 1):
                if (k - inicio) % 4 == 3:
                    saida[k] = pistas[inicio] + 1 if pistas[inicio] < 3 else pistas[inicio] - 1
        inicio = fim + 1
    return saida


def gravar(caminho, notas, pistas):
    linhas = ['  {"pista": %d, "tempoAcertoMs": %d}' % (p, round(n["t"])) for n, p in zip(notas, pistas)]
    with open(caminho, "w", encoding="utf-8", newline="") as f:
        f.write("[\r\n" + ",\r\n".join(linhas) + "\r\n]")


def relatorio(nome, ok):
    t_baixo = np.array([n["t"] for n in ok])
    print("Notas reais do baixo detectadas: %d" % len(t_baixo))
    rng = np.random.RandomState(1)
    for dif in ("Facil", "Medio", "Dificil"):
        cam = os.path.join(PROJ, "Mapas", "%s.%s.json" % (nome, dif))
        if not os.path.exists(cam):
            continue
        m = json.load(open(cam, encoding="utf-8-sig"))
        t = np.array([x["tempoAcertoMs"] for x in m], float)
        dist = np.array([np.min(np.abs(t_baixo - v)) for v in t])
        falso = rng.uniform(t.min(), t.max(), len(t))
        dist0 = np.array([np.min(np.abs(t_baixo - v)) for v in falso])
        print("  %-8s %4d notas | a menos de 60 ms de uma nota do baixo: %3.0f%%  (acaso: %3.0f%%) | mediana %3.0f ms"
              % (dif, len(t), 100 * np.mean(dist < 60), 100 * np.mean(dist0 < 60), np.median(dist)))


def main():
    nome = sys.argv[1]
    mp3 = os.path.join(PROJ, "MusicasMp3", nome + ".mp3")
    notas, dur = analisar(mp3)
    ok = notas_validas(notas)
    if "--relatorio" in sys.argv:
        relatorio(nome, ok)
        return
    conjuntos = escolher(ok)
    for dif, sel in conjuntos.items():
        pistas = quebrar_repeticoes(pistas_pelo_contorno(ok, sel))
        gravar(os.path.join(PROJ, "Mapas", "%s.%s.json" % (nome, dif)), sel, pistas)
        cont = np.bincount(pistas, minlength=4)
        print("%-8s %4d notas | pistas %s | primeira %d ms, ultima %d ms" %
              (dif, len(sel), cont.tolist(), round(sel[0]["t"]), round(sel[-1]["t"])))


if __name__ == "__main__":
    main()
