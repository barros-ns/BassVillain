using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;

namespace BassVillainApp
{
    /// <summary>
    /// Desenha a tela de estatísticas mostrada ao fim da fase: resultado da partida
    /// e comparação com o histórico, no estilo (fundo, fonte, cores) da música jogada.
    /// </summary>
    internal sealed class RenderizadorEstatisticas : IDisposable
    {
        private readonly TemaVisual tema;
        private readonly FundoCache fundo = new FundoCache();

        private static readonly Color Verde = Color.FromArgb(110, 230, 130);
        private static readonly Color Vermelho = Color.FromArgb(255, 100, 100);
        private static readonly Color Ouro = Color.FromArgb(255, 215, 70);

        public RenderizadorEstatisticas(TemaVisual tema)
        {
            this.tema = tema;
        }

        /// <summary>Fundo da tela (Imagens\stats.jpg ou, sem ela, um degradê do tema).</summary>
        public void DesenharFundo(Graphics g, Size t)
        {
            fundo.Desenhar(g, tema, t, TemaVisual.ImagemEstatisticas);
        }

        public void Dispose()
        {
            fundo.Dispose();
        }

        public void Desenhar(Graphics g, Size t, RelatorioPartida r)
        {
            if (t.Width < 300 || t.Height < 500)
                return;

            SmoothingMode anterior = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            DesenharFundo(g, t);

            const float M = 20f;
            float largura = t.Width - 2f * M;

            DesenharCabecalho(g, new RectangleF(M, 16f, largura, 84f), r);
            DesenharPontuacaoERank(g, new RectangleF(M, 112f, largura, 140f), r);
            DesenharMetricas(g, new RectangleF(M, 264f, largura, 124f), r);
            DesenharPrecisao(g, new RectangleF(M, 398f, largura, 44f), r);
            DesenharHistorico(g, new RectangleF(M, 452f, largura, 168f), r);

            g.SmoothingMode = anterior;
        }

        #region Blocos
        private void DesenharCabecalho(Graphics g, RectangleF a, RelatorioPartida r)
        {
            Painel(g, a);

            TextoEstilizado.Desenhar(g, tema, r.Musica.ToUpperInvariant(),
                new RectangleF(a.X + 20f, a.Y + 8f, a.Width * 0.50f, 50f),
                36f, tema.CorTexto, StringAlignment.Near);

            TextoEstilizado.Rotulo(g, tema, "DIFICULDADE: " + r.NomeDificuldade.ToUpperInvariant(),
                new RectangleF(a.X + 22f, a.Y + 56f, a.Width * 0.5f, 22f),
                16f, Color.FromArgb(220, tema.CorTexto), StringAlignment.Near);

            TextoEstilizado.Desenhar(g, tema, r.Aprovado ? "FASE CONCLUÍDA!" : "TENTE NOVAMENTE",
                new RectangleF(a.X + a.Width * 0.54f, a.Y + 10f, a.Width * 0.46f - 22f, 42f),
                26f, r.Aprovado ? Verde : Vermelho, StringAlignment.Far);

            string detalhe = r.Simulado
                ? "PARTIDA SIMULADA (DEBUG) — NÃO SALVA NO HISTÓRICO"
                : "MÍNIMO PARA PASSAR: " + RelatorioPartida.PercentualParaPassar.ToString("0", CultureInfo.InvariantCulture) + "% DE ACERTO";
            TextoEstilizado.Rotulo(g, tema, detalhe,
                new RectangleF(a.X + a.Width * 0.30f, a.Y + 56f, a.Width * 0.70f - 22f, 22f),
                13f, Color.FromArgb(190, tema.CorTexto), StringAlignment.Far);
        }

