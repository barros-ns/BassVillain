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
    public partial class TelaInicial : FormularioBase
    {
        #region Variaveis Globais
        
        #endregion

        #region Incialização do Formulário
        public TelaInicial()
        {
            InitializeComponent();
            configDaPagina();
            configEventoDosBotoes();

        }

        private void configDaPagina()
        {
            // Botões grandes, cada um com a cor de uma pista do jogo.
            btn_Iniciar.TamanhoTexto = 36f;
            btn_Iniciar.CorDestaque = TemaVisual.Menu.CorNota(0);
            btn_config.TamanhoTexto = 28f;
            btn_config.CorDestaque = TemaVisual.Menu.CorNota(1);

            AcceptButton = btn_Iniciar;
            Paint += TelaInicial_Paint;
            SizeChanged += delegate { posicionarControles(); };
        }

        // Título no estilo do jogo (ou a logo Imagens\Menu\titulo.png, se existir).
        private void TelaInicial_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            DesenhoMenu.Titulo(e.Graphics, TemaDaTela, ClientSize);
        }

        // A janela só ganha o tamanho final no Load, então os botões são centralizados depois dele.
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            posicionarControles();
            btn_Iniciar.Select();
        }

        private void posicionarControles()
        {
            btn_Iniciar.Location = new Point((ClientSize.Width - btn_Iniciar.Width) / 2, 376);
            btn_config.Location = new Point((ClientSize.Width - btn_config.Width) / 2, btn_Iniciar.Bottom + 22);
        }

        private void configEventoDosBotoes()
        {
            btn_config.Click += btn_config_Click;
            btn_Iniciar.Click += btn_Iniciar_Click;
        }

        #endregion

        #region Evento dos Botões do Formulário
        private void btn_config_Click(object sender, EventArgs e)
        {
            frm_Configuracoes telaConfig = new frm_Configuracoes(this);
            telaConfig.Show();
            this.Hide();
        }

        private void btn_Iniciar_Click(object sender, EventArgs e)
        {
            frm_TelaSelecaoMusica PagSelecaoMusc = new frm_TelaSelecaoMusica(this);
            PagSelecaoMusc.Show();
            this.Hide();
        }

        #endregion

        #region Propriedades do Formulário
        

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
