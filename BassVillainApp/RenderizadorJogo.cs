using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;

namespace BassVillainApp
{
    /// <summary>Tudo que o desenho precisa saber, preenchido pela TelaJogo a cada frame.</summary>
    internal class EstadoRender
    {
        public IList<Nota> Notas = new List<Nota>();
        public long TempoAtualMs;
        public int TempoDeQuedaMs = 1500;

        public bool[] Pressionada = new bool[4];
        public string[] NomesTeclas = { "A", "S", "K", "L" };

        // Relógio "de tela" (ms) e o instante do último acerto de cada pista (para o brilho).
        public long AgoraMs;
        public long[] UltimoAcertoMs = { -100000, -100000, -100000, -100000 };

        public int Pontuacao;
        public int Acertos;
        public int Erros;
        public int ComboAtual;
        public int ComboMaximo;
        public float ProgressoMusica; // 0..1

        public AnimacaoRank Rank = new AnimacaoRank();
    }

    /// <summary>
    /// Desenha o jogo: fundo, pista em perspectiva (trapézio), botões de acerto,
    /// notas e o painel de informações.
    /// </summary>
    internal sealed class RenderizadorJogo : IDisposable
    {
        #region Ajustes visuais (mexa aqui)
        private const int NumPistas = 4;

        private const float MargemPainel = 12f;
        private const float AlturaPainel = 84f;

        // Quão pequena a pista é lá no fundo, comparada com junto aos botões (0.30 = 30%).
        private const float EscalaLonge = 0.30f;

        // Largura da pista junto aos botões, em fração da largura da tela.
        private const float LarguraPistaRel = 0.62f;

        // Distância da linha dos botões até a borda de baixo da tela.
        private const float FolgaInferior = 100f;

        // Notas "deitadas" na pista ficam mais achatadas (1 = sem achatar).
        private const float AchatamentoNota = 0.8f;

        // Triângulos apontando para cima (true) ou para baixo (false).
        private const bool TrianguloParaCima = true;

        private const int DuracaoBrilhoMs = 220;
        private const int PassoLinhasMs = 500; // linhas horizontais que correm pela pista
        #endregion

        private readonly TemaVisual tema;
        private Bitmap fundoCache;
        private Size fundoTamanho;

        public RenderizadorJogo(TemaVisual tema)
        {
            this.tema = tema;
        }

        public void Dispose()
        {
            if (fundoCache != null)
                fundoCache.Dispose();
        }

        #region Geometria da perspectiva
        // Perspectiva de verdade: um ponto de fuga acima da tela; quanto mais perto, maior (escala s).
        private sealed class Geometria
        {
            public readonly Size Tela;
            public readonly float Cx, YTopo, YAlvo, YFuga, MeiaBase, ZLonge, SBase;

            public Geometria(Size tela)
            {
                Tela = tela;
                Cx = tela.Width / 2f;
                YTopo = MargemPainel + AlturaPainel + 14f;
                YAlvo = tela.Height - FolgaInferior;
                ZLonge = 1f / EscalaLonge;
                YFuga = (YTopo - YAlvo * EscalaLonge) / (1f - EscalaLonge);
                MeiaBase = tela.Width * LarguraPistaRel / 2f;
                SBase = (tela.Height - YFuga) / (YAlvo - YFuga); // escala na borda de baixo da tela
            }

            // progresso da nota (0 = surgindo lá no fundo, 1 = em cima do botão) -> escala
            public float Escala(float progresso)
            {
                return 1f / (ZLonge - (ZLonge - 1f) * progresso);
            }

            public float Y(float escala) { return YFuga + (YAlvo - YFuga) * escala; }

            public float X(float escala, float u) { return Cx + u * MeiaBase * escala; }

            // posição lateral (-1..1) do centro de cada pista
            public float U(int pista) { return (pista + 0.5f) / NumPistas * 2f - 1f; }

            public float LarguraPistaBase { get { return 2f * MeiaBase / NumPistas; } }
        }
        #endregion

