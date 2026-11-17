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
    /// <summary>
    /// Tela de estatísticas: aparece quando a fase termina e mostra as métricas da partida
    /// comparadas ao histórico (guardado em %AppData%\BassVillain\historico.json).
    /// </summary>
    public partial class TelaConclusao : FormularioBase
    {

        #region Globais
        double pctgAcertos;
        int dificuldade;

        private RelatorioPartida relatorio;
        private RenderizadorEstatisticas renderizador;
        private TemaVisual tema;

        // Mesmo visual da música que acabou de ser jogada.
        protected override TemaVisual TemaDaTela
        {
            get { return tema ?? (tema = TemaVisual.ParaMusica(Properties.Settings.Default.MusicaSelecionada)); }
        }
        #endregion

        #region Inicialização

        public TelaConclusao(ResultadoPartida resultado)
        {
            InitializeComponent();

            dificuldade = Properties.Settings.Default.DificuldadeSelecionada;
            relatorio = criarRelatorio(resultado);
            pctgAcertos = relatorio.Percentual;

            desbloquearNiveis(resultado);
            declrararComponentes();
        }

        // Compara com o histórico da mesma fase e, só então, grava esta partida
        // (partidas simuladas pelo debug aparecem na tela mas não são gravadas).
        private RelatorioPartida criarRelatorio(ResultadoPartida resultado)
        {
            List<RegistroPartida> todas = HistoricoPartidas.Carregar();
            List<RegistroPartida> daFase = HistoricoPartidas.DaFase(todas, resultado.NomeMusica, dificuldade);

            RelatorioPartida novo = RelatorioPartida.Criar(resultado, dificuldade, daFase);

            if (!novo.Simulado)
                HistoricoPartidas.Adicionar(todas, novo.ParaRegistro());

            return novo;
        }
        #endregion

        #region Declarar componentes
        private void declrararComponentes()
        {
            renderizador = new RenderizadorEstatisticas(TemaDaTela);

            pnl_Estatisticas.Paint += pnl_Estatisticas_Paint;
            btn_telaInicial.Click += btn_telaInicial_Click;
            AcceptButton = btn_telaInicial;
            FormClosed += delegate { renderizador.Dispose(); };
        }

        private void pnl_Estatisticas_Paint(object sender, PaintEventArgs e)
        {
            renderizador.Desenhar(e.Graphics, pnl_Estatisticas.ClientSize, relatorio);
        }
        #endregion

        #region Botões
        private void btn_telaInicial_Click(object sender, EventArgs e)
        {
            TelaInicial telaInicial = new TelaInicial();
            telaInicial.Show();
            FecharParaVoltar();
        }
        #endregion

        #region Desbloquear Níveis
        private void desbloquearNiveis(ResultadoPartida resultado)
        {
            dificuldade = Properties.Settings.Default.DificuldadeSelecionada;

            if (pctgAcertos >= 70)
            {
                switch (resultado.NomeMusica)
                {
                    case "Sultans of Swing":
                        desbloquearSultansOfSwing(); break;
                    case "Dark Necessities":
                        desbloqueaDarkNecessities(); break;
                    case "Seven Nation Army":
                        desbloquearSevenNationArmy(); break;
                }
                Properties.Settings.Default.Save();
            }
        }
            

        private void desbloquearSultansOfSwing()
        {
            switch(dificuldade)
            {
                case 0:
                    Properties.Settings.Default.SultansOfSwingMedioDesbloqueada = true;
                    Properties.Settings.Default.DarkNecessitiesFacilDesbloqueada = true;
                    
                    break;
                case 1:
                    Properties.Settings.Default.SultansOfSwingDificilDesbloqueada = true;
                    break;
            }
        }

        private void desbloqueaDarkNecessities()
        {
            switch(dificuldade)
            {
                case 0:
                    Properties.Settings.Default.DarkNecessitiesMedioDesbloqueada = true;
                    Properties.Settings.Default.SevenNationArmyFacilDesbloqueada = true;
                    break;
                case 1:
                    Properties.Settings.Default.DarkNecessitiesDificilDesbloqueada = true;
                    break;
            }
        }

        private void desbloquearSevenNationArmy()
        {
            switch(dificuldade)
            {
                case 0:
                    Properties.Settings.Default.SevenNationArmyMedioDesbloqueada = true;
                    break;
                case 1:
                    Properties.Settings.Default.SevenNationArmyDificilDesbloqueada = true;
                    break;
            }
        }

        #endregion
    }
}