        private void DesenharPontuacaoERank(Graphics g, RectangleF a, RelatorioPartida r)
        {
            // Cartão do rank
            var cartaoRank = new RectangleF(a.X, a.Y, 230f, a.Height);
            Painel(g, cartaoRank);
            TextoEstilizado.Rotulo(g, tema, "RANK DO COMBO",
                new RectangleF(cartaoRank.X, cartaoRank.Y + 8f, cartaoRank.Width, 18f),
                13f, Color.FromArgb(200, tema.CorTexto), StringAlignment.Center);

            Image imagemRank = r.Rank == RankCombo.Rank.Nenhum ? null : tema.ImagemRank(r.Rank);
            if (imagemRank != null)
            {
                g.DrawImage(imagemRank, cartaoRank.X + cartaoRank.Width / 2f - 55f, cartaoRank.Y + 30f, 110f, 110f);
            }
            else
            {
                string nome = RankCombo.Nome(r.Rank);
                TextoEstilizado.Desenhar(g, tema, nome,
                    new RectangleF(cartaoRank.X, cartaoRank.Y + 28f, cartaoRank.Width, 100f),
                    nome.Length > 2 ? 72f : 92f, RankCombo.Cor(r.Rank), StringAlignment.Center);
            }

            // Cartão da pontuação
            var cartaoPontos = new RectangleF(a.X + 246f, a.Y, a.Width - 246f, a.Height);
            Painel(g, cartaoPontos);
            TextoEstilizado.Rotulo(g, tema, "PONTUAÇÃO",
                new RectangleF(cartaoPontos.X, cartaoPontos.Y + 8f, cartaoPontos.Width, 18f),
                13f, Color.FromArgb(200, tema.CorTexto), StringAlignment.Center);

            TextoEstilizado.Desenhar(g, tema, r.Pontuacao.ToString(CultureInfo.InvariantCulture),
                new RectangleF(cartaoPontos.X, cartaoPontos.Y + 28f, cartaoPontos.Width, 70f),
                62f, tema.CorTexto, StringAlignment.Center);

            // Selo: novo recorde, recorde anterior ou primeira vez
            var selo = new RectangleF(cartaoPontos.X + cartaoPontos.Width / 2f - 130f, cartaoPontos.Bottom - 38f, 260f, 28f);
            if (r.NovoRecorde)
            {
                using (GraphicsPath forma = Formas.RetanguloArredondado(selo, 14f))
                {
                    using (var fundoSelo = new SolidBrush(Color.FromArgb(70, Ouro)))
                        g.FillPath(fundoSelo, forma);
                    using (var borda = new Pen(Ouro, 2f))
                        g.DrawPath(borda, forma);
                }
                TextoEstilizado.Rotulo(g, tema, "★ NOVO RECORDE! ★", selo, 16f, Ouro, StringAlignment.Center);
            }
            else if (r.RecordeAnterior.HasValue)
            {
                TextoEstilizado.Rotulo(g, tema, "RECORDE: " + r.RecordeAnterior.Value.ToString(CultureInfo.InvariantCulture),
                    selo, 16f, Color.FromArgb(210, tema.CorTexto), StringAlignment.Center);
            }
            else
            {
                TextoEstilizado.Rotulo(g, tema, "PRIMEIRA VEZ NESTA FASE", selo, 15f,
                    Color.FromArgb(210, tema.CorTexto), StringAlignment.Center);
            }
        }

