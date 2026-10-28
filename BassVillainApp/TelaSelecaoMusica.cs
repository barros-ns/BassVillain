using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BassVillainApp
{
    public partial class frm_TelaSelecaoMusica : FormularioBase
    {

        #region Inicialização
        public frm_TelaSelecaoMusica(TelaInicial telaInicial)
        {
            InitializeComponent();
        }

        #endregion

        #region Propriedades do Formulário
        private void TelaSelecaoMusica_FormClosing(object sender, FormClosingEventArgs e)
        {
                if(msgBox("Deseja realmente sair?",
                    "Confirmação",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            else
            {
                e.Cancel = true;
            }
            
        }
        #endregion

        #region MessageBoxOptions
        private DialogResult msgBox(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(message, caption, buttons, icon);
        }

        #endregion
    }
}
