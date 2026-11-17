using System.Drawing;
using System.Drawing.Drawing2D;

namespace BassVillainApp
{
    /// <summary>Formas reutilizadas pelo jogo, pelos menus e pela tela de estatísticas.</summary>
    internal static class Formas
    {
        public static GraphicsPath TrianguloParaCima(float cx, float cy, float largura, float altura)
        {
            return TrianguloArredondado(new[]
            {
                new PointF(cx, cy - altura / 2f),
                new PointF(cx + largura / 2f, cy + altura / 2f),
                new PointF(cx - largura / 2f, cy + altura / 2f)
            });
        }

        public static GraphicsPath TrianguloParaBaixo(float cx, float cy, float largura, float altura)
        {
            return TrianguloArredondado(new[]
            {
                new PointF(cx - largura / 2f, cy - altura / 2f),
                new PointF(cx + largura / 2f, cy - altura / 2f),
                new PointF(cx, cy + altura / 2f)
            });
        }

        public static GraphicsPath TrianguloParaDireita(float cx, float cy, float largura, float altura)
        {
            return TrianguloArredondado(new[]
            {
                new PointF(cx - largura / 2f, cy - altura / 2f),
                new PointF(cx + largura / 2f, cy),
                new PointF(cx - largura / 2f, cy + altura / 2f)
            });
        }

        // Triângulo com os três cantos arredondados (curvas de Bézier nos vértices).
        private static GraphicsPath TrianguloArredondado(PointF[] v)
        {
            const float arredondar = 0.22f; // quanto de cada aresta vira curva
            var caminho = new GraphicsPath();
            PointF fimAnterior = PointF.Empty;

            for (int i = 0; i < 3; i++)
            {
                PointF anterior = v[(i + 2) % 3];
                PointF atual = v[i];
                PointF proximo = v[(i + 1) % 3];

                var entrada = new PointF(atual.X + (anterior.X - atual.X) * arredondar,
                                         atual.Y + (anterior.Y - atual.Y) * arredondar);
                var saida = new PointF(atual.X + (proximo.X - atual.X) * arredondar,
                                       atual.Y + (proximo.Y - atual.Y) * arredondar);
                var c1 = new PointF(entrada.X + (atual.X - entrada.X) * 0.6f,
                                    entrada.Y + (atual.Y - entrada.Y) * 0.6f);
                var c2 = new PointF(saida.X + (atual.X - saida.X) * 0.6f,
                                    saida.Y + (atual.Y - saida.Y) * 0.6f);

                if (i > 0)
                    caminho.AddLine(fimAnterior, entrada);
                caminho.AddBezier(entrada, c1, c2, saida);
                fimAnterior = saida;
            }

            caminho.CloseFigure();
            return caminho;
        }

        public static GraphicsPath RetanguloArredondado(RectangleF r, float raio)
        {
            float d = System.Math.Min(raio * 2f, System.Math.Min(r.Width, r.Height));
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
    }
}