        private void DesenharMetricas(Graphics g, RectangleF a, RelatorioPartida r)
        {
            string[] titulos = { "PERFECT", "GREAT", "GOOD", "MISS", "MAIOR COMBO", "ACERTOS" };
            string[] valores =
            {
                r.Perfect.ToString(CultureInfo.InvariantCulture),
                r.Great.ToString(CultureInfo.InvariantCulture),
                r.Good.ToString(CultureInfo.InvariantCulture),
                r.Miss.ToString(CultureInfo.InvariantCulture),
                r.MaiorCombo.ToString(CultureInfo.InvariantCulture),
                r.Acertos.ToString(CultureInfo.InvariantCulture) + " / " + r.TotalNotas.ToString(CultureInfo.InvariantCulture)
            };
            Color[] cores =
            {
                tema.CorNota(0), tema.CorNota(1), tema.CorNota(2), Vermelho,
                r.Rank == RankCombo.Rank.Nenhum ? tema.CorTexto : RankCombo.Cor(r.Rank), tema.CorTexto
            };

            const float folga = 12f;
            float larguraCartao = (a.Width - 2f * folga) / 3f;
            float alturaCartao = (a.Height - folga) / 2f;

            for (int i = 0; i < 6; i++)
            {
                var cartao = new RectangleF(
                    a.X + (i % 3) * (larguraCartao + folga),
                    a.Y + (i / 3) * (alturaCartao + folga),
                    larguraCartao, alturaCartao);

                Painel(g, cartao);
                TextoEstilizado.Rotulo(g, tema, titulos[i],
                    new RectangleF(cartao.X, cartao.Y + 5f, cartao.Width, 16f), 12f,
                    Color.FromArgb(200, tema.CorTexto), StringAlignment.Center);
                TextoEstilizado.Desenhar(g, tema, valores[i],
                    new RectangleF(cartao.X, cartao.Y + 20f, cartao.Width, cartao.Height - 22f),
                    28f, cores[i], StringAlignment.Center);
            }
        }

