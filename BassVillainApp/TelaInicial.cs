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
    public partial class TelaInicial : Form
    {
        #region Variaveis Globais
        frm_TelaSelecaoMusica PagSelecaoMusc;
        #endregion

        #region Incialização do Formulário
        public TelaInicial()
        {
            InitializeComponent();
            configDaPagina();
            configEventoDosBotoes();
            configOutrasPag();

        }

        private void configDaPagina()
        {
            btn_Iniciar.Focus();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;

            this.FormClosing += new FormClosingEventHandler(TelaInicial_FormClosing);


        }

        private void configEventoDosBotoes()
        {
            btn_config.Click += new EventHandler(btn_config_Click);
            btn_Iniciar.Click += new EventHandler(btn_Iniciar_Click);
            btn_estatisticas.Click += new EventHandler(btn_estatisticas_Click);
        }

        private void configOutrasPag()
        {
            PagSelecaoMusc = new frm_TelaSelecaoMusica(this);
        }
        #endregion

        #region Evento dos Botões do Formulário
        private void btn_config_Click(object sender, EventArgs e)
        {
            msgBox("Botão Configurações clicado!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }

        private void btn_Iniciar_Click(object sender, EventArgs e)
        {
            msgBox("Botão Iniciar clicado!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            PagSelecaoMusc.Show();
            this.Hide();
        }

        private void btn_estatisticas_Click(object sender, EventArgs e)
        {
            msgBox("Botão Estatísticas clicado!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Propriedades do Formulário
        private void TelaInicial_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = msgBox("Deseja realmente sair?",
                    "Confirmação",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
        #endregion

        #region MessageBoxOptions
        private DialogResult msgBox(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(message, caption, buttons, icon);
        }


        #endregion

        #region Lixeira
        #endregion


    }
}
