using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BassVillainApp
{
    /// <summary>
    /// Desenha o fundo do tema. A imagem é redimensionada uma única vez (e de novo
    /// só se a janela mudar de tamanho), em vez de ser esticada a cada frame.
    /// </summary>
    internal sealed class FundoCache : IDisposable
    {
        private Bitmap cache;
        private Size tamanho;

        public void Desenhar(Graphics g, TemaVisual tema, Size t, Image imagem = null)
        {
            Image fundo = imagem ?? tema.ImagemFundo;

            if (fundo == null)
            {
                using (var degrade = new LinearGradientBrush(new Rectangle(0, 0, t.Width, t.Height),
                           TextoEstilizado.Clarear(tema.CorFundo, 0.25f), tema.CorFundo, 90f))
                    g.FillRectangle(degrade, 0, 0, t.Width, t.Height);
                return;
            }

            if (cache == null || tamanho != t)
            {
                if (cache != null)
                    cache.Dispose();

                cache = new Bitmap(t.Width, t.Height);
                using (Graphics gc = Graphics.FromImage(cache))
                {
                    gc.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gc.DrawImage(fundo, 0, 0, t.Width, t.Height);
                }
                tamanho = t;
            }

            g.DrawImage(cache, 0, 0, t.Width, t.Height);
        }

        public void Dispose()
        {
            if (cache != null)
                cache.Dispose();
        }
    }
}
