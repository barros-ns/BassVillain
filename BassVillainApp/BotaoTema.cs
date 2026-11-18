using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BassVillainApp
{
    /// <summary>
    /// Botão desenhado no estilo do jogo: painel translúcido com contorno, triângulo
    /// arredondado de destaque (o mesmo formato dos botões da pista) e a fonte do tema.
    /// Imagens opcionais: Imagens\Menu\menu_botao.png e menu_botao_hover.png.
    /// </summary>
    public class BotaoTema : Button
    {
        public enum Estado { Normal, Sobre, Apertado, Desabilitado }

        private bool sobre;
        private bool apertado;

        /// Tema usado no desenho (se não definido, o do menu).
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TemaVisual Tema { get; set; }

        /// Cor do triângulo e do brilho. Vazia = primeira cor do tema.
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color CorDestaque { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float TamanhoTexto { get; set; }

        /// Botão pequeno (ex.: teclas): sem o triângulo, com o texto centralizado.
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Compacto { get; set; }

        /// Desenha o que fica atrás do botão quando o pai pinta o próprio fundo (ex.: tela de estatísticas).
        /// O Graphics já vem deslocado para as coordenadas do pai.
        
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<Graphics> PintarFundoDoPai { get; set; }

        public BotaoTema()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Cursor = Cursors.Hand;
            FlatStyle = FlatStyle.Flat;
            TamanhoTexto = 28f;

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
        }

        protected override void OnMouseEnter(EventArgs e) { sobre = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { sobre = false; apertado = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { apertado = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { apertado = false; Invalidate(); base.OnMouseUp(e); }
        protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }
        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

        // O fundo é pintado aqui, recortando o que há atrás do botão (imagem de fundo da tela ou cor).
        // Assim não herdamos textos desenhados pelo pai nem cantos pretos da transparência.
        
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (PintarFundoDoPai != null)
            {
                var estado = g.Save();
                g.TranslateTransform(-Left, -Top);
                PintarFundoDoPai(g);
                g.Restore(estado);
                return;
            }

            // Deixa o próprio WinForms reconstruir o fundo do controle,
            // respeitando imagens, cores e desenhos personalizados do formulário.
            base.OnPaintBackground(e);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            if (Parent != null)
            {
                using (SolidBrush fundo = new SolidBrush(Parent.BackColor))
                    e.Graphics.FillRectangle(fundo, ClientRectangle);
            }

            TemaVisual tema = Tema ?? TemaVisual.Menu;
            Color destaque = CorDestaque.IsEmpty ? tema.CorNota(0) : CorDestaque;

            Estado estado = !Enabled ? Estado.Desabilitado
                          : apertado ? Estado.Apertado
                          : (sobre || Focused) ? Estado.Sobre
                          : Estado.Normal;

            Pintar(e.Graphics, ClientRectangle, Text, estado, tema, destaque, TamanhoTexto, Compacto);
        }

        /// <summary>O desenho em si (estático, para poder ser testado sem janela).</summary>
        internal static void Pintar(Graphics g, Rectangle area, string texto, Estado estado,
                                    TemaVisual tema, Color destaque, float tamanhoTexto, bool compacto = false)
        {
            SmoothingMode anterior = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            bool realce = estado == Estado.Sobre || estado == Estado.Apertado;
            var r = new RectangleF(area.X + 4f, area.Y + 4f, area.Width - 8f, area.Height - 8f);
            if (estado == Estado.Apertado)
                r.Offset(0f, 2f);

            // Imagem própria do botão, se houver.
            Image imagem = tema.ImagemBotaoMenu(realce);
            if (imagem != null)
            {
                g.DrawImage(imagem, area);
                TextoEstilizado.Desenhar(g, tema, texto, r, tamanhoTexto, tema.CorTexto, StringAlignment.Center);
                g.SmoothingMode = anterior;
                return;
            }

            float raio = r.Height * 0.28f;
            using (GraphicsPath forma = Formas.RetanguloArredondado(r, raio))
            {
                // Brilho externo ao passar o mouse.
                if (realce)
                {
                    using (var brilho = new Pen(Color.FromArgb(estado == Estado.Apertado ? 50 : 85, destaque), 9f))
                    {
                        brilho.LineJoin = LineJoin.Round;
                        g.DrawPath(brilho, forma);
                    }
                }

                // Corpo: degradê escuro, mais claro quando em destaque.
                Color topo = TextoEstilizado.Clarear(tema.CorBotao, realce ? 0.30f : 0.12f);
                Color baixo = estado == Estado.Apertado
                    ? TextoEstilizado.Clarear(tema.CorFundo, 0.05f)
                    : tema.CorFundo;
                using (var corpo = new LinearGradientBrush(r, Color.FromArgb(230, topo), Color.FromArgb(240, baixo), 90f))
                    g.FillPath(corpo, forma);

                // Faixa de luz no topo (acabamento).
                var luz = new RectangleF(r.X + raio * 0.5f, r.Y + 3f, r.Width - raio, r.Height * 0.38f);
                using (GraphicsPath formaLuz = Formas.RetanguloArredondado(luz, raio * 0.7f))
                using (var pincelLuz = new LinearGradientBrush(luz,
                           Color.FromArgb(realce ? 55 : 35, 255, 255, 255), Color.FromArgb(0, 255, 255, 255), 90f))
                    g.FillPath(pincelLuz, formaLuz);

                // Contorno.
                using (var contorno = new Pen(realce ? destaque : tema.CorContorno, realce ? 3.5f : 2.5f))
                {
                    contorno.LineJoin = LineJoin.Round;
                    g.DrawPath(contorno, forma);
                }

                if (!compacto)
                {
                    // Triângulo de destaque, igual aos botões da pista.
                    float lado = r.Height * (realce ? 0.40f : 0.34f);
                    float cx = r.X + r.Height * 0.55f;
                    float cy = r.Y + r.Height / 2f;
                    using (GraphicsPath tri = Formas.TrianguloParaDireita(cx, cy, lado, lado * 1.1f))
                    {
                        using (var preench = new LinearGradientBrush(
                                   new RectangleF(cx - lado / 2f, cy - lado / 2f, lado, lado * 1.1f),
                                   TextoEstilizado.Clarear(destaque, 0.55f), destaque, 90f))
                            g.FillPath(preench, tri);
                        using (var borda = new Pen(Color.FromArgb(220, 255, 255, 255), 2f))
                        {
                            borda.LineJoin = LineJoin.Round;
                            g.DrawPath(borda, tri);
                        }
                    }

                }

                // Texto, deslocado para não cobrir o triângulo.
                float recuo = compacto ? 0f : r.Height * 1.0f;
                var areaTexto = new RectangleF(r.X + recuo, r.Y, r.Width - recuo - (compacto ? 0f : r.Height * 0.3f), r.Height);
                Color corTexto = realce ? Color.White : tema.CorTexto;
                TextoEstilizado.Desenhar(g, tema, texto, areaTexto, tamanhoTexto, corTexto, StringAlignment.Center);

                // Desabilitado: escurece por cima.
                if (estado == Estado.Desabilitado)
                {
                    using (var veu = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
                        g.FillPath(veu, forma);
                }
            }

            g.SmoothingMode = anterior;
        }
    }
}
