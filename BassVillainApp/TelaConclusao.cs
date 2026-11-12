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
        ResultadoPartida resultado;
        #endregion

        #region Inicialização

        public TelaConclusao(ResultadoPartida resultado)
        {
            
            InitializeComponent();
            mostrarResultado(resultado);
        }
        #endregion

        #region Mostrar Resultado
        private void mostrarResultado(ResultadoPartida resultado)
        {
            this.resultado = resultado;
            lb_resultados.Items.Add("Perfect: " + resultado.Perfect);
            lb_resultados.Items.Add("Great: " + resultado.Great);
            lb_resultados.Items.Add("Good: " + resultado.Good);
            lb_resultados.Items.Add("Miss: " + resultado.Miss);
            lb_resultados.Items.Add("Pontuação: " + resultado.Pontuacao);
        }

        #endregion
    }
}
