using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BassVillainApp.Properties;

namespace BassVillainApp
{
    public enum EnumMusica
    {
        SultansOfSwing = 0,
        DarkNecessities =1,
        SevenNationArmy = 2
    }

    public static class CatalogoMusicas
    {
        public static string GetMusica()
        {
        int musica = Settings.Default.MusicaSelecionada;
        String arquivo;

            switch (musica)
            {
                case 0:
                    arquivo = "SultansOfSwing.mp3";
                    break;
                case 1:
                    arquivo = "DarkNecessities.mp3";
                    break ;
                case 2:
                    arquivo = "SevenNationArmy.mp3";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(musica), musica, null);
            }
            return Path.Combine(Application.StartupPath, "MusicasMp3", arquivo);
        }
    }
}
