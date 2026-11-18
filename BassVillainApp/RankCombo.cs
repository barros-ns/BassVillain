using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace BassVillainApp
{
    /// <summary>
    /// Ranks do combo atual: C, B, A, A+, S, SS e SS+.
    /// Para mudar a dificuldade de subir de rank, edite os números em "faixas".
    /// </summary>
    public static class RankCombo
    {
        public enum Rank { Nenhum = 0, C, B, A, APlus, S, SS, SSPlus }

        private sealed class Faixa
        {
            public readonly int ComboMinimo;
            public readonly string Nome;
            public readonly string Arquivo; // nome do PNG opcional: Imagens\<tema>\<Arquivo>.png
            public readonly Color Cor;      // cor usada quando não há imagem

            public Faixa(int comboMinimo, string nome, string arquivo, Color cor)
            {
                ComboMinimo = comboMinimo;
                Nome = nome;
                Arquivo = arquivo;
                Cor = cor;
            }
        }

        // Na mesma ordem do enum (sem o "Nenhum"). Combos mínimos em ordem crescente.
        private static readonly Faixa[] faixas =
        {
            new Faixa(10,  "C",   "rank_C",      Color.LightGray),
            new Faixa(25,  "B",   "rank_B",      Color.LimeGreen),
            new Faixa(50,  "A",   "rank_A",      Color.DeepSkyBlue),
            new Faixa(75,  "A+",  "rank_Aplus",  Color.Cyan),
            new Faixa(100, "S",   "rank_S",      Color.Gold),
            new Faixa(150, "SS",  "rank_SS",     Color.Orange),
            new Faixa(200, "SS+", "rank_SSplus", Color.Magenta)
        };

        public static Rank Obter(int combo)
        {
            Rank rank = Rank.Nenhum;
            for (int i = 0; i < faixas.Length; i++)
            {
                if (combo >= faixas[i].ComboMinimo)
                    rank = (Rank)(i + 1);
            }
            return rank;
        }

        public static string Nome(Rank rank)
        {
            return rank == Rank.Nenhum ? "-" : faixas[(int)rank - 1].Nome;
        }

        public static Color Cor(Rank rank)
        {
            return rank == Rank.Nenhum ? Color.Gray : faixas[(int)rank - 1].Cor;
        }

        public static string NomeDoArquivo(Rank rank)
        {
            return faixas[(int)rank - 1].Arquivo;
        }
    }

    /// <summary>
    /// Desenha e anima o rank: "pop" com brilho ao subir, encolhe e some ao cair,
    /// e um pulinho a cada nota acertada.
    /// </summary>
    internal class AnimacaoRank
    {
        private const int DuracaoSubidaMs = 450;
        private const int DuracaoQuedaMs = 400;
        private const int DuracaoPulsoMs = 140;
        private const float TamanhoFonte = 40f;
        private const float TamanhoImagem = 64f;

        // Animação em tempo real (independe do relógio da música).
        private readonly Stopwatch relogio = Stopwatch.StartNew();

        private RankCombo.Rank atual = RankCombo.Rank.Nenhum;
        private RankCombo.Rank anterior = RankCombo.Rank.Nenhum;
        private bool subiu;

        private long inicioMudancaMs = -100000;
        private long inicioPulsoMs = -100000;

        /// <summary>Chamar sempre que o combo mudar (ou a cada tick: só reage a mudança de rank).</summary>
        public void Atualizar(int comboAtual)
        {
            RankCombo.Rank novo = RankCombo.Obter(comboAtual);

            if (novo == atual)
                return;

            anterior = atual;
            subiu = novo > atual;
            atual = novo;
            inicioMudancaMs = relogio.ElapsedMilliseconds;
        }

        /// <summary>Pulinho rápido, chamado a cada nota acertada.</summary>
        public void Pulsar()
        {
            inicioPulsoMs = relogio.ElapsedMilliseconds;
        }

        public void Desenhar(Graphics g, RectangleF area, TemaVisual tema)
        {
            long agora = relogio.ElapsedMilliseconds;
            int duracao = subiu ? DuracaoSubidaMs : DuracaoQuedaMs;
            float t = Math.Min(1f, Math.Max(0f, (agora - inicioMudancaMs) / (float)duracao));
            bool animando = t < 1f;

            // Rank que acabou de ser perdido: encolhe e some.
            if (!subiu && animando && anterior != RankCombo.Rank.Nenhum)
                DesenharRank(g, area, tema, anterior, 1f - 0.5f * t, 1f - t, 0f);

            if (atual != RankCombo.Rank.Nenhum)
            {
                float escala = 1f;
                float opacidade = 1f;
                float brilho = 0f;

                if (animando && subiu)
                {
                    float inverso = 1f - t;
                    escala = 1f + 0.4f * inverso * inverso; // começa maior e assenta (ease-out)
                    brilho = inverso;
                }
                else if (animando)
                {
                    opacidade = t; // apareceu por uma queda: surge aos poucos
                }

                float tp = Math.Min(1f, (agora - inicioPulsoMs) / (float)DuracaoPulsoMs);
                escala *= 1f + 0.12f * (1f - tp);

                DesenharRank(g, area, tema, atual, escala, opacidade, brilho);
            }

        }

        private void DesenharRank(Graphics g, RectangleF area, TemaVisual tema,
                                  RankCombo.Rank rank, float escala, float opacidade, float brilho)
        {
            float cx = area.X + area.Width / 2f;
            float cy = area.Y + area.Height * 0.5f;
            int alfa = (int)(255 * Math.Max(0f, Math.Min(1f, opacidade)));

            Image imagem = tema.ImagemRank(rank);

            if (imagem != null)
            {
                float lado = TamanhoImagem * escala;
                var destino = new Rectangle(
                    (int)(cx - lado / 2f), (int)(cy - lado / 2f), (int)lado, (int)lado);

                using (var atributos = new ImageAttributes())
                {
                    var matriz = new ColorMatrix();
                    matriz.Matrix33 = alfa / 255f;
                    atributos.SetColorMatrix(matriz);
                    g.DrawImage(imagem, destino, 0, 0, imagem.Width, imagem.Height,
                                GraphicsUnit.Pixel, atributos);
                }
                return;
            }

            // Sem imagem: desenha o texto do rank com contorno (legível em qualquer fundo).
            Color cor = RankCombo.Cor(rank);
            SmoothingMode modoAnterior = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var caminho = new GraphicsPath())
            {
                caminho.AddString(RankCombo.Nome(rank), tema.Familia,
                                  (int)tema.EstiloFonte, TamanhoFonte, PointF.Empty,
                                  StringFormat.GenericTypographic);

                RectangleF limites = caminho.GetBounds();
                using (var centralizar = new Matrix())
                {
                    centralizar.Translate(-(limites.X + limites.Width / 2f),
                                          -(limites.Y + limites.Height / 2f));
                    caminho.Transform(centralizar);
                }
                using (var posicionar = new Matrix())
                {
                    posicionar.Scale(escala, escala);
                    posicionar.Translate(cx, cy, MatrixOrder.Append);
                    caminho.Transform(posicionar);
                }

                if (brilho > 0f)
                {
                    using (var pincelBrilho = new Pen(Color.FromArgb((int)(alfa * 0.5f * brilho), cor), 4f + 12f * brilho))
                    {
                        pincelBrilho.LineJoin = LineJoin.Round;
                        g.DrawPath(pincelBrilho, caminho);
                    }
                }

                RectangleF limitesFinais = caminho.GetBounds();
                using (var contorno = new Pen(Color.FromArgb(alfa, Color.Black), 4f))
                using (var preenchimento = new LinearGradientBrush(limitesFinais,
                           Color.FromArgb(alfa, TextoEstilizado.Clarear(cor, 0.55f)),
                           Color.FromArgb(alfa, cor), LinearGradientMode.Vertical))
                {
                    contorno.LineJoin = LineJoin.Round;
                    g.DrawPath(contorno, caminho);
                    g.FillPath(preenchimento, caminho);
                }
            }

            g.SmoothingMode = modoAnterior;
        }
    }
}
