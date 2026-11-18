using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BassVillainApp
{
    /// Desenho do título do menu inicial (separado para poder ser testado sem janela).
    internal static class DesenhoMenu
    {

        public static void Titulo(Graphics g, TemaVisual tema, Size tela)
        {
            Image logo = tema.ImagemTitulo;

            if (logo != null)
            {
                float escala = Math.Min(640f / logo.Width, 240f / logo.Height);
                float w = logo.Width * escala;
                float h = logo.Height * escala;

                g.DrawImage(
                    logo,
                    (tela.Width - w) / 2f,
                    60f + (240f - h) / 2f,
                    w,
                    h
                );
            }
            else
            {
                using (Font fonte = new Font("Arial", 68f, FontStyle.Bold))
                using (Brush sombra = new SolidBrush(Color.FromArgb(130, 0, 0, 0)))
                using (Brush texto = new SolidBrush(Color.White))
                using (StringFormat formato = new StringFormat())
                {
                    formato.Alignment = StringAlignment.Center;
                    formato.LineAlignment = StringAlignment.Center;

                    RectangleF areaBass = new RectangleF(0, 65, tela.Width, 100);
                    RectangleF areaVillain = new RectangleF(0, 155, tela.Width, 100);

                    RectangleF sombraBass = areaBass;
                    sombraBass.Offset(3, 4);

                    RectangleF sombraVillain = areaVillain;
                    sombraVillain.Offset(3, 4);

                    g.DrawString("BASS", fonte, sombra, sombraBass, formato);
                    g.DrawString("VILLAIN", fonte, sombra, sombraVillain, formato);

                    g.DrawString("BASS", fonte, texto, areaBass, formato);
                    g.DrawString("VILLAIN", fonte, texto, areaVillain, formato);
                }
            }

            // Símbolos das quatro pistas
            // Notas estilizadas no padrão Guitar Hero
            for (int i = 0; i < 4; i++)
            {
                float cx = tela.Width / 2f + (i - 1.5f) * 62f;
                float cy = 322f;

                Color cor = tema.CorNota(i);
                Color corClara = TextoEstilizado.Clarear(cor, 0.55f);
                Color roxo = Color.FromArgb(155, 65, 255);

                PointF[] pontosSombra =
                {
        new PointF(cx, cy - 23),
        new PointF(cx + 25, cy + 20),
        new PointF(cx - 25, cy + 20)
    };

                PointF[] pontosExternos =
                {
        new PointF(cx, cy - 21),
        new PointF(cx + 23, cy + 17),
        new PointF(cx - 23, cy + 17)
    };

                PointF[] pontosInternos =
                {
        new PointF(cx, cy - 14),
        new PointF(cx + 16, cy + 11),
        new PointF(cx - 16, cy + 11)
    };

                using (GraphicsPath sombra = new GraphicsPath())
                using (GraphicsPath externo = new GraphicsPath())
                using (GraphicsPath interno = new GraphicsPath())
                {
                    sombra.AddPolygon(pontosSombra);
                    externo.AddPolygon(pontosExternos);
                    interno.AddPolygon(pontosInternos);

                    // Sombra externa
                    using (SolidBrush pincel = new SolidBrush(Color.FromArgb(190, 0, 0, 0)))
                    {
                        g.FillPath(pincel, sombra);
                    }

                    // Base preta
                    using (SolidBrush pincel = new SolidBrush(Color.FromArgb(255, 8, 5, 15)))
                    {
                        g.FillPath(pincel, externo);
                    }

                    // Contorno roxo metálico
                    using (Pen borda = new Pen(roxo, 3f))
                    {
                        borda.LineJoin = LineJoin.Round;
                        g.DrawPath(borda, externo);
                    }

                    // Interior com degradê
                    RectangleF area = new RectangleF(
                        cx - 17, cy - 15, 34, 28
                    );

                    using (LinearGradientBrush degradê = new LinearGradientBrush(
                        area, corClara, cor, 90f))
                    {
                        g.FillPath(degradê, interno);
                    }

                    // Contorno interno
                    using (Pen bordaInterna = new Pen(Color.FromArgb(220, 255, 255, 255), 1.3f))
                    {
                        g.DrawPath(bordaInterna, interno);
                    }

                    // Reflexo na parte superior
                    using (Pen brilho = new Pen(Color.FromArgb(180, 255, 255, 255), 2f))
                    {
                        brilho.StartCap = LineCap.Round;
                        brilho.EndCap = LineCap.Round;

                        g.DrawLine(
                            brilho,
                            cx - 9, cy - 7,
                            cx, cy - 13
                        );
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
