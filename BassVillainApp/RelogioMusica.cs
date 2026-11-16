using System;
using System.Diagnostics;

namespace BassVillainApp
{
    /// <summary>
    /// Relógio do jogo. Usa o Stopwatch (suave) e se ajusta continuamente
    /// à posição real da música (precisa), para as notas não se afastarem do áudio.
    /// Tem a mesma "cara" do Stopwatch (Restart, Start, Stop, ElapsedMilliseconds)
    /// para o resto do código quase não mudar.
    /// </summary>
    internal class RelogioMusica
    {
        private readonly Stopwatch cronometro = new Stopwatch();

        // Soma dos pequenos ajustes feitos para acompanhar o áudio.
        private double correcaoMs;

        // Fração do erro corrigida a cada tick (5% => ajuste gradual, sem "pulos" visuais).
        private const double FatorCorrecao = 0.05;

        // Erro acima disso (ex.: o áudio demorou para começar) é corrigido de uma vez.
        private const double LimiteSaltoMs = 500;

        /// <summary>
        /// Calibração do jogador (tela de configurações), em ms.
        /// Positivo = as notas chegam à linha mais CEDO; negativo = mais TARDE
        /// (útil quando o som chega atrasado, como em fone Bluetooth).
        /// </summary>
        public int OffsetMs { get; set; }

        public long ElapsedMilliseconds
        {
            get { return (long)(cronometro.Elapsed.TotalMilliseconds + correcaoMs + OffsetMs); }
        }

        public void Restart()
        {
            correcaoMs = 0;
            cronometro.Restart();
        }

        public void Start() { cronometro.Start(); }

        public void Stop() { cronometro.Stop(); }

        /// <summary>
        /// Chamado a cada tick com a posição atual da música.
        /// </summary>
        public void Corrigir(double posicaoAudioMs)
        {
            if (!cronometro.IsRunning)
                return;

            double erro = posicaoAudioMs - (cronometro.Elapsed.TotalMilliseconds + correcaoMs);

            if (Math.Abs(erro) > LimiteSaltoMs)
                correcaoMs += erro;
            else
                correcaoMs += erro * FatorCorrecao;
        }
    }
}
