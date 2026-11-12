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
    public partial class TelaConclusao : FormularioBase
    {

        #region Globais
        double pctgAcertos;
        int dificuldade;
        #endregion

        #region Inicialização

        public TelaConclusao(ResultadoPartida resultado)
        {
            
            InitializeComponent();
            mostrarResultado(resultado);
            desbloquearNiveis(resultado);
            declrararComponentes();
        }
        #endregion

        #region Declarar componentes
        private void declrararComponentes()
        {
            btn_telaInicial.Click += btn_telaInicial_Click;
        }
        #endregion


        #region Botões
        private void btn_telaInicial_Click(object sender, EventArgs e)
        {
            TelaInicial telaInicial = new TelaInicial();   
            telaInicial.Show();
            this.Hide();
        }
        #endregion
        #region Mostrar Resultado
        private void mostrarResultado(ResultadoPartida resultado)
        {
            int totalNotas = resultado.tamanhoDaMusica;
            int acertos = resultado.NotasAcertadasResultadoPartida;
            pctgAcertos = totalNotas > 0 ? 100.0 * acertos / totalNotas: 0.0;
            bool passou = pctgAcertos > 70;

            lbl_NomeMusica.Text = resultado.NomeMusica;
            lb_resultados.Items.Add($"Perfect: {resultado.Perfect}");
            lb_resultados.Items.Add($"Great: {resultado.Great}");
            lb_resultados.Items.Add($"Good: {resultado.Good}");
            lb_resultados.Items.Add($"Miss: {resultado.Miss}");
            lb_resultados.Items.Add($"Pontuação: {resultado.Pontuacao}");
            lb_resultados.Items.Add($"Maior Combo: {resultado.MaiorCombo}");
            lb_resultados.Items.Add($"% de Acerto: {pctgAcertos.ToString("F2")}%");
            lb_resultados.Items.Add($"Passou: {passou}");
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
