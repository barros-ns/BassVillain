using System;
using System.Drawing;
using System.Windows.Forms;

namespace BassVillainApp
{
    /// <summary>Desenho do título do menu inicial (separado para poder ser testado sem janela).</summary>
    internal static class DesenhoMenu
    {
        public static void Titulo(Graphics g, TemaVisual tema, Size tela)
        {
            Image logo = tema.ImagemTitulo;
            if (logo != null)
            {
                // Logo própria (Imagens\Menu\titulo.png), ajustada para caber.
                float escala = Math.Min(640f / logo.Width, 240f / logo.Height);
                float w = logo.Width * escala;
                float h = logo.Height * escala;
                g.DrawImage(logo, (tela.Width - w) / 2f, 60f + (240f - h) / 2f, w, h);
            }
            else
            {
                TextoEstilizado.Desenhar(g, tema, "BASS",
                    new RectangleF(0f, 56f, tela.Width, 125f), 112f, tema.CorNota(0), StringAlignment.Center);
                TextoEstilizado.Desenhar(g, tema, "VILLAIN",
                    new RectangleF(0f, 172f, tela.Width, 125f), 112f, tema.CorNota(2), StringAlignment.Center);
            }

            // Quatro triângulos, uma cor por pista: o mesmo símbolo dos botões do jogo.
            for (int i = 0; i < 4; i++)
            {
                float cx = tela.Width / 2f + (i - 1.5f) * 62f;
                using (var forma = Formas.TrianguloParaCima(cx, 322f, 40f, 34f))
                {
                    using (var preench = new System.Drawing.Drawing2D.LinearGradientBrush(
                               new RectangleF(cx - 20f, 305f, 40f, 34f),
                               TextoEstilizado.Clarear(tema.CorNota(i), 0.5f), tema.CorNota(i), 90f))
                        g.FillPath(preench, forma);
                    using (var borda = new Pen(Color.FromArgb(230, 255, 255, 255), 2f))
                    {
                        borda.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                        g.DrawPath(borda, forma);
                    }
                }
            }
        }

        /// <summary>Cabeçalho das telas de seleção: título grande, subtítulo e os quatro triângulos do jogo.</summary>
        public static void TituloDeTela(Graphics g, TemaVisual tema, string titulo, string subtitulo, Size tela, float topo)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            TextoEstilizado.Desenhar(g, tema, titulo,
                new RectangleF(30f, topo, tela.Width - 60f, 80f), 64f, tema.CorNota(0), StringAlignment.Center);

            if (!string.IsNullOrEmpty(subtitulo))
                TextoEstilizado.Rotulo(g, tema, subtitulo,
                    new RectangleF(0f, topo + 88f, tela.Width, 30f), 20f, tema.CorTexto, StringAlignment.Center);

            float yTriangulos = topo + (string.IsNullOrEmpty(subtitulo) ? 108f : 138f);
            for (int i = 0; i < 4; i++)
            {
                float cx = tela.Width / 2f + (i - 1.5f) * 40f;
                using (var forma = Formas.TrianguloParaCima(cx, yTriangulos, 26f, 22f))
                {
                    using (var preench = new System.Drawing.Drawing2D.LinearGradientBrush(
                               new RectangleF(cx - 13f, yTriangulos - 11f, 26f, 22f),
                               TextoEstilizado.Clarear(tema.CorNota(i), 0.5f), tema.CorNota(i), 90f))
                        g.FillPath(preench, forma);
                    using (var borda = new Pen(Color.FromArgb(230, 255, 255, 255), 1.5f))
                    {
                        borda.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                        g.DrawPath(borda, forma);
                    }
                }
            }
        }

        /// <summary>Legenda pequena logo abaixo de um botão (ex.: "BLOQUEADA" ou a melhor pontuação).</summary>
        public static void LegendaDoBotao(Graphics g, TemaVisual tema, Control botao, string texto, Color cor)
        {
            TextoEstilizado.Rotulo(g, tema, texto,
                new RectangleF(botao.Left, botao.Bottom + 2f, botao.Width, 24f), 15f, cor, StringAlignment.Center);
        }
    }
}