        public void Desenhar(Graphics g, Size tamanho, EstadoRender estado)
        {
            if (tamanho.Width < 100 || tamanho.Height < 200)
                return;

            SmoothingMode anterior = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var geo = new Geometria(tamanho);

            DesenharFundo(g, tamanho);
            DesenharPista(g, geo, estado);
            DesenharBotoes(g, geo, estado);
            DesenharNotas(g, geo, estado);
            DesenharPainel(g, tamanho, estado);

            // Rank do combo no espaço livre à esquerda da pista.
            float bordaEsquerda = geo.X(1f, -1f);
            estado.Rank.Desenhar(g,
                new RectangleF(12f, geo.YTopo + 70f, Math.Max(60f, bordaEsquerda - 24f), 110f), tema);

            g.SmoothingMode = anterior;
        }

        #region Fundo e pista
        private void DesenharFundo(Graphics g, Size t)
        {
            if (tema.ImagemFundo != null)
            {
                // Redimensiona uma vez só (e de novo se a janela mudar de tamanho).
                if (fundoCache == null || fundoTamanho != t)
                {
                    if (fundoCache != null)
                        fundoCache.Dispose();

                    fundoCache = new Bitmap(t.Width, t.Height);
                    using (Graphics gc = Graphics.FromImage(fundoCache))
                    {
                        gc.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gc.DrawImage(tema.ImagemFundo, 0, 0, t.Width, t.Height);
                    }
                    fundoTamanho = t;
                }
                g.DrawImage(fundoCache, 0, 0, t.Width, t.Height);
                return;
            }

            using (var degrade = new LinearGradientBrush(new Rectangle(0, 0, t.Width, t.Height),
                       TextoEstilizado.Clarear(tema.CorFundo, 0.25f), tema.CorFundo, 90f))
                g.FillRectangle(degrade, 0, 0, t.Width, t.Height);
        }

        private void DesenharPista(Graphics g, Geometria geo, EstadoRender e)
        {
            float sTopo = geo.Escala(0f);
            float yTopo = geo.YTopo;
            float yBase = geo.Tela.Height;

            var superficie = new[]
            {
                new PointF(geo.X(sTopo, -1f), yTopo),
                new PointF(geo.X(sTopo,  1f), yTopo),
                new PointF(geo.X(geo.SBase,  1f), yBase),
                new PointF(geo.X(geo.SBase, -1f), yBase)
            };

            // Superfície escura e translúcida: deixa o fundo da música aparecer.
            using (var degrade = new LinearGradientBrush(
                       new RectangleF(0f, yTopo, geo.Tela.Width, yBase - yTopo),
                       Color.FromArgb(110, 0, 0, 0), Color.FromArgb(200, 0, 0, 0), 90f))
                g.FillPolygon(degrade, superficie);

            // Linhas horizontais correndo em direção ao jogador (dão sensação de velocidade).
            long primeira = ((e.TempoAtualMs / PassoLinhasMs) + 1) * PassoLinhasMs;
            for (long tempo = primeira; tempo < e.TempoAtualMs + e.TempoDeQuedaMs; tempo += PassoLinhasMs)
            {
                float progresso = 1f - (tempo - e.TempoAtualMs) / (float)e.TempoDeQuedaMs;
                if (progresso < 0f || progresso > 1.3f)
                    continue;

                float s = geo.Escala(progresso);
                float y = geo.Y(s);
                int alfa = (int)(70 * Math.Min(1f, progresso * 4f));

                using (var pena = new Pen(Color.FromArgb(alfa, tema.CorContorno), Math.Max(1f, 2f * s)))
                    g.DrawLine(pena, geo.X(s, -1f), y, geo.X(s, 1f), y);
            }

            // Divisórias das pistas.
            using (var pena = new Pen(Color.FromArgb(110, tema.CorContorno), 1.5f))
            {
                for (int i = 1; i < NumPistas; i++)
                {
                    float u = (float)i / NumPistas * 2f - 1f;
                    g.DrawLine(pena, geo.X(sTopo, u), yTopo, geo.X(geo.SBase, u), yBase);
                }
            }

            // Bordas laterais (trilhos).
            using (var pena = new Pen(Color.FromArgb(230, tema.CorContorno), 4f))
            {
                g.DrawLine(pena, superficie[0], superficie[3]);
                g.DrawLine(pena, superficie[1], superficie[2]);
            }
        }
        #endregion

