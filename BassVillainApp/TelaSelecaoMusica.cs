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
    public partial class frm_TelaSelecaoMusica : Form
    {
        public frm_TelaSelecaoMusica(TelaInicial telaInicial)
        {
            InitializeComponent();
        }

        private void configDaPagina()
        {
            btn_Msc1.Focus();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;

            this.FormClosing += new FormClosingEventHandler(TelaSelecaoMusica_FormClosing);


        }

        #region Propriedades do Formulário
        private void TelaSelecaoMusica_FormClosing(object sender, FormClosingEventArgs e)
        {
                DialogResult result = msgBox("Deseja realmente sair?",
                    "Confirmação",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
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
