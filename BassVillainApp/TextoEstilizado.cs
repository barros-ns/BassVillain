using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BassVillainApp
{
    /// <summary>
    /// Textos do jogo: números grandes com contorno, sombra e degradê (ou imagens de dígitos
    /// se existirem num_0.png ... num_9.png) e rótulos pequenos.
    /// </summary>
    internal static class TextoEstilizado
    {
        public static Color Clarear(Color cor, float fator)
        {
            return Color.FromArgb(
                cor.A,
                (int)(cor.R + (255 - cor.R) * fator),
                (int)(cor.G + (255 - cor.G) * fator),
                (int)(cor.B + (255 - cor.B) * fator));
        }

        /// <summary>Rótulo pequeno (ex.: "PONTUAÇÃO") com sombra leve.</summary>
        public static void Rotulo(Graphics g, TemaVisual tema, string texto, RectangleF area,
                                  float tamanhoPx, Color cor, StringAlignment alinhamento)
        {
            using (Font fonte = tema.CriarFonte(tamanhoPx))
            using (var formato = new StringFormat())
            {
                formato.Alignment = alinhamento;
                formato.LineAlignment = StringAlignment.Center;

                var sombra = new RectangleF(area.X + 1f, area.Y + 1f, area.Width, area.Height);
                using (Brush pincelSombra = new SolidBrush(Color.FromArgb(160, 0, 0, 0)))
                    g.DrawString(texto, fonte, pincelSombra, sombra, formato);

                using (Brush pincel = new SolidBrush(cor))
                    g.DrawString(texto, fonte, pincel, area, formato);
            }
        }

        /// <summary>Texto grande estilizado. tamanhoPx é a altura aproximada das letras.</summary>
        public static void Desenhar(Graphics g, TemaVisual tema, string texto, RectangleF area,
                                    float tamanhoPx, Color cor, StringAlignment alinhamento)
        {
            if (string.IsNullOrEmpty(texto))
                return;

            if (DesenharComImagens(g, tema, texto, area, tamanhoPx, alinhamento))
                return;

            SmoothingMode anterior = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float tamanhoUsado = tamanhoPx;
            GraphicsPath caminho = CriarCaminho(tema, texto, tamanhoUsado);
            RectangleF limites = caminho.GetBounds();

            // Texto mais largo que a área: diminui a letra até caber (evita sobrepor vizinhos).
            if (limites.Width > area.Width && area.Width > 10f)
            {
                tamanhoUsado = tamanhoPx * area.Width / limites.Width;
                caminho.Dispose();
                caminho = CriarCaminho(tema, texto, tamanhoUsado);
                limites = caminho.GetBounds();
            }

            using (caminho)
            {
                if (limites.Width < 1f || limites.Height < 1f)
                {
                    g.SmoothingMode = anterior;
                    return;
                }

                float x = area.X;
                if (alinhamento == StringAlignment.Center)
                    x = area.X + (area.Width - limites.Width) / 2f;
                else if (alinhamento == StringAlignment.Far)
                    x = area.Right - limites.Width;
                float y = area.Y + (area.Height - limites.Height) / 2f;

                using (var mover = new Matrix())
                {
                    mover.Translate(x - limites.X, y - limites.Y);
                    caminho.Transform(mover);
                }

                float espessura = Math.Max(2f, tamanhoUsado / 7f);

                // Brilho (neon) na cor do texto
                using (var brilho = new Pen(Color.FromArgb(70, cor), espessura * 2.8f))
                {
                    brilho.LineJoin = LineJoin.Round;
                    g.DrawPath(brilho, caminho);
                }

                // Sombra
                using (var mover = new Matrix())
                using (GraphicsPath sombra = (GraphicsPath)caminho.Clone())
                using (var pincelSombra = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
                {
                    mover.Translate(2f, 3f);
                    sombra.Transform(mover);
                    g.FillPath(pincelSombra, sombra);
                }

                // Contorno + degradê (mais claro em cima)
                using (var contorno = new Pen(Color.FromArgb(230, 0, 0, 0), espessura))
                using (var degrade = new LinearGradientBrush(
                           new RectangleF(x, y, limites.Width, limites.Height),
                           Clarear(cor, 0.55f), cor, LinearGradientMode.Vertical))
                {
                    contorno.LineJoin = LineJoin.Round;
                    g.DrawPath(contorno, caminho);
                    g.FillPath(degrade, caminho);
                }

                // Fio de luz por cima (acabamento)
                using (var fio = new Pen(Color.FromArgb(110, 255, 255, 255), 1.2f))
                {
                    fio.LineJoin = LineJoin.Round;
                    g.DrawPath(fio, caminho);
                }
            }

            g.SmoothingMode = anterior;
        }

        private static GraphicsPath CriarCaminho(TemaVisual tema, string texto, float tamanhoPx)
        {
            var caminho = new GraphicsPath();
            caminho.AddString(texto, tema.Familia, (int)tema.EstiloFonte, tamanhoPx,
                              PointF.Empty, StringFormat.GenericTypographic);
            return caminho;
        }

        // Usa num_0.png ... num_9.png quando TODOS os caracteres têm imagem.
        private static bool DesenharComImagens(Graphics g, TemaVisual tema, string texto,
                                               RectangleF area, float tamanhoPx,
                                               StringAlignment alinhamento)
        {
            var imagens = new List<Image>();
            foreach (char c in texto)
            {
                Image imagem = tema.ImagemDigito(c);
                if (imagem == null)
                    return false;
                imagens.Add(imagem);
            }

            float altura = tamanhoPx * 1.2f;
            float total = 0f;
            foreach (Image imagem in imagens)
                total += altura * imagem.Width / imagem.Height;

            float x = area.X;
            if (alinhamento == StringAlignment.Center)
                x = area.X + (area.Width - total) / 2f;
            else if (alinhamento == StringAlignment.Far)
                x = area.Right - total;
            float y = area.Y + (area.Height - altura) / 2f;

            foreach (Image imagem in imagens)
            {
                float largura = altura * imagem.Width / imagem.Height;
                g.DrawImage(imagem, x, y, largura, altura);
                x += largura;
            }
            return true;
        }
    }
}