        #region Botões e notas
        private void DesenharBotoes(Graphics g, Geometria geo, EstadoRender e)
        {
            float largura = geo.LarguraPistaBase * 0.80f;
            float altura = largura * 0.86f;

            for (int pista = 0; pista < NumPistas; pista++)
            {
                float cx = geo.X(1f, geo.U(pista));
                float cy = geo.YAlvo;
                Color cor = tema.CorNota(pista);
                bool apertada = e.Pressionada[pista];

                float brilho = 1f - (e.AgoraMs - e.UltimoAcertoMs[pista]) / (float)DuracaoBrilhoMs;
                brilho = Math.Max(0f, Math.Min(1f, brilho));

                // Imagem do botão: "botao_press" quando apertado, senão "botao".
                Image imagem = apertada ? tema.ImagemPista("botao_press", pista) : null;
                bool trocouImagem = imagem != null;
                if (imagem == null)
                    imagem = tema.ImagemPista("botao", pista);

                if (imagem != null)
                {
                    float lado = largura * 1.1f;
                    g.DrawImage(imagem, cx - lado / 2f, cy - lado / 2f, lado, lado);

                    // Sem imagem própria para "apertado": um brilho no formato do botão.
                    if (apertada && !trocouImagem)
                    {
                        using (GraphicsPath forma = Triangulo(cx, cy, largura, altura))
                        using (var pena = new Pen(Color.FromArgb(150, cor), 8f))
                        {
                            pena.LineJoin = LineJoin.Round;
                            g.DrawPath(pena, forma);
                        }
                    }
                }
                else
                {
                    DesenharBotaoSemImagem(g, cx, cy, largura, altura, cor, apertada);
                }

                // Onda de brilho ao acertar uma nota.
                if (brilho > 0f)
                {
                    float crescer = 1f + 0.7f * (1f - brilho);
                    using (GraphicsPath onda = Triangulo(cx, cy, largura * crescer, altura * crescer))
                    using (var pena = new Pen(Color.FromArgb((int)(200 * brilho), TextoEstilizado.Clarear(cor, 0.6f)), 5f))
                    {
                        pena.LineJoin = LineJoin.Round;
                        g.DrawPath(pena, onda);
                    }
                }

                // Nome da tecla logo abaixo do botão.
                TextoEstilizado.Desenhar(g, tema, e.NomesTeclas[pista],
                    new RectangleF(cx - 40f, cy + altura / 2f + 4f, 80f, 30f),
                    18f, apertada ? Color.White : tema.CorTexto, StringAlignment.Center);
            }
        }

        private void DesenharBotaoSemImagem(Graphics g, float cx, float cy, float largura, float altura,
                                            Color cor, bool apertada)
        {
            using (GraphicsPath forma = Triangulo(cx, cy, largura, altura))
            {
                if (apertada)
                {
                    using (var brilho = new Pen(Color.FromArgb(110, cor), 14f))
                    {
                        brilho.LineJoin = LineJoin.Round;
                        g.DrawPath(brilho, forma);
                    }
                    using (var preench = new LinearGradientBrush(
                               new RectangleF(cx - largura / 2f, cy - altura / 2f, largura, altura),
                               TextoEstilizado.Clarear(cor, 0.6f), cor, LinearGradientMode.Vertical))
                        g.FillPath(preench, forma);
                    using (var borda = new Pen(Color.White, 4f))
                    {
                        borda.LineJoin = LineJoin.Round;
                        g.DrawPath(borda, forma);
                    }
                }
                else
                {
                    using (var preench = new SolidBrush(Color.FromArgb(70, cor)))
                        g.FillPath(preench, forma);
                    using (var borda = new Pen(cor, 4f))
                    {
                        borda.LineJoin = LineJoin.Round;
                        g.DrawPath(borda, forma);
                    }
                }
            }
        }

