using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Drawing.Text;
using System.Windows.Forms;

namespace BassVillainApp
{
    /// <summary>
    /// Aparência de uma tela: cores, fonte e imagens opcionais.
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
        public Color CorLinhaAlvo { get; private set; }   // destaques (barra de progresso etc.)
        public Color[] CoresNotas { get; private set; }   // uma cor por pista
        #endregion

        #region Fonte
        /// <summary>Nome da fonte do tema. Se não estiver instalada, usa uma fonte padrão.</summary>
        public string FamiliaFonte { get; private set; }

        private FontFamily familia;

        public FontFamily Familia
        {
            get
            {
                if (familia == null)
                    familia = EscolherFonte(FamiliaFonte);
                return familia;
            }
        }

        /// <summary>Primeira fonte da lista (separada por vírgula) que estiver instalada.</summary>
        private static FontFamily EscolherFonte(string lista)
        {
            FontFamily[] instaladas = new InstalledFontCollection().Families;
            foreach (string nome in lista.Split(','))
            {
                foreach (FontFamily f in instaladas)
                {
                    if (string.Equals(f.Name, nome.Trim(), StringComparison.OrdinalIgnoreCase))
                        return f;
                }
            }
            return FontFamily.GenericSansSerif;
        }

        /// <summary>Negrito, se a fonte tiver; senão normal (evita erro em fontes sem negrito).</summary>
        public FontStyle EstiloFonte
        {
            get { return Familia.IsStyleAvailable(FontStyle.Bold) ? FontStyle.Bold : FontStyle.Regular; }
        }

        /// <summary>Fonte do tema em pixels (quem chama deve dar Dispose).</summary>
        public Font CriarFonte(float tamanhoPx)
        {
            return new Font(Familia, tamanhoPx, EstiloFonte, GraphicsUnit.Pixel);
        }
        #endregion

        #region Imagens (null = não existe, usa as cores)
        public Image ImagemFundo { get; private set; }   // fundo.png  - fundo da tela
        public Image ImagemPainel { get; private set; }  // painel.png - painel de informações do jogo

        private readonly Dictionary<string, Image> cacheImagens = new Dictionary<string, Image>();

        /// <summary>
        /// Imagem por pista. Procura "nome_N.png" (N = 0..3) e, se não houver, "nome.png".
        /// Ex.: botao_0.png, botao.png, botao_press_2.png, nota.png
        /// </summary>
        public Image ImagemPista(string nome, int pista)
        {
            string chave = nome + "|" + pista;
            Image imagem;
            if (!cacheImagens.TryGetValue(chave, out imagem))
            {
                imagem = CarregarImagem(Pasta, nome + "_" + pista)
                         ?? CarregarImagem(Pasta, nome);
                cacheImagens[chave] = imagem;
            }
            return imagem;
        }

        /// <summary>Dígito opcional para números estilizados: num_0.png ... num_9.png.</summary>
        public Image ImagemDigito(char digito)
        {
            if (digito < '0' || digito > '9')
                return null;

            return ImagemUnica("num_" + digito);
        }

        /// <summary>Logo do jogo no menu (Imagens\Menu\titulo.png).</summary>
        public Image ImagemTitulo
        {
            get { return ImagemUnica("titulo"); }
        }

        /// <summary>Fundo dos botões do menu: menu_botao.png e menu_botao_hover.png.</summary>
        public Image ImagemBotaoMenu(bool sobre)
        {
            return ImagemUnica(sobre ? "menu_botao_hover" : "menu_botao");
        }

        /// <summary>Rank do combo: rank_C.png, rank_Aplus.png ...</summary>
        public Image ImagemRank(RankCombo.Rank rank)
        {
            return ImagemUnica(RankCombo.NomeDoArquivo(rank));
        }

        private Image ImagemUnica(string nome)
        {
            Image imagem;
            if (!cacheImagens.TryGetValue(nome, out imagem))
            {
                imagem = CarregarImagem(Pasta, nome);
                cacheImagens[nome] = imagem;
            }
            return imagem;
        }
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
                    t.FamiliaFonte = "Algerian, Bookman Old Style, Georgia"; // jazz: serifada decorativa
                    t.CorFundo = Color.FromArgb(32, 22, 12);
                    t.CorTexto = Color.FromArgb(255, 232, 170);
                    t.CorBotao = Color.FromArgb(70, 50, 25);
                    t.CorContorno = Color.FromArgb(190, 150, 60);
                    t.CorLinhaAlvo = Color.Goldenrod;
                    t.CoresNotas = new[]
                    {
                        Color.FromArgb(255, 205, 60), Color.FromArgb(255, 130, 40),
                        Color.FromArgb(235, 90, 110), Color.FromArgb(255, 235, 190)
                    };
                    break;

