
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BassVillainApp
{
    public class BotaoTema : Button
    {
        public enum Estado
        {
            Normal,
            Sobre,
            Apertado,
            Desabilitado
        }

        private bool sobre;
        private bool apertado;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TemaVisual Tema { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color CorDestaque { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float TamanhoTexto { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Compacto { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<Graphics> PintarFundoDoPai { get; set; }

        public BotaoTema()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor,
                true
            );

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;

            BackColor = Color.Transparent;
            ForeColor = Color.White;
            Cursor = Cursors.Hand;

            TamanhoTexto = 28f;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            sobre = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            sobre = false;
            apertado = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                apertado = true;

            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            apertado = false;
            Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }


        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Parent == null)
                return;

            GraphicsState estado = e.Graphics.Save();

            try
            {
                // Ajusta o desenho para as coordenadas do formulário.
                e.Graphics.TranslateTransform(-Left, -Top);

                PaintEventArgs args = new PaintEventArgs(
                    e.Graphics,
                    new Rectangle(Left, Top, Width, Height)
                );

                if (PintarFundoDoPai != null)
                {
                    PintarFundoDoPai(e.Graphics);
                }
                else
                {
                    // Redesenha o fundo e também os elementos
                    // que são pintados diretamente no formulário.
                    InvokePaintBackground(Parent, args);
                    InvokePaint(Parent, args);
                }
            }
            finally
            {
                e.Graphics.Restore(estado);
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            TemaVisual tema = Tema ?? TemaVisual.Menu;

            Color destaque = CorDestaque.IsEmpty
                ? tema.CorNota(0)
                : CorDestaque;

            Estado estado = !Enabled
                ? Estado.Desabilitado
                : apertado
                    ? Estado.Apertado
                    : sobre
                        ? Estado.Sobre
                        : Estado.Normal;

            Pintar(
                e.Graphics,
                ClientRectangle,
                Text,
                estado,
                tema,
                destaque,
                TamanhoTexto,
                Compacto
            );
        }

        internal static void Pintar(
            Graphics g,
            Rectangle area,
            string texto,
            Estado estado,
            TemaVisual tema,
            Color destaque,
            float tamanhoTexto,
            bool compacto = false)
        {
            SmoothingMode anterior = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle r = new Rectangle(
                area.X + 3,
                area.Y + 3,
                area.Width - 6,
                area.Height - 6
            );

            if (estado == Estado.Apertado)
                r.Offset(0, 2);

            bool realce = estado == Estado.Sobre ||
                          estado == Estado.Apertado;

            // Cores do botão.
            Color fundo = tema.CorBotao;
            Color fundoClaro = TextoEstilizado.Clarear(fundo, 0.12f);

            if (realce)
            {
                fundo = TextoEstilizado.Clarear(destaque, 0.12f);
                fundoClaro = TextoEstilizado.Clarear(destaque, 0.25f);
            }

            if (estado == Estado.Apertado)
            {
                fundo = tema.CorFundo;
                fundoClaro = TextoEstilizado.Clarear(tema.CorBotao, 0.05f);
            }

            // Corpo retangular, sem cantos arredondados.
            using (LinearGradientBrush pincel = new LinearGradientBrush(
                r,
                fundoClaro,
                fundo,
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(pincel, r);
            }

            // Contorno preto característico do estilo grafite.
            using (Pen contorno = new Pen(Color.FromArgb(245, 5, 4, 8), 3f))
            {
                g.DrawRectangle(contorno, r);
            }

            // Pequeno detalhe roxo na lateral.
            if (!compacto)
            {
                int altura = Math.Min(24, r.Height - 12);
                int largura = 6;

                Rectangle detalhe = new Rectangle(
                    r.X + 9,
                    r.Y + (r.Height - altura) / 2,
                    largura,
                    altura
                );

                using (SolidBrush pincel = new SolidBrush(destaque))
                    g.FillRectangle(pincel, detalhe);
            }

            // Texto.
            RectangleF areaTexto;

            if (compacto)
            {
                areaTexto = r;
            }
            else
            {
                areaTexto = new RectangleF(
                    r.X + r.Height * 0.55f,
                    r.Y,
                    r.Width - r.Height * 0.65f,
                    r.Height
                );
            }

            Color corTexto = estado == Estado.Desabilitado
                ? Color.FromArgb(130, tema.CorTexto)
                : Color.White;

            TextoEstilizado.Desenhar(
                g,
                tema,
                texto,
                areaTexto,
                tamanhoTexto,
                corTexto,
                StringAlignment.Center
            );

            // Linha roxa inferior, discreta.
            if (realce)
            {
                using (Pen linha = new Pen(destaque, 2f))
                {
                    g.DrawLine(
                        linha,
                        r.X + 5,
                        r.Bottom - 4,
                        r.Right - 5,
                        r.Bottom - 4
                    );
                }
            }

            g.SmoothingMode = anterior;
        }
    }
}