        private void DesenharNotas(Graphics g, Geometria geo, EstadoRender e)
        {
            // Só as notas visíveis, e do fundo para a frente (as mais perto cobrem as mais longe).
            var visiveis = new List<KeyValuePair<float, Nota>>();
            foreach (Nota nota in e.Notas)
            {
                if (nota.Acertada || nota.Perdida)
                    continue;

                float progresso = 1f - (nota.TempoAcertoMs - e.TempoAtualMs) / (float)e.TempoDeQuedaMs;
                if (progresso < 0f || progresso > 1.3f)
                    continue;

                visiveis.Add(new KeyValuePair<float, Nota>(progresso, nota));
            }
            visiveis.Sort((a, b) => a.Key.CompareTo(b.Key));

            foreach (KeyValuePair<float, Nota> item in visiveis)
            {
                float progresso = item.Key;
                int pista = item.Value.Pista;

                float s = geo.Escala(progresso);
                float cx = geo.X(s, geo.U(pista));
                float cy = geo.Y(s);
                float largura = geo.LarguraPistaBase * 0.80f * s;
                float altura = largura * 0.86f * AchatamentoNota;
                float opacidade = Math.Min(1f, progresso / 0.10f); // surge suavemente no fundo

                Image imagem = tema.ImagemPista("nota", pista);
                if (imagem != null)
                {
                    DesenharImagemComOpacidade(g, imagem,
                        new RectangleF(cx - largura * 0.55f, cy - altura * 0.55f, largura * 1.1f, altura * 1.1f),
                        opacidade);
                    continue;
                }

                Color cor = tema.CorNota(pista);
                int alfa = (int)(255 * opacidade);
                using (GraphicsPath forma = Triangulo(cx, cy, largura, altura))
                {
                    using (var preench = new LinearGradientBrush(
                               new RectangleF(cx - largura / 2f, cy - altura / 2f, largura, Math.Max(1f, altura)),
                               Color.FromArgb(alfa, TextoEstilizado.Clarear(cor, 0.5f)),
                               Color.FromArgb(alfa, cor), LinearGradientMode.Vertical))
                        g.FillPath(preench, forma);

                    using (var borda = new Pen(Color.FromArgb(alfa, 255, 255, 255), Math.Max(1.5f, 3f * s)))
                    {
                        borda.LineJoin = LineJoin.Round;
                        g.DrawPath(borda, forma);
                    }
                }
            }
        }

        private static void DesenharImagemComOpacidade(Graphics g, Image imagem, RectangleF destino, float opacidade)
        {
            if (opacidade >= 0.99f)
            {
                g.DrawImage(imagem, destino);
                return;
            }

            using (var atributos = new ImageAttributes())
            {
                var matriz = new ColorMatrix();
                matriz.Matrix33 = opacidade;
                atributos.SetColorMatrix(matriz);
                g.DrawImage(imagem,
                    new Rectangle((int)destino.X, (int)destino.Y, (int)destino.Width, (int)destino.Height),
                    0, 0, imagem.Width, imagem.Height, GraphicsUnit.Pixel, atributos);
            }
        }

        // Triângulo com os três cantos arredondados (curvas de Bézier nos vértices).
        private static GraphicsPath Triangulo(float cx, float cy, float largura, float altura)
        {
            PointF[] v = TrianguloParaCima
                ? new[]
                {
                    new PointF(cx, cy - altura / 2f),
                    new PointF(cx + largura / 2f, cy + altura / 2f),
                    new PointF(cx - largura / 2f, cy + altura / 2f)
                }
                : new[]
                {
                    new PointF(cx - largura / 2f, cy - altura / 2f),
                    new PointF(cx + largura / 2f, cy - altura / 2f),
                    new PointF(cx, cy + altura / 2f)
                };

            const float arredondar = 0.22f; // quanto de cada aresta vira curva
            var caminho = new GraphicsPath();
            PointF fimAnterior = PointF.Empty;

            for (int i = 0; i < 3; i++)
            {
                PointF anterior = v[(i + 2) % 3];
                PointF atual = v[i];
                PointF proximo = v[(i + 1) % 3];

                var entrada = new PointF(atual.X + (anterior.X - atual.X) * arredondar,
                                         atual.Y + (anterior.Y - atual.Y) * arredondar);
                var saida = new PointF(atual.X + (proximo.X - atual.X) * arredondar,
                                       atual.Y + (proximo.Y - atual.Y) * arredondar);
                var c1 = new PointF(entrada.X + (atual.X - entrada.X) * 0.6f,
                                    entrada.Y + (atual.Y - entrada.Y) * 0.6f);
                var c2 = new PointF(saida.X + (atual.X - saida.X) * 0.6f,
                                    saida.Y + (atual.Y - saida.Y) * 0.6f);

                if (i > 0)
                    caminho.AddLine(fimAnterior, entrada);
                caminho.AddBezier(entrada, c1, c2, saida);
                fimAnterior = saida;
            }

            caminho.CloseFigure();
            return caminho;
        }
        #endregion

