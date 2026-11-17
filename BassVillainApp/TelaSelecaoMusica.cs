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

        // Os três botões, na ordem do EnumMusica (0 = Sultans, 1 = Dark Necessities, 2 = Seven Nation Army).
        private BotaoTema[] botoes;
        private readonly string[] nomes = { "Sultans of Swing", "Dark Necessities", "Seven Nation Army" };
        private readonly string[] legendas = new string[3];
        #endregion


        #region Inicialização
        public frm_TelaSelecaoMusica(TelaInicial telaInicial)
        {
            InitializeComponent();
            this.telaInicial = telaInicial;
            botoes = new[] { btn_Msc1, btn_Msc2, btn_Msc3 };

            checarDesbloqueioMusicas();
            configEventoDosBotoes();
            configDaPagina();
        }

        // Cada botão usa o tema (fonte, cores) da própria música.
        private void configDaPagina()
        {
            for (int i = 0; i < botoes.Length; i++)
            {
                TemaVisual tema = TemaVisual.ParaMusica(i);
                botoes[i].Tema = tema;
                botoes[i].CorDestaque = tema.CorNota(0);
                botoes[i].TamanhoTexto = 30f;
                botoes[i].Text = nomes[i].ToUpper();
            }

            btn_voltar.Tema = TemaVisual.Menu;
            btn_voltar.CorDestaque = TemaVisual.Menu.CorNota(2);
            btn_voltar.TamanhoTexto = 22f;
            btn_voltar.Text = "VOLTAR";

            CancelButton = btn_voltar; // Esc volta
            Paint += TelaSelecaoMusica_Paint;
            SizeChanged += delegate { posicionarControles(); };

            // Quando a tela volta a aparecer (depois de jogar), atualiza bloqueios e melhores pontuações.
            VisibleChanged += delegate
            {
                if (Visible)
                {
                    checarDesbloqueioMusicas();
                    atualizarLegendas();
                    Invalidate();
                }
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            posicionarControles();
            atualizarLegendas();
        }

        private void posicionarControles()
        {
            int y = 210;
            foreach (BotaoTema b in botoes)
            {
                b.Size = new Size(580, 96);
                b.Location = new Point((ClientSize.Width - b.Width) / 2, y);
                y += 128;
            }

            btn_voltar.Size = new Size(190, 56);
            btn_voltar.Location = new Point(30, ClientSize.Height - btn_voltar.Height - 24);
        }

        private void atualizarLegendas()
        {
            List<RegistroPartida> historico = HistoricoPartidas.Carregar();

            for (int i = 0; i < botoes.Length; i++)
            {
                if (!botoes[i].Enabled)
                {
                    legendas[i] = "BLOQUEADA";
                    continue;
                }

                string nome = nomes[i];
                List<RegistroPartida> partidas = historico.Where(r => r.Musica == nome).ToList();

                legendas[i] = partidas.Count == 0
                    ? "Ainda não jogada"
                    : "Melhor: " + partidas.Max(r => r.Pontuacao).ToString("N0") + " pts   |   " +
                      partidas.Count + (partidas.Count == 1 ? " partida" : " partidas");
            }
        }

        private void TelaSelecaoMusica_Paint(object sender, PaintEventArgs e)
        {
            DesenhoMenu.TituloDeTela(e.Graphics, TemaDaTela, "ESCOLHA A MÚSICA", null, ClientSize, 40f);

            for (int i = 0; i < botoes.Length; i++)
            {
                if (legendas[i] == null)
                    continue;

                DesenhoMenu.LegendaDoBotao(e.Graphics, TemaDaTela, botoes[i], legendas[i],
                    botoes[i].Enabled ? TemaDaTela.CorTexto : Color.Gray);
            }
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
            criarTelaDificuldade(EnumMusica.SultansOfSwing, "Sultans of Swing");
        }

        private void btn_Msc2_Click(object sender, EventArgs e)
        {
           criarTelaDificuldade(EnumMusica.DarkNecessities, "Dark Necessities");
        }

        private void btn_Msc3_Click(object sender, EventArgs e)
        {
            criarTelaDificuldade(EnumMusica.SevenNationArmy, "Seven Nation Army");
        }

        private void btn_voltar_Click(object sender, EventArgs e)
        {
            telaInicial.Show();
            FecharParaVoltar();
        }

        #endregion

        #region Criar Tela
        private void criarTelaDificuldade(EnumMusica enumMusica, String musicaSelecionada)
        {
            TelaSelecaoDificuldade telaDificuldade = new TelaSelecaoDificuldade(enumMusica,musicaSelecionada, this);
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
