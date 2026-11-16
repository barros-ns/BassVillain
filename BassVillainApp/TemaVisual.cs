using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BassVillainApp
{
    /// <summary>
    /// Aparência de uma tela: cores + imagens opcionais.
    /// Se uma imagem não existir na pasta Imagens\, o tema desenha com as cores (nada quebra).
    /// Para ter uma estética nova basta: (1) ajustar as cores em CriarTema e
    /// (2) soltar os PNGs na pasta do tema (veja Imagens\LEIA-ME.txt).
    /// </summary>
    public sealed class TemaVisual
    {
        #region Cores (usadas quando não há imagem, e para textos)
        public Color CorFundo { get; private set; }
        public Color CorTexto { get; private set; }
        public Color CorBotao { get; private set; }
        public Color CorContorno { get; private set; }
        public Color CorLinhaAlvo { get; private set; }
        public Color[] CoresNotas { get; private set; }
        #endregion

        #region Imagens (null = não existe, usa a cor)
        public Image ImagemFundo { get; private set; }
        public Image ImagemNota { get; private set; }
        public Image ImagemAlvo { get; private set; }
        #endregion

        public string Pasta { get; private set; }

        private TemaVisual(string pasta)
        {
            Pasta = pasta;
        }

        public Color CorNota(int pista)
        {
            return CoresNotas[pista % CoresNotas.Length];
        }

        #region Temas do jogo
        private static readonly Dictionary<string, TemaVisual> cache = new Dictionary<string, TemaVisual>();

        public static TemaVisual Menu
        {
            get { return Obter("Menu"); }
        }

        /// <summary>Tema da música (0 = Sultans, 1 = Dark Necessities, 2 = Seven Nation Army).</summary>
        public static TemaVisual ParaMusica(int musica)
        {
            switch (musica)
            {
                case 0: return Obter("SultansOfSwing");
                case 1: return Obter("DarkNecessities");
                case 2: return Obter("SevenNationArmy");
                default: throw new ArgumentOutOfRangeException("musica");
            }
        }

        private static TemaVisual Obter(string pasta)
        {
            TemaVisual tema;
            if (!cache.TryGetValue(pasta, out tema))
            {
                tema = CriarTema(pasta);
                cache[pasta] = tema;
            }
            return tema;
        }

        // >>> A ESTÉTICA DE CADA NÍVEL SE DEFINE AQUI <<<
        // Para todos os níveis iguais, devolva as mesmas cores em todos os casos.
        private static TemaVisual CriarTema(string pasta)
        {
            var t = new TemaVisual(pasta);

            switch (pasta)
            {
                case "SultansOfSwing":
                    t.CorFundo = Color.FromArgb(32, 22, 12);
                    t.CorTexto = Color.FromArgb(255, 232, 170);
                    t.CorBotao = Color.FromArgb(70, 50, 25);
                    t.CorContorno = Color.FromArgb(190, 150, 60);
                    t.CorLinhaAlvo = Color.Goldenrod;
                    t.CoresNotas = new[] { Color.Gold, Color.Orange, Color.Gold, Color.Orange };
                    break;

                case "DarkNecessities":
                    t.CorFundo = Color.FromArgb(24, 8, 8);
                    t.CorTexto = Color.FromArgb(255, 210, 210);
                    t.CorBotao = Color.FromArgb(75, 20, 20);
                    t.CorContorno = Color.FromArgb(170, 50, 50);
                    t.CorLinhaAlvo = Color.Crimson;
                    t.CoresNotas = new[] { Color.OrangeRed, Color.Crimson, Color.OrangeRed, Color.Crimson };
                    break;

                case "SevenNationArmy":
                    t.CorFundo = Color.FromArgb(14, 14, 14);
                    t.CorTexto = Color.WhiteSmoke;
                    t.CorBotao = Color.FromArgb(45, 45, 45);
                    t.CorContorno = Color.Gray;
                    t.CorLinhaAlvo = Color.White;
                    t.CoresNotas = new[] { Color.White, Color.Red, Color.White, Color.Red };
                    break;

                default: // "Menu"
                    t.CorFundo = Color.FromArgb(18, 18, 30);
                    t.CorTexto = Color.White;
                    t.CorBotao = Color.FromArgb(40, 40, 70);
                    t.CorContorno = Color.FromArgb(100, 100, 150);
                    t.CorLinhaAlvo = Color.Red;
                    t.CoresNotas = new[] { Color.DeepSkyBlue, Color.DeepSkyBlue, Color.DeepSkyBlue, Color.DeepSkyBlue };
                    break;
            }

            t.ImagemFundo = CarregarImagem(pasta, "fundo");
            t.ImagemNota = CarregarImagem(pasta, "nota");
            t.ImagemAlvo = CarregarImagem(pasta, "alvo");
            return t;
        }
        #endregion

        #region Imagens
        /// <summary>
        /// Procura Imagens\&lt;pasta&gt;\&lt;nome&gt;.png ao lado do .exe. Devolve null se não existir.
        /// </summary>
        public static Image CarregarImagem(string pasta, string nome)
        {
            string caminho = Path.Combine(Application.StartupPath, "Imagens", pasta, nome + ".png");

            if (!File.Exists(caminho))
                return null;

            try
            {
                // Copia para memória: assim o arquivo não fica travado pelo jogo.
                using (Image original = Image.FromFile(caminho))
                    return new Bitmap(original);
            }
            catch (Exception ex) when (ex is IOException || ex is OutOfMemoryException ||
                                       ex is ArgumentException)
            {
                return null; // PNG inválido: usa as cores.
            }
        }
        #endregion

        #region Aplicar em uma tela (menus)
        public void AplicarEm(Form form)
        {
            form.BackColor = CorFundo;
            form.ForeColor = CorTexto;

            if (ImagemFundo != null)
            {
                form.BackgroundImage = ImagemFundo;
                form.BackgroundImageLayout = ImageLayout.Stretch;
            }

            AplicarNosControles(form.Controls);
        }

        private void AplicarNosControles(Control.ControlCollection controles)
        {
            foreach (Control c in controles)
            {
                if (c is Button)
                {
                    var botao = (Button)c;
                    botao.FlatStyle = FlatStyle.Flat;
                    botao.BackColor = CorBotao;
                    botao.ForeColor = CorTexto;
                    botao.FlatAppearance.BorderColor = CorContorno;
                }
                else if (c is Label)
                {
                    c.BackColor = Color.Transparent; // deixa o fundo da tela aparecer
                    c.ForeColor = CorTexto;
                }
                else if (c is ListBox || c is TextBox)
                {
                    c.BackColor = CorBotao;
                    c.ForeColor = CorTexto;
                }

                if (c.HasChildren)
                    AplicarNosControles(c.Controls);
            }
        }
        #endregion
    }
}
