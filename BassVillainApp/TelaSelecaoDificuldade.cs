using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BassVillainApp.Properties;

namespace BassVillainApp
{

    public partial class TelaSelecaoDificuldade : FormularioBase
    {

        #region Variaveis Globais

        private String nomeMusicaSelecionada;
        private EnumMusica musicaEnum;
        frm_TelaSelecaoMusica telaAnterior;

        private BotaoTema[] botoes;
        private readonly string[] legendas = new string[3];

        // A tela usa o tema da música escolhida (fundo, fonte e cores).
        protected override TemaVisual TemaDaTela
        {
            get { return TemaVisual.ParaMusica(getMusica(musicaEnum)); }
        }

        #endregion


        #region Inicialização

        public TelaSelecaoDificuldade(EnumMusica musicaEnum, String musica, frm_TelaSelecaoMusica telaSelecaoMusica)
        {

            nomeMusicaSelecionada = musica;
            this.musicaEnum = musicaEnum;
            telaAnterior = telaSelecaoMusica;
            InitializeComponent();
            botoes = new[] { btn_facil, btn_medio, btn_dificil };
            checarNiveisDesbloqueados();
            configEventoDosBotoes();
            configDaPagina();
        }

        private void configDaPagina()
        {
            TemaVisual tema = TemaDaTela;
            string[] textos = { "FÁCIL", "MÉDIO", "DIFÍCIL" };

            for (int i = 0; i < botoes.Length; i++)
            {
                botoes[i].Tema = tema;
                botoes[i].CorDestaque = tema.CorNota(i);
                botoes[i].TamanhoTexto = 34f;
                botoes[i].Text = textos[i];
            }

            btn_voltar.Tema = tema;
            btn_voltar.CorDestaque = tema.CorNota(3);
            btn_voltar.TamanhoTexto = 22f;
            btn_voltar.Text = "VOLTAR";

            CancelButton = btn_voltar; // Esc volta
            Paint += TelaSelecaoDificuldade_Paint;
            SizeChanged += delegate { posicionarControles(); };

            // Ao voltar de uma partida, atualiza os desbloqueios e as melhores pontuações.
            VisibleChanged += delegate
            {
                if (Visible)
                {
                    checarNiveisDesbloqueados();
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
            int y = 250;
            foreach (BotaoTema b in botoes)
            {
                b.Size = new Size(520, 96);
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

                List<RegistroPartida> partidas = HistoricoPartidas.DaFase(historico, nomeMusicaSelecionada, i);

                legendas[i] = partidas.Count == 0
                    ? "Ainda não jogada"
                    : "Melhor: " + partidas.Max(r => r.Pontuacao).ToString("N0") + " pts   |   " +
                      partidas.Count + (partidas.Count == 1 ? " partida" : " partidas");
            }
        }

        private void TelaSelecaoDificuldade_Paint(object sender, PaintEventArgs e)
        {
            TemaVisual tema = TemaDaTela;
            DesenhoMenu.TituloDeTela(e.Graphics, tema, nomeMusicaSelecionada.ToUpper(),
                "SELECIONE A DIFICULDADE", ClientSize, 36f);

            for (int i = 0; i < botoes.Length; i++)
            {
                if (legendas[i] == null)
                    continue;

                DesenhoMenu.LegendaDoBotao(e.Graphics, tema, botoes[i], legendas[i],
                    botoes[i].Enabled ? tema.CorTexto : Color.Gray);
            }
        }

        private void checarNiveisDesbloqueados()
        {
            btn_facil.Enabled = false;
            btn_medio.Enabled = false;
            btn_dificil.Enabled = false;

            switch (nomeMusicaSelecionada)
            {
                case "Sultans of Swing":
                    checarSultansOfSwing();
                    break;
                case "Dark Necessities"     :
                    checarDarkNecessities();
                    break;
                case "Seven Nation Army":
                    checarSevenNationArmy();
                    break;
                default:
                    break;
            }
            Settings.Default.MusicaSelecionada = getMusica(musicaEnum);
        }

        private void configEventoDosBotoes()
        {
            btn_facil.Click += btn_facil_Click;
            btn_medio.Click += btn_medio_Click;
            btn_dificil.Click += btn_dificil_Click;
            btn_voltar.Click += btn_voltar_Click;
        }
        #endregion

        #region Checador de Níveis Desbloqueados Por Música

        private void checarSultansOfSwing()
        {
            btn_facil.Enabled = Properties.Settings.Default.SultansOfSwingFacilDesbloqueada;
            btn_medio.Enabled = Properties.Settings.Default.SultansOfSwingMedioDesbloqueada;
            btn_dificil.Enabled = Properties.Settings.Default.SultansOfSwingDificilDesbloqueada;
        }
        
        private void checarDarkNecessities()
        {
            btn_facil.Enabled = Properties.Settings.Default.DarkNecessitiesFacilDesbloqueada;
            btn_medio.Enabled = Properties.Settings.Default.DarkNecessitiesMedioDesbloqueada;
            btn_dificil.Enabled = Properties.Settings.Default.DarkNecessitiesDificilDesbloqueada;
        }

        private void checarSevenNationArmy()
        {
            btn_facil.Enabled = Properties.Settings.Default.SevenNationArmyFacilDesbloqueada;
            btn_medio.Enabled = Properties.Settings.Default.SevenNationArmyMedioDesbloqueada;
            btn_dificil.Enabled = Properties.Settings.Default.SevenNationArmyDificilDesbloqueada;
        }

        #endregion

        #region Evento dos Botões

        private void btn_facil_Click(object sender, EventArgs e)
        {
            Settings.Default.DificuldadeSelecionada = 0;
            TelaJogo telaJogo = new TelaJogo();
            telaJogo.Show();
            this.Hide();

        }
        private void btn_medio_Click(object sender, EventArgs e)
        {
            Settings.Default.DificuldadeSelecionada = 1;
            TelaJogo telaJogo = new TelaJogo();
            telaJogo.Show();
            this.Hide();
        }

        private void btn_dificil_Click(object sender, EventArgs e)
        {
            Settings.Default.DificuldadeSelecionada = 2;
            TelaJogo telaJogo = new TelaJogo();
            telaJogo.Show();
            this.Hide();
        }
        private void btn_voltar_Click(object sender, EventArgs e)
        {
            telaAnterior.Show();
            FecharParaVoltar();
        }
        #endregion

        #region Propriedades do Formulário
        #endregion

        #region Pegar nome da msuica
        private int getMusica(EnumMusica musicaEnum)
        {
            switch (musicaEnum)
            {
                case EnumMusica.SultansOfSwing:
                    return 0;
                case EnumMusica.DarkNecessities:
                    return 1;
                case EnumMusica.SevenNationArmy:
                    return 2;
                default:
                    throw new ArgumentOutOfRangeException(nameof(musicaEnum), musicaEnum, null);
            }
        }
        #endregion

    }
}