        private void DesenharPrecisao(Graphics g, RectangleF a, RelatorioPartida r)
        {
            Painel(g, a);

            TextoEstilizado.Rotulo(g, tema, "PRECISÃO",
                new RectangleF(a.X + 16f, a.Y, 100f, a.Height), 15f, tema.CorTexto, StringAlignment.Near);

            var barra = new RectangleF(a.X + 120f, a.Y + a.Height / 2f - 8f, a.Width - 250f, 16f);
            using (GraphicsPath fundoBarra = Formas.RetanguloArredondado(barra, 8f))
            {
                using (var pincel = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
                    g.FillPath(pincel, fundoBarra);
                using (var borda = new Pen(Color.FromArgb(140, tema.CorContorno), 1.5f))
                    g.DrawPath(borda, fundoBarra);
            }

            float cheio = barra.Width * (float)Math.Max(0.0, Math.Min(100.0, r.Percentual)) / 100f;
            if (cheio > 4f)
            {
                var preenchido = new RectangleF(barra.X, barra.Y, cheio, barra.Height);
                Color cor = r.Aprovado ? Verde : Vermelho;
                using (GraphicsPath formaCheia = Formas.RetanguloArredondado(preenchido, 8f))
                using (var degrade = new LinearGradientBrush(preenchido, TextoEstilizado.Clarear(cor, 0.45f), cor, 90f))
                    g.FillPath(degrade, formaCheia);
            }

            // Marca dos 70% (mínimo para passar).
            float xMarca = barra.X + barra.Width * (float)(RelatorioPartida.PercentualParaPassar / 100.0);
            using (var marca = new Pen(Color.FromArgb(230, 255, 255, 255), 2f))
                g.DrawLine(marca, xMarca, barra.Y - 5f, xMarca, barra.Bottom + 5f);

            TextoEstilizado.Desenhar(g, tema, r.Percentual.ToString("0.0", CultureInfo.InvariantCulture) + "%",
                new RectangleF(a.Right - 125f, a.Y + 2f, 110f, a.Height - 4f),
                22f, r.Aprovado ? Verde : Vermelho, StringAlignment.Far);
        }

        private void DesenharHistorico(Graphics g, RectangleF a, RelatorioPartida r)
        {
            Painel(g, a);

            TextoEstilizado.Rotulo(g, tema, "HISTÓRICO NESTA FASE",
                new RectangleF(a.X + 18f, a.Y + 8f, 300f, 20f), 14f, tema.CorTexto, StringAlignment.Near);

            // Coluna da esquerda: números do histórico.
            string[] rotulos = { "TENTATIVAS", "MÉDIA DE ACERTO", "RECORDE ANTERIOR" };
            string[] valores =
            {
                r.Tentativas.ToString(CultureInfo.InvariantCulture),
                r.Tentativas > 0 ? r.MediaPercentual.ToString("0.0", CultureInfo.InvariantCulture) + "%" : "—",
                r.RecordeAnterior.HasValue ? r.RecordeAnterior.Value.ToString(CultureInfo.InvariantCulture) : "—"
            };
            for (int i = 0; i < 3; i++)
            {
                float y = a.Y + 34f + i * 44f;
                TextoEstilizado.Rotulo(g, tema, rotulos[i], new RectangleF(a.X + 18f, y, 230f, 14f),
                    12f, Color.FromArgb(200, tema.CorTexto), StringAlignment.Near);
                TextoEstilizado.Desenhar(g, tema, valores[i], new RectangleF(a.X + 18f, y + 14f, 230f, 26f),
                    20f, tema.CorTexto, StringAlignment.Near);
            }

            // Coluna da direita: pontuação das últimas partidas.
            var area = new RectangleF(a.X + 270f, a.Y + 38f, a.Width - 292f, a.Height - 52f);
            TextoEstilizado.Rotulo(g, tema, "ÚLTIMAS PONTUAÇÕES",
                new RectangleF(area.X, a.Y + 8f, area.Width, 20f), 12f,
                Color.FromArgb(200, tema.CorTexto), StringAlignment.Far);

            if (r.UltimasPontuacoes.Count == 0)
            {
                TextoEstilizado.Rotulo(g, tema, "Sem partidas anteriores nesta fase", area, 15f,
                    Color.FromArgb(190, tema.CorTexto), StringAlignment.Center);
                return;
            }

            int n = r.UltimasPontuacoes.Count;
            int maior = Math.Max(1, r.UltimasPontuacoes.Max());
            float passo = area.Width / n;
            float larguraBarra = Math.Min(58f, passo * 0.62f);
            float alturaMax = area.Height - 22f;

            using (var linhaBase = new Pen(Color.FromArgb(150, tema.CorContorno), 2f))
                g.DrawLine(linhaBase, area.X, area.Bottom, area.Right, area.Bottom);

            for (int i = 0; i < n; i++)
            {
                int valor = r.UltimasPontuacoes[i];
                bool atual = !r.Simulado && i == n - 1;
                float altura = Math.Max(4f, alturaMax * valor / maior);
                var barra = new RectangleF(area.X + passo * i + (passo - larguraBarra) / 2f,
                                           area.Bottom - altura, larguraBarra, altura);

                Color cor = atual ? tema.CorNota(0) : tema.CorContorno;
                using (GraphicsPath forma = Formas.RetanguloArredondado(barra, 5f))
                using (var degrade = new LinearGradientBrush(
                           new RectangleF(barra.X, barra.Y, barra.Width, Math.Max(1f, barra.Height)),
                           Color.FromArgb(atual ? 255 : 190, TextoEstilizado.Clarear(cor, 0.4f)),
                           Color.FromArgb(atual ? 255 : 190, cor), 90f))
                {
                    g.FillPath(degrade, forma);
                    if (atual)
                        using (var borda = new Pen(Color.White, 2f))
                            g.DrawPath(borda, forma);
                }

                TextoEstilizado.Rotulo(g, tema, valor.ToString(CultureInfo.InvariantCulture),
                    new RectangleF(barra.X - 14f, barra.Y - 18f, barra.Width + 28f, 16f), 12f,
                    atual ? Color.White : Color.FromArgb(200, tema.CorTexto), StringAlignment.Center);
            }
        }
        #endregion

        // Cartão translúcido com contorno, igual ao painel de informações do jogo.
        private void Painel(Graphics g, RectangleF a)
        {
            using (GraphicsPath forma = Formas.RetanguloArredondado(a, 14f))
            {
                using (var fundoPainel = new LinearGradientBrush(a,
                           Color.FromArgb(215, TextoEstilizado.Clarear(tema.CorBotao, 0.08f)),
                           Color.FromArgb(230, tema.CorFundo), 90f))
                    g.FillPath(fundoPainel, forma);

                using (var borda = new Pen(tema.CorContorno, 2.5f))
                    g.DrawPath(borda, forma);
            }
        }
    }
}
