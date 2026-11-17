using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace BassVillainApp
{
    /// <summary>Uma partida concluída, como fica gravada no histórico.</summary>
    public sealed class RegistroPartida
    {
        public string Data { get; set; }
        public string Musica { get; set; }
        public int Dificuldade { get; set; }
        public int Pontuacao { get; set; }
        public int Perfect { get; set; }
        public int Great { get; set; }
        public int Good { get; set; }
        public int Miss { get; set; }
        public int MaiorCombo { get; set; }
        public int TotalNotas { get; set; }
        public double Percentual { get; set; }
        public bool Aprovado { get; set; }
    }

    /// <summary>
    /// Histórico de partidas: um JSON em %AppData%\BassVillain\historico.json
    /// (a mesma pasta das configurações). Guarda as últimas partidas de todas as fases.
    /// </summary>
    public static class HistoricoPartidas
    {
        private const int LimiteDeRegistros = 500;

        private static string Caminho
        {
            get { return Path.Combine(Configuracoes.PastaDeDados, "historico.json"); }
        }

        public static List<RegistroPartida> Carregar()
        {
            try
            {
                if (File.Exists(Caminho))
                {
                    var lidos = new JavaScriptSerializer().Deserialize<List<RegistroPartida>>(File.ReadAllText(Caminho));
                    if (lidos != null)
                        return lidos;
                }
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException ||
                                       ex is ArgumentException || ex is InvalidOperationException)
            {
                // Arquivo ilegível ou corrompido: recomeça um histórico vazio.
            }

            return new List<RegistroPartida>();
        }

        /// <summary>Só as partidas de uma música numa dificuldade, da mais antiga para a mais recente.</summary>
        public static List<RegistroPartida> DaFase(IEnumerable<RegistroPartida> todos, string musica, int dificuldade)
        {
            return todos.Where(r => r.Musica == musica && r.Dificuldade == dificuldade).ToList();
        }

        /// <summary>Acrescenta a partida e grava. Mantém só as últimas LimiteDeRegistros.</summary>
        public static void Adicionar(List<RegistroPartida> todos, RegistroPartida novo)
        {
            todos.Add(novo);

            if (todos.Count > LimiteDeRegistros)
                todos.RemoveRange(0, todos.Count - LimiteDeRegistros);

            try
            {
                Directory.CreateDirectory(Configuracoes.PastaDeDados);
                File.WriteAllText(Caminho, new JavaScriptSerializer().Serialize(todos));
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                // Sem permissão para gravar: a tela de resultados continua funcionando.
            }
        }
    }

    /// <summary>Tudo que a tela de estatísticas mostra sobre a partida que acabou de terminar.</summary>
    internal sealed class RelatorioPartida
    {
        public const double PercentualParaPassar = 70.0;

        public string Musica;
        public int Dificuldade;
        public string NomeDificuldade;

        public int Pontuacao, Perfect, Great, Good, Miss, MaiorCombo, TotalNotas, Acertos;
        public double Percentual;
        public bool Aprovado;
        public RankCombo.Rank Rank;

        /// <summary>Partida gerada pelos botões de debug: aparece na tela, mas não entra no histórico.</summary>
        public bool Simulado;

        // Comparação com as partidas anteriores da mesma fase.
        public int? RecordeAnterior;
        public bool NovoRecorde;
        public int Tentativas;
        public double MediaPercentual;
        public List<int> UltimasPontuacoes = new List<int>(); // mais antiga -> atual (a atual por último)

        public static string NomeDaDificuldade(int dificuldade)
        {
            switch (dificuldade)
            {
                case 0: return "Fácil";
                case 1: return "Médio";
                case 2: return "Difícil";
                default: return "?";
            }
        }

        public static RelatorioPartida Criar(ResultadoPartida resultado, int dificuldade,
                                             IList<RegistroPartida> historicoDaFase)
        {
            var r = new RelatorioPartida();
            r.Musica = resultado.NomeMusica;
            r.Dificuldade = dificuldade;
            r.NomeDificuldade = NomeDaDificuldade(dificuldade);
            r.Simulado = resultado.Simulado;

            r.Pontuacao = resultado.Pontuacao;
            r.Perfect = resultado.Perfect;
            r.Great = resultado.Great;
            r.Good = resultado.Good;
            r.Miss = resultado.Miss;
            r.MaiorCombo = resultado.MaiorCombo;
            r.TotalNotas = resultado.tamanhoDaMusica;
            r.Acertos = Math.Min(resultado.NotasAcertadasResultadoPartida, r.TotalNotas);
            r.Percentual = r.TotalNotas > 0 ? Math.Min(100.0, 100.0 * r.Acertos / r.TotalNotas) : 0.0;
            r.Aprovado = r.Percentual >= PercentualParaPassar;
            r.Rank = RankCombo.Obter(r.MaiorCombo);

            // Comparação com o passado (partidas simuladas não contam).
            if (historicoDaFase.Count > 0)
                r.RecordeAnterior = historicoDaFase.Max(h => h.Pontuacao);

            // Só é "novo recorde" se já havia um recorde para superar (a primeira vez é só a primeira).
            r.NovoRecorde = !r.Simulado && r.RecordeAnterior.HasValue &&
                            r.Pontuacao > r.RecordeAnterior.Value;

            double somaPercentuais = historicoDaFase.Sum(h => h.Percentual);
            r.Tentativas = historicoDaFase.Count;
            if (!r.Simulado)
            {
                somaPercentuais += r.Percentual;
                r.Tentativas++;
            }
            r.MediaPercentual = r.Tentativas > 0 ? somaPercentuais / r.Tentativas : 0.0;

            int quantasAnteriores = r.Simulado ? 8 : 7;
            r.UltimasPontuacoes = historicoDaFase
                .Skip(Math.Max(0, historicoDaFase.Count - quantasAnteriores))
                .Select(h => h.Pontuacao).ToList();
            if (!r.Simulado)
                r.UltimasPontuacoes.Add(r.Pontuacao);

            return r;
        }

        public RegistroPartida ParaRegistro()
        {
            return new RegistroPartida
            {
                Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                Musica = Musica,
                Dificuldade = Dificuldade,
                Pontuacao = Pontuacao,
                Perfect = Perfect,
                Great = Great,
                Good = Good,
                Miss = Miss,
                MaiorCombo = MaiorCombo,
                TotalNotas = TotalNotas,
                Percentual = Math.Round(Percentual, 2),
                Aprovado = Aprovado
            };
        }
    }
}
