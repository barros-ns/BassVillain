using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
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
            var settings = Properties.Settings.Default;

            if (!settings.Tutorial) 
            {
                mostrarTutorial();
                settings.Tutorial = true;
                settings.Save();
            }

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

        #region Mostrar Tutorial
        private void mostrarTutorial()
        {
            Keys[] teclas = Configuracoes.Teclas;

            msgBox(
            "Bem-vindo ao Bass Villain!\n\n" +
            "Bass Villain é um jogo de ritmo inspirado em Guitar Hero. " +
            "As notas descem por quatro pistas. Quando uma nota chegar à linha de acerto, " +
            "pressione a tecla da pista correspondente.\n\n" +
            "Quanto mais perto do momento certo, melhor o julgamento: Perfect, Great ou Good.\n\n" +
            "Acerte várias notas seguidas para aumentar o combo. Conforme o combo cresce, " +
            "você sobe de rank, de C até SS+, e ganha mais pontos por acerto. " +
            "Se deixar uma nota passar, o combo volta a zero.",
            "Bass Villain | Como jogar",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
            );

            msgBox(
                "As pistas estão numeradas da esquerda para a direita.\n\n" +
                $"Pista 1: {Configuracoes.NomeDaTecla(teclas[0])}\n" +
                $"Pista 2: {Configuracoes.NomeDaTecla(teclas[1])}\n" +
                $"Pista 3: {Configuracoes.NomeDaTecla(teclas[2])}\n" +
                $"Pista 4: {Configuracoes.NomeDaTecla(teclas[3])}\n\n" +
                "Pressione Esc durante a partida para pausar. " +
                "Você pode trocar as teclas no menu Configurações.\n\n" +
                "Clique em OK para escolher sua primeira música.",
                "Bass Villain | Controles",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        #endregion

        #region Lixeira
        #endregion


    }
}
