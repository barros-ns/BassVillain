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

        #endregion


        #region Inicialização

        public TelaSelecaoDificuldade(EnumMusica musicaEnum, String musica, frm_TelaSelecaoMusica telaSelecaoMusica)
        {

            nomeMusicaSelecionada = musica;
            this.musicaEnum = musicaEnum;
            telaAnterior = telaSelecaoMusica;
            InitializeComponent();
            MudarNomeDoFormulario();
            checarNiveisDesbloqueados();
            configEventoDosBotoes();
        }

        private void MudarNomeDoFormulario()
        {
            lbl_nomeDaMusica.Text = nomeMusicaSelecionada;
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
