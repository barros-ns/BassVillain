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
        bool DarkNecessitiesFacilDesbloqueada = Properties.Settings.Default.DarkNecessitiesFacilDesbloqueada;
        bool SevenNationArmyFacilDesbloqueada = Properties.Settings.Default.SevenNationArmyFacilDesbloqueada;
        #endregion


        #region Inicialização
        public frm_TelaSelecaoMusica(TelaInicial telaInicial)
        {
            InitializeComponent();
            checarDesbloqueioMusicas();
        }

        #endregion

        #region Propriedades do Formulário
        private void checarDesbloqueioMusicas()
        {
            btn_Msc2.Enabled = DarkNecessitiesFacilDesbloqueada;
            btn_Msc3.Enabled = SevenNationArmyFacilDesbloqueada;
        }



        #endregion


        #region Eventos Botões
        private void btn_Msc1_Click(object sender, EventArgs e)
        {
            // Lógica para iniciar a música 1
            MessageBox.Show("Iniciando música 1: 'Song Title 1'");
        }

        private void btn_Msc2_Click(object sender, EventArgs e)
        {
            // Lógica para iniciar a música 2
            MessageBox.Show("Iniciando música 2: 'Dark Necessities'");
        }

        private void btn_Msc3_Click(object sender, EventArgs e)
        {
            // Lógica para iniciar a música 3
            MessageBox.Show("Iniciando música 3: 'Seven Nation Army'");
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
