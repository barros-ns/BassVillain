using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace BassVillainApp
{
    public partial class TelaJogo : FormularioBase
    {

        #region Variaveis do Jogo
        private readonly Stopwatch relogio = new Stopwatch();

        private readonly System.Windows.Forms.Timer timerJogo =
            new System.Windows.Forms.Timer();

        private readonly Nota notaTeste = new Nota(1, 3000);

        private const int TempoDeQuedaMs = 1500;
        #endregion

        public TelaJogo()
        {
            InitializeComponent();
            DoubleBuffered = true;
            pnl_Jogo.Paint+=pnl_Jogo_Paint;

            timerJogo.Interval = 16;
            timerJogo.Tick += timerJogo_Tick;

            Shown += TelaJogo_Shown;
            FormClosed += TelaJogo_FormClosed;
        }

        #region Eventos do Jogo
        private void TelaJogo_Shown(object sender, EventArgs e)
        {
            relogio.Restart();
            timerJogo.Start();
        }

        private void timerJogo_Tick(object sender, EventArgs e)
        {
            pnl_Jogo.Invalidate();
        }

        private void TelaJogo_FormClosed(object sender,FormClosedEventArgs e)
        {
            timerJogo.Stop();
            timerJogo.Dispose();
            relogio.Stop();
        }
        #endregion

        #region Desenho do Jogo
        private void pnl_Jogo_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            float inicioX = pnl_Jogo.Width * 0.10f;
            float inicioY = 20f;

            float largura = pnl_Jogo.Width * 0.80f;
            float altura = pnl_Jogo.Height -40f;

            float larguraPista = largura / 4f;
            float yAlvo = inicioY + altura - 80f;

            using (Pen contorno = new Pen(Color.Gray, 2))
            using (Pen linhaAlvo = new Pen(Color.Red, 3))
            {
                // Contorno do cenário.
                g.DrawRectangle(
                    contorno, inicioX, inicioY, largura, altura
                );

                // Três divisórias formam quatro pistas.
                for (int i = 1; i < 4; i++)
                {
                    float x = inicioX + i * larguraPista;

                    g.DrawLine(
                        contorno,
                        x, inicioY,
                        x, inicioY + altura
                    );
                }
                g.DrawLine(
                linhaAlvo,
                inicioX, yAlvo,
                inicioX + largura, yAlvo
            );
            }

            long tempoAtualMs = relogio.ElapsedMilliseconds;

            float progresso =
                1f - (notaTeste.TempoAcertoMs - tempoAtualMs)
                / (float)TempoDeQuedaMs;

            // Desenha apenas durante o percurso até a linha.
            if (progresso >= 0f && progresso <= 1f)
            {
                float distanciaQueda = yAlvo - inicioY;

                float yNota = inicioY + progresso * distanciaQueda;
                float xNota = inicioX + notaTeste.Pista * larguraPista;

                const float alturaNota = 20f;

                using (Brush pincel = new SolidBrush(Color.DeepSkyBlue))
                {
                    g.FillRectangle(
                        pincel,
                        xNota + 10f,
                        yNota - alturaNota / 2f,
                        larguraPista - 20f,
                        alturaNota
                    );
                }
            }




        }
        #endregion
    }
}
