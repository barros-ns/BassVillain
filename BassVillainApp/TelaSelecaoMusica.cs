using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BassVillainApp
{
    public partial class frm_TelaSelecaoMusica : FormularioBase
    {
        #region Variaveis Globais
        TelaInicial telaInicial;
        #endregion


        #region Inicialização
        public frm_TelaSelecaoMusica(TelaInicial telaInicial)
        {
            InitializeComponent();
            checarDesbloqueioMusicas();
            configEventoDosBotoes();
            this.telaInicial = telaInicial;

        }

        #endregion

        #region Propriedades do Formulário
        private void checarDesbloqueioMusicas()
        {
            btn_Msc2.Enabled = Properties.Settings.Default.DarkNecessitiesFacilDesbloqueada;
            btn_Msc3.Enabled = Properties.Settings.Default.SevenNationArmyFacilDesbloqueada;
        }

        private void configEventoDosBotoes()
        {
            btn_Msc1.Click += btn_Msc1_Click;
            btn_Msc2.Click += btn_Msc2_Click;
            btn_Msc3.Click += btn_Msc3_Click; 
            btn_voltar.Click += btn_voltar_Click;
        }


        #endregion


        #region Eventos Botões
        private void btn_Msc1_Click(object sender, EventArgs e)
        {
            criarTelaDificuldade("Sultans of Swing");
        }

        private void btn_Msc2_Click(object sender, EventArgs e)
        {
           criarTelaDificuldade("Dark Necessities");
        }

        private void btn_Msc3_Click(object sender, EventArgs e)
        {
            criarTelaDificuldade("Seven Nation Army");
        }

        private void btn_voltar_Click(object sender, EventArgs e)
        {
            telaInicial.Show();
            this.Hide();
        }

        #endregion

        #region Criar Tela
        private void criarTelaDificuldade(string musicaSelecionada)
        {
            TelaSelecaoDificuldade telaDificuldade = new TelaSelecaoDificuldade(musicaSelecionada, this);
            telaDificuldade.Show();
            this.Hide();
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