        #region Painel de informações
        private void DesenharPainel(Graphics g, Size t, EstadoRender e)
        {
            var area = new RectangleF(MargemPainel, MargemPainel, t.Width - 2f * MargemPainel, AlturaPainel);

            if (tema.ImagemPainel != null)
            {
                g.DrawImage(tema.ImagemPainel, area);
            }
            else
            {
                using (GraphicsPath forma = RetanguloArredondado(area, 14f))
                {
                    using (var fundo = new LinearGradientBrush(area,
                               Color.FromArgb(225, TextoEstilizado.Clarear(tema.CorBotao, 0.1f)),
                               Color.FromArgb(235, tema.CorFundo), 90f))
                        g.FillPath(fundo, forma);

                    using (var borda = new Pen(tema.CorContorno, 2.5f))
                        g.DrawPath(borda, forma);
                }
            }

            RankCombo.Rank rank = RankCombo.Obter(e.ComboAtual);
            Color corCombo = rank == RankCombo.Rank.Nenhum ? tema.CorTexto : RankCombo.Cor(rank);

            // x e largura em fração do painel; os números são só dígitos (servem para num_N.png).
            DesenharCampo(g, area, 0.03f, 0.27f, "PONTUAÇÃO", e.Pontuacao.ToString(CultureInfo.InvariantCulture), 30f, tema.CorTexto);
            DesenharCampo(g, area, 0.32f, 0.17f, "COMBO", e.ComboAtual.ToString(CultureInfo.InvariantCulture), 30f, corCombo);
            DesenharCampo(g, area, 0.52f, 0.14f, "ACERTOS", e.Acertos.ToString(CultureInfo.InvariantCulture), 22f, tema.CorTexto);
            DesenharCampo(g, area, 0.67f, 0.13f, "ERROS", e.Erros.ToString(CultureInfo.InvariantCulture), 22f, tema.CorTexto);
            DesenharCampo(g, area, 0.81f, 0.16f, "MAIOR COMBO", e.ComboMaximo.ToString(CultureInfo.InvariantCulture), 22f, tema.CorTexto);

            // Barra de progresso da música, na base do painel.
            var barra = new RectangleF(area.X + 18f, area.Bottom - 12f, area.Width - 36f, 5f);
            using (var caminhoFundo = RetanguloArredondado(barra, 2.5f))
            using (var pincelFundo = new SolidBrush(Color.FromArgb(110, 0, 0, 0)))
                g.FillPath(pincelFundo, caminhoFundo);

            float preenchido = barra.Width * Math.Max(0f, Math.Min(1f, e.ProgressoMusica));
            if (preenchido > 1f)
            {
                using (var caminhoBarra = RetanguloArredondado(new RectangleF(barra.X, barra.Y, preenchido, barra.Height), 2.5f))
                using (var pincelBarra = new SolidBrush(tema.CorLinhaAlvo))
                    g.FillPath(pincelBarra, caminhoBarra);
            }
        }

        private void DesenharCampo(Graphics g, RectangleF painel, float xRel, float larguraRel,
                                   string titulo, string valor, float tamanhoValor, Color corValor)
        {
            float x = painel.X + painel.Width * xRel;
            float largura = painel.Width * larguraRel;

            TextoEstilizado.Rotulo(g, tema, titulo,
                new RectangleF(x, painel.Y + 8f, largura, 16f),
                12f, Color.FromArgb(200, tema.CorTexto), StringAlignment.Center);

            TextoEstilizado.Desenhar(g, tema, valor,
                new RectangleF(x, painel.Y + 24f, largura, 42f),
                tamanhoValor, corValor, StringAlignment.Center);
        }

        private static GraphicsPath RetanguloArredondado(RectangleF r, float raio)
        {
            float d = Math.Min(raio * 2f, Math.Min(r.Width, r.Height));
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
        #endregion
    }
}
