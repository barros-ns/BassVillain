using System;
using System.Drawing;

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
    }
}