                case "DarkNecessities":
                    t.FamiliaFonte = "Showcard Gothic, Chiller, Impact";
                    t.CorFundo = Color.FromArgb(24, 8, 8);
                    t.CorTexto = Color.FromArgb(255, 210, 210);
                    t.CorBotao = Color.FromArgb(75, 20, 20);
                    t.CorContorno = Color.FromArgb(170, 50, 50);
                    t.CorLinhaAlvo = Color.Crimson;
                    t.CoresNotas = new[]
                    {
                        Color.FromArgb(235, 50, 60), Color.FromArgb(255, 140, 40),
                        Color.FromArgb(210, 50, 160), Color.FromArgb(150, 90, 235)
                    };
                    break;

                case "SevenNationArmy":
                    t.FamiliaFonte = "Stencil, Bauhaus 93, Arial Black"; // militar
                    t.CorFundo = Color.FromArgb(14, 14, 14);
                    t.CorTexto = Color.WhiteSmoke;
                    t.CorBotao = Color.FromArgb(45, 45, 45);
                    t.CorContorno = Color.Gray;
                    t.CorLinhaAlvo = Color.FromArgb(230, 40, 40);
                    t.CoresNotas = new[]
                    {
                        Color.FromArgb(230, 40, 40), Color.FromArgb(240, 240, 240),
                        Color.FromArgb(160, 165, 175), Color.FromArgb(150, 20, 30)
                    };
                    break;

                default: // "Menu"
                    t.FamiliaFonte = "Bauhaus 93, Segoe UI Black, Segoe UI";
                    t.CorFundo = Color.FromArgb(18, 18, 30);
                    t.CorTexto = Color.White;
                    t.CorBotao = Color.FromArgb(40, 40, 70);
                    t.CorContorno = Color.FromArgb(100, 100, 150);
                    t.CorLinhaAlvo = Color.Red;
                    t.CoresNotas = new[]
                    {
                        Color.FromArgb(0, 190, 255), Color.FromArgb(160, 110, 255),
                        Color.FromArgb(255, 90, 170), Color.FromArgb(90, 235, 220)
                    };
                    break;
            }

            t.ImagemFundo = CarregarFundo(NomeDoFundo(pasta));
            t.ImagemPainel = CarregarImagem(pasta, "painel");
            return t;
        }
        #endregion

        #region Imagens de fundo (Imagens\sultans.jpg, seven.jpg, dark.jpg, interface.jpg, stats.jpg)
        private static string NomeDoFundo(string pasta)
        {
            switch (pasta)
            {
                case "SultansOfSwing": return "sultans";
                case "SevenNationArmy": return "seven";
                case "DarkNecessities": return "dark";
                default: return "interface"; // menus, seleção e configurações
            }
        }

        private static Image fundoEstatisticas;
        private static bool fundoEstatisticasCarregado;

        /// <summary>Fundo da tela de estatísticas (Imagens\stats.jpg). Null = degradê do tema.</summary>
        public static Image ImagemEstatisticas
        {
            get
            {
                if (!fundoEstatisticasCarregado)
                {
                    fundoEstatisticas = CarregarFundo("stats");
                    fundoEstatisticasCarregado = true;
                }
                return fundoEstatisticas;
            }
        }

        /// <summary>Procura Imagens\nome.jpg (ou .jpeg / .png). Null se não existir.</summary>
        private static Image CarregarFundo(string nome)
        {
            foreach (string ext in new[] { ".jpg", ".jpeg", ".png" })
            {
                string caminho = Path.Combine(Application.StartupPath, "Imagens", nome + ext);
                if (!File.Exists(caminho))
                    continue;

                try
                {
                    using (Image original = Image.FromFile(caminho))
                        return new Bitmap(original);
                }
                catch (Exception ex) when (ex is IOException || ex is OutOfMemoryException ||
                                           ex is ArgumentException)
                {
                    // arquivo inválido: tenta o próximo / usa as cores
                }
            }
            return null;
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
                if (c is BotaoTema)
                {
                    ((BotaoTema)c).Tema = this; // desenha a si mesmo, no estilo do tema
                }
                else if (c is Button)
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
