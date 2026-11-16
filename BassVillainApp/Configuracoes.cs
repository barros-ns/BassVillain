using System;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace BassVillainApp
{
    /// <summary>
    /// Preferências do jogador (volume, calibração, teclas).
    /// Ficam em um JSON em %AppData%\BassVillain\config.json.
    /// </summary>
    public static class Configuracoes
    {
        public const int NumeroDePistas = 4;
        public const int OffsetMinimoMs = -300;
        public const int OffsetMaximoMs = 300;

        public sealed class DadosConfiguracao
        {
            public int Volume { get; set; }
            public int OffsetMs { get; set; }
            public string[] Teclas { get; set; }
        }

        private static readonly string[] TeclasPadrao = { "A", "S", "K", "L" };

        private static DadosConfiguracao dados = Carregar();

        /// <summary>Pasta onde o jogo guarda seus dados (também usada pelo histórico).</summary>
        public static string PastaDeDados
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "BassVillain");
            }
        }

        private static string CaminhoArquivo
        {
            get { return Path.Combine(PastaDeDados, "config.json"); }
        }

        #region Propriedades

        /// <summary>Volume da música, de 0 a 100.</summary>
        public static int Volume
        {
            get { return dados.Volume; }
            set { dados.Volume = Math.Max(0, Math.Min(100, value)); }
        }

        /// <summary>Calibração de sincronia em ms (ver RelogioMusica.OffsetMs).</summary>
        public static int OffsetMs
        {
            get { return dados.OffsetMs; }
            set { dados.OffsetMs = Math.Max(OffsetMinimoMs, Math.Min(OffsetMaximoMs, value)); }
        }

        /// <summary>Teclas das 4 pistas (uma cópia, para ninguém alterar sem querer).</summary>
        public static Keys[] Teclas
        {
            get { return dados.Teclas.Select(t => (Keys)Enum.Parse(typeof(Keys), t)).ToArray(); }
        }

        #endregion

        #region Teclas

        public static bool TeclaEmUso(Keys tecla, int ignorarPista)
        {
            Keys[] atuais = Teclas;
            for (int i = 0; i < atuais.Length; i++)
            {
                if (i != ignorarPista && atuais[i] == tecla)
                    return true;
            }
            return false;
        }

        public static void DefinirTecla(int pista, Keys tecla)
        {
            if (pista < 0 || pista >= NumeroDePistas)
                throw new ArgumentOutOfRangeException("pista");

            dados.Teclas[pista] = tecla.ToString();
        }

        /// <summary>Texto amigável para mostrar a tecla na tela.</summary>
        public static string NomeDaTecla(Keys tecla)
        {
            if (tecla >= Keys.D0 && tecla <= Keys.D9)
                return ((int)tecla - (int)Keys.D0).ToString();

            switch (tecla)
            {
                case Keys.Left: return "←";
                case Keys.Right: return "→";
                case Keys.Up: return "↑";
                case Keys.Down: return "↓";
                case Keys.Space: return "Espaço";
                default: return tecla.ToString();
            }
        }

        #endregion

        #region Salvar e carregar

        public static void Salvar()
        {
            try
            {
                Directory.CreateDirectory(PastaDeDados);
                File.WriteAllText(CaminhoArquivo, new JavaScriptSerializer().Serialize(dados));
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                // Não conseguir salvar não pode derrubar o jogo: as preferências valem até fechar.
            }
        }

        public static void RestaurarPadrao()
        {
            dados = CriarPadrao();
        }

        private static DadosConfiguracao CriarPadrao()
        {
            return new DadosConfiguracao
            {
                Volume = 70,
                OffsetMs = 0,
                Teclas = (string[])TeclasPadrao.Clone()
            };
        }

        private static DadosConfiguracao Carregar()
        {
            try
            {
                if (File.Exists(CaminhoArquivo))
                {
                    var lido = new JavaScriptSerializer()
                        .Deserialize<DadosConfiguracao>(File.ReadAllText(CaminhoArquivo));

                    if (lido != null && TeclasValidas(lido.Teclas))
                    {
                        lido.Volume = Math.Max(0, Math.Min(100, lido.Volume));
                        lido.OffsetMs = Math.Max(OffsetMinimoMs, Math.Min(OffsetMaximoMs, lido.OffsetMs));
                        return lido;
                    }
                }
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException ||
                                       ex is ArgumentException || ex is InvalidOperationException)
            {
                // Arquivo ilegível ou corrompido: volta ao padrão.
            }

            return CriarPadrao();
        }

        // O arquivo pode ter sido editado à mão: confere tamanho, nomes e repetições.
        private static bool TeclasValidas(string[] teclas)
        {
            if (teclas == null || teclas.Length != NumeroDePistas)
                return false;

            Keys ignorado;
            if (!teclas.All(t => Enum.TryParse(t, out ignorado)))
                return false;

            return teclas.Distinct().Count() == NumeroDePistas;
        }

        #endregion
    }
}
