
using System.Drawing;
using System.Windows.Forms;

namespace BassVillainApp
{
    public sealed class ControleTelaCheia
    {
        private readonly Form formulario;

        private FormBorderStyle bordaOriginal;
        private FormWindowState estadoOriginal;
        private Rectangle limitesOriginais;
        private bool autoSizeOriginal;

        private bool estadoCapturado;
        private bool aplicado;

        public ControleTelaCheia(Form form)
        {
            formulario = form;
        }

        public void Aplicar(bool telaCheia)
        {
            if (telaCheia && !aplicado)
            {
                if (!estadoCapturado)
                {
                    bordaOriginal = formulario.FormBorderStyle;
                    estadoOriginal = formulario.WindowState;
                    limitesOriginais = formulario.Bounds;
                    autoSizeOriginal = formulario.AutoSize;

                    estadoCapturado = true;
                }

                formulario.AutoSize = false;
                formulario.WindowState = FormWindowState.Normal;
                formulario.FormBorderStyle = FormBorderStyle.None;
                formulario.Bounds = Screen.FromControl(formulario).Bounds;

                aplicado = true;
            }
            else if (!telaCheia && aplicado)
            {
                formulario.WindowState = FormWindowState.Normal;
                formulario.FormBorderStyle = bordaOriginal;
                formulario.Bounds = limitesOriginais;
                formulario.AutoSize = autoSizeOriginal;
                formulario.WindowState = estadoOriginal;

                aplicado = false;
            }
        }
    }
}
