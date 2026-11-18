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
        private static bool telaCheiaGlobal = false;
        private readonly ControleTelaCheia controleTelaCheia;
        public FormularioBase()
        {
            controleTelaCheia = new ControleTelaCheia(this);

            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            FormClosing += new FormClosingEventHandler(FormularioBase_FormClosing);
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        public void AlternarTelaCheiaGlobal()
        {
            telaCheiaGlobal = !telaCheiaGlobal;
            controleTelaCheia.Aplicar(telaCheiaGlobal);
        }

        protected override void OnVisibleChanged(System.EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible && controleTelaCheia != null)
            {
                controleTelaCheia.Aplicar(telaCheiaGlobal);
            }
        }

        public static bool TelaCheiaAtiva
        {
            get { return telaCheiaGlobal; }
        }

        // Cada tela pode escolher seu tema; o padrão é o do menu.
        protected virtual TemaVisual TemaDaTela
        {
            get { return TemaVisual.Menu; }
        }

        protected override void OnLoad(EventArgs e) //Define um tamanho fixo de formulário
        {
            Size = new System.Drawing.Size(800, 750);
            TemaDaTela.AplicarEm(this);
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
