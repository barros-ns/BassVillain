using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BassVillainApp
{
    public class ResultadoPartida
    {
        public int Perfect { get; set; }
        public int Great { get; set; }
        public int Good { get; set; }
        public int Miss { get; set; }
        public int Pontuacao { get; set; }
        public int MaiorCombo { get; set; }
        public String NomeMusica { get; set; }
        public int tamanhoDaMusica { get; set; }

        // Partida criada pelos botões de debug (Vencer/Perder): não entra no histórico.
        public bool Simulado { get; set; }

        public int NotasAcertadasResultadoPartida
        {
            get { return Perfect + Great + Good; }
        }

    }
}
