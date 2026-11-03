using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BassVillainApp
{
    internal class Nota
    {
        public int Pista { get; set; }
        public int TempoAcertoMs { get; set; }

        public Nota(int pista, int tempoAcertoMs)
        {
            Pista = pista;
            TempoAcertoMs = tempoAcertoMs;
        }
    }
}
