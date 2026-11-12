using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BassVillainApp
{
    public  class FormularioBase : Form
    {
        private bool fechandoParaVoltar;
        public FormularioBase()
        {
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            FormClosing += new FormClosingEventHandler(FormularioBase_FormClosing);
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            

        }

        protected override void OnLoad(EventArgs e) //Define um tamanho fixo de formulário
        {
            Size = new System.Drawing.Size(800, 700);
            base.OnLoad(e);
        }
        public void FormularioBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fechandoParaVoltar)
            {
                return;
            }

            // O encerramento do aplicativo não deve pedir confirmação de novo.
            if (e.CloseReason != CloseReason.UserClosing)
                return;

            DialogResult resposta = MessageBox.Show(
                this,
                "Deseja realmente sair?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Cancela este fechamento individual.
            e.Cancel = true;

            if (resposta == DialogResult.Yes)
            {
                // Encerra o aplicativo depois que este evento terminar.
                BeginInvoke(new MethodInvoker(Application.Exit));
            }
        }

        protected void FecharParaVoltar()
        {
            fechandoParaVoltar = true;
            Close();
        }
    }
}
