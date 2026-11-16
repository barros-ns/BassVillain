using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using MediaPlayer = System.Windows.Media.MediaPlayer;
using BassVillainApp.Properties;
using System.Web.Script.Serialization;
using System.Runtime.InteropServices;


namespace BassVillainApp
{
    public partial class TelaJogo : FormularioBase
    {

        #region Variaveis do Jogo
        private readonly RelogioMusica relogio = new RelogioMusica();

        private readonly Nota notaTeste = new Nota(1, 3000);

        private const int TempoDeQuedaMs = 1500;

        private const int ToleranciaAcertoMs = 150;

        ResultadoPartida resultadoPartida = new ResultadoPartida();

        private readonly MediaPlayer playerMusica = new MediaPlayer();

        private bool partidaEmAndamento;

        private List<Nota> notas = new List<Nota>();

        private String caminho;

        private int comboAtual = 0;

        // Teclas das pistas, lidas das configurações ao abrir a tela.
        private Keys[] teclas;

        // Combo animado (C, B, A, A+, S, SS, SS+).
        private readonly AnimacaoRank animacaoRank = new AnimacaoRank();

        // Tema da música que está tocando (cores e imagens opcionais).
        private TemaVisual tema;

        protected override TemaVisual TemaDaTela
        {
            get { return tema ?? (tema = TemaVisual.ParaMusica(Settings.Default.MusicaSelecionada)); }
        }

        // Teclas já pressionadas (evita que segurar a tecla conte como várias batidas).
        private readonly HashSet<Keys> teclasSegurando = new HashSet<Keys>();

        #endregion

        #region Inicialização

        public TelaJogo()
        {
            InitializeComponent();
            DeclararComponentesJogo();
            DeclararComponentesMusica();
        }

        private void DeclararComponentesJogo() 
        {
            KeyPreview = true;

            teclas = Configuracoes.Teclas;
            pnl_Jogo.BackColor = TemaDaTela.CorFundo;
            relogio.OffsetMs = Configuracoes.OffsetMs;

            pnl_Jogo.Paint += pnl_Jogo_Paint;

            timerJogo.Interval = 16;
            timerJogo.Tick += timerJogo_Tick;

            Shown += TelaJogo_Shown;
            FormClosed += TelaJogo_FormClosed;

            KeyDown += TelaJogo_KeyDown;
            KeyUp += TelaJogo_KeyUp;
            Deactivate += delegate { teclasSegurando.Clear(); };

            btn_skipPass.Click += btn_skipPass_Click;
            btn_skipPass.Visible = false; //Criar uma variavel global no settings para mexer na visibilidade desse botão
            btn_skipFail.Click += btn_skipFail_Click;
            btn_skipFail.Visible = false; //Criar uma variavel global no settings para mexer na visibilidade desse botão
        }

        private void DeclararComponentesMusica() 
        {
            playerMusica.Volume = Configuracoes.Volume / 100.0;

            playerMusica.MediaOpened += PlayerMusica_MediaOpened;
            playerMusica.MediaFailed += PlayerMusica_MediaFailed;
            playerMusica.MediaEnded += PlayerMusica_MediaEnded;

            
        }
        #endregion

        #region Musica
        private void PlayerMusica_MediaOpened(object sender, EventArgs e)
        {
            playerMusica.Position = TimeSpan.Zero;

            playerMusica.Play();
            relogio.Restart();

            partidaEmAndamento = true;
            timerJogo.Start();
        }

        private void PlayerMusica_MediaFailed(object sender, System.Windows.Media.ExceptionEventArgs e)
        {
            partidaEmAndamento = false;

            timerJogo.Stop();
            relogio.Stop();
            playerMusica.Close();

            MessageBox.Show(
                this,
                "Não foi possível reproduzir a música:\n"
                + e.ErrorException.Message,
                "Erro de áudio",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        private void PlayerMusica_MediaEnded(object sender, EventArgs e)
        {
            partidaEmAndamento = false;

            timerJogo.Stop();
            relogio.Stop();
            pnl_Jogo.Invalidate();

            mostrarTelaResultados();
        }

        #endregion

        #region GetMusica Pela dificuldade
        private string getMusicaByDificuldade(int dificuldade)
        {
            int musica = Settings.Default.MusicaSelecionada;
            switch (musica)
            {
                case 0:
                    resultadoPartida.NomeMusica = "Sultans of Swing";
                    return getSultansByDificuldade(dificuldade);
                case 1:
                    resultadoPartida.NomeMusica = "Dark Necessities";
                    return getDarkNecessitiesByDificuldade(dificuldade);
                case 2:
                    resultadoPartida.NomeMusica = "Seven Nation Army";
                    return getSevenNationArmyByDificuldade(dificuldade);
                default:
                    throw new ArgumentOutOfRangeException(nameof(musica), "Dificuldade inválida.");
            }
        }

        private String getSultansByDificuldade(int dificuldade)
        {
                       switch (dificuldade)
            {
                case 0:
                    return "SultansOfSwing.Facil.json";
                case 1:
                    return "SultansOfSwing.Medio.json";
                case 2:
                    return "SultansOfSwing.Dificil.json";
                default:
                    throw new ArgumentOutOfRangeException(nameof(dificuldade), "Dificuldade inválida.");
            }
        }

        private String getDarkNecessitiesByDificuldade(int dificuldade)
        {
            switch (dificuldade)
            {
                case 0:
                    return "DarkNecessities.Facil.json";
                case 1:
                    return "DarkNecessities.Medio.json";
                case 2:
                    return "DarkNecessities.Dificil.json";
                default:
                    throw new ArgumentOutOfRangeException(nameof(dificuldade), "Dificuldade inválida.");
            }
        }

        private String getSevenNationArmyByDificuldade(int dificuldade)
        {
            switch (dificuldade)
            {
                case 0:
                    return "SevenNationArmy.Facil.json";
                case 1:
                    return "SevenNationArmy.Medio.json";
                case 2:
                    return "SevenNationArmy.Dificil.json";
                default:
                    throw new ArgumentOutOfRangeException(nameof(dificuldade), "Dificuldade inválida.");
            }
        }

        #endregion

        #region Eventos do Jogo
        private void TelaJogo_Shown(object sender, EventArgs e)
        {
            caminho = CatalogoMusicas.GetMusica();
            int dificuldade = Settings.Default.DificuldadeSelecionada;
            string nomeDoMapa = getMusicaByDificuldade(dificuldade);


            if (!File.Exists(caminho))
            {
                MessageBox.Show(
                    this,
                    "Arquivo de música não encontrado:\n" + caminho
                );

                return;
            }

            try
            {
                notas = CarregarMapa(nomeDoMapa);
                resultadoPartida.tamanhoDaMusica = notas.Count;
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is ArgumentException ||
                                       ex is InvalidOperationException)
            {
                MessageBox.Show(this, "Erro ao carregar " + nomeDoMapa + "\n" + ex.Message);
                return;
            }


            playerMusica.Open(new Uri(caminho, UriKind.Absolute));
        }

        private void timerJogo_Tick(object sender, EventArgs e)
        {
            if (!partidaEmAndamento)
                return;
            // Ajusta o relógio à posição real da música antes de usar o tempo.
            relogio.Corrigir(playerMusica.Position.TotalMilliseconds);

            long tempoAtualMs = relogio.ElapsedMilliseconds;

            foreach (Nota nota in notas)
            {
                if (!nota.Acertada &&
                    !nota.Perdida &&
                    tempoAtualMs > nota.TempoAcertoMs + ToleranciaAcertoMs)
                {
                    nota.Perdida = true;
                    resultadoPartida.Miss++;
                    comboAtual = 0;
                }
            }
            atualizarPlacar();
            pnl_Jogo.Invalidate();
        }

        private void TelaJogo_FormClosed(object sender,FormClosedEventArgs e)
        {
            timerJogo.Stop();
            timerJogo.Dispose();
            relogio.Stop();
        }

        private void mostrarTelaResultados()
        {
            TelaConclusao telaConclusao = new TelaConclusao(resultadoPartida);
            telaConclusao.Show();
            this.Hide();
        }
        #endregion

        #region Desenho do Jogo
        private void pnl_Jogo_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            float inicioX = pnl_Jogo.Width * 0.10f;
            float inicioY = 20f;

            float largura = pnl_Jogo.Width * 0.80f;
            float altura = pnl_Jogo.Height -40f;

            float larguraPista = largura / 4f;
            float yAlvo = inicioY + altura - 80f;

            // Fundo do tema (se houver imagem).
            if (tema.ImagemFundo != null)
                g.DrawImage(tema.ImagemFundo, pnl_Jogo.ClientRectangle);

            using (Pen contorno = new Pen(tema.CorContorno, 2))
            using (Pen linhaAlvo = new Pen(tema.CorLinhaAlvo, 10))
            {
                // Contorno do cenário.
                g.DrawRectangle(
                    contorno, inicioX, inicioY, largura, altura
                );

                // Três divisórias formam quatro pistas.
                for (int i = 1; i < 4; i++)
                {
                    float x = inicioX + i * larguraPista;

                    g.DrawLine(
                        contorno,
                        x, inicioY,
                        x, inicioY + altura
                    );
                }
                if (tema.ImagemAlvo != null)
                    g.DrawImage(tema.ImagemAlvo, inicioX, yAlvo - 10f, largura, 20f);
                else
                    g.DrawLine(
                        linhaAlvo,
                        inicioX, yAlvo,
                        inicioX + largura, yAlvo
                    );
            }

            long tempoAtualMs = relogio.ElapsedMilliseconds;

            DesenharTempoRestante(e.Graphics);

            foreach (Nota nota in notas)
            {
                if(nota.Acertada || nota.Perdida)
                    continue;

                float progresso = 1f - (nota.TempoAcertoMs - tempoAtualMs) / (float)TempoDeQuedaMs;

                // Desenha apenas durante o percurso até a linha.
                if (progresso < 0f)
                    continue;
                
                    float distanciaQueda = yAlvo - inicioY;

                    float yNota = inicioY + progresso * distanciaQueda;
                    float xNota = inicioX + nota.Pista * larguraPista;

                    const float alturaNota = 28f;

                    var areaNota = new RectangleF(
                        xNota + 10f,
                        yNota - alturaNota / 2f,
                        larguraPista - 20f,
                        alturaNota);

                    if (tema.ImagemNota != null)
                    {
                        g.DrawImage(tema.ImagemNota, areaNota);
                    }
                    else
                    {
                        using (Brush pincel = new SolidBrush(tema.CorNota(nota.Pista)))
                            g.FillRectangle(pincel, areaNota);
                    }
                

            }

            DesenharTeclasDasPistas(e.Graphics, inicioX, larguraPista, yAlvo);

            // Rank do combo na margem esquerda, ao lado das pistas.
            animacaoRank.Desenhar(g, new RectangleF(0f, inicioY + altura * 0.25f, inicioX, 110f), tema);
        }

        private void DesenharTempoRestante(Graphics g)
        {
            if (!playerMusica.NaturalDuration.HasTimeSpan)
                return;

            double total = playerMusica.NaturalDuration.TimeSpan.TotalSeconds;
            if (total <= 0)
                return;

            double restante = Math.Max(
                0, Math.Min(total, total - playerMusica.Position.TotalSeconds));

            const float largura = 10f;
            const float topo = 20f;

            float altura = pnl_Jogo.ClientSize.Height - 2 * topo;
            if (altura <= 0)
                return;

            float x = pnl_Jogo.ClientSize.Width * 0.95f - largura / 2f;
            float preenchimento = altura * (float)(restante / total);

            g.FillRectangle(Brushes.DimGray, x, topo, largura, altura);
            g.FillRectangle(Brushes.DeepSkyBlue,
                x, topo + altura - preenchimento, largura, preenchimento);
        }

        private void DesenharTeclasDasPistas(
        Graphics g, float inicioX, float larguraPista, float yAlvo)
        {
            using (var fonte = new Font(Font.FontFamily, 16f, FontStyle.Bold))
            using (var formato = new StringFormat())
            {
                formato.Alignment = StringAlignment.Center;
                formato.LineAlignment = StringAlignment.Center;

                for (int pista = 0; pista < teclas.Length; pista++)
                {
                    float x = inicioX + pista * larguraPista;
                    var area = new RectangleF(x, yAlvo + 12f, larguraPista, 32f);

                    using (Brush pincelTexto = new SolidBrush(tema.CorTexto))
                        g.DrawString(Configuracoes.NomeDaTecla(teclas[pista]), fonte, pincelTexto, area, formato);
                }
            }
        }
        #endregion

        #region Comandos do Jogo
        private void TelaJogo_KeyUp(object sender, KeyEventArgs e)
        {
            teclasSegurando.Remove(e.KeyCode);
        }

        private void TelaJogo_KeyDown(object sender, KeyEventArgs e)
        {
            // Segurar a tecla gera KeyDown repetido; só a primeira batida vale.
            if (e.KeyCode != Keys.Escape && !teclasSegurando.Add(e.KeyCode))
                return;

            if (e.KeyCode == Keys.Escape)
            {
                pausarJogo();
                return;
            }

            // A posição da tecla na lista é o número da pista.
            int pista = Array.IndexOf(teclas, e.KeyCode);
            if (pista >= 0)
                VerificarAcerto(pista);
        }
        #endregion

        #region Verificador de Acertos
        private void VerificarAcerto(int pista)
        {
            long tempoAtualMs = relogio.ElapsedMilliseconds;

            Nota notaMaisProxima = null;
            long menorDiferenca = long.MaxValue;
            int pontos = 0;

            foreach (Nota nota in notas)
            {
                // Considera apenas notas pendentes da pista pressionada.
                if (nota.Pista != pista || nota.Acertada || nota.Perdida)
                    continue;

                long diferenca = Math.Abs(tempoAtualMs - nota.TempoAcertoMs);

                if (diferenca < menorDiferenca)
                {
                    menorDiferenca = diferenca;
                    notaMaisProxima = nota;
                }
            }

            if (notaMaisProxima != null &&
                menorDiferenca <= ToleranciaAcertoMs)
            {
                comboAtual ++;
                animacaoRank.Pulsar();

                if (menorDiferenca <= 50)
                {
                    resultadoPartida.Perfect++;
                    pontos = 100;
                }
                    
                else if (menorDiferenca <= 100)
                {
                    resultadoPartida.Great++;
                    pontos = 70;
                }
                else
                {
                    resultadoPartida.Good++;
                    pontos = 40;
                }


                notaMaisProxima.Acertada = true;

                atualizarPlacar();
                resultadoPartida.MaiorCombo = Math.Max(resultadoPartida.MaiorCombo, comboAtual);
                resultadoPartida.Pontuacao += pontos;
                pnl_Jogo.Invalidate();
            }
        }
        #endregion

        #region Placar

        private void atualizarPlacar()
        {
            lbl_Acertos.Text = $"Acertadas: {resultadoPartida.NotasAcertadasResultadoPartida}";
            lbl_Erros.Text = $"Erradas: {resultadoPartida.Miss}";
            lbl_ComboAtual.Text = $"Combo Atual: {comboAtual}";
            lbl_ComboMax.Text = $"Combo Máximo: {resultadoPartida.MaiorCombo}";
            lbl_Pontuacao.Text = $"Pontuação: {resultadoPartida.Pontuacao}";
            animacaoRank.Atualizar(comboAtual);
        }
        #endregion

        #region Converter Objetos Json

        public sealed class NotaDoMapa
        {
            public int? pista { get; set; }
            public int? tempoAcertoMs { get; set; }
        }

        private static List<Nota> CarregarMapa(string nomeDoArquivo)
        {
            string caminho = Path.Combine(
                Application.StartupPath, "Mapas", nomeDoArquivo);

            string json = File.ReadAllText(caminho);
            var entradas = new JavaScriptSerializer().Deserialize<List<NotaDoMapa>>(json);

            if (entradas == null || entradas.Count == 0)
                throw new InvalidDataException("O mapa está vazio.");

            var mapa = new List<Nota>();

            foreach (NotaDoMapa entrada in entradas)
            {
                if (entrada == null ||
                    !entrada.pista.HasValue || entrada.pista.Value < 0 || entrada.pista.Value > 3 ||
                    !entrada.tempoAcertoMs.HasValue || entrada.tempoAcertoMs.Value < 0)
                {
                    throw new InvalidDataException("Há uma nota com pista ou tempo inválido.");
                }

                mapa.Add(new Nota(entrada.pista.Value, entrada.tempoAcertoMs.Value));
            }

            return mapa;
        }
        #endregion

        #region Pausar
        private void pausarJogo()
        {
            if (!partidaEmAndamento)
                return;
            partidaEmAndamento = false;
            timerJogo.Stop();
            relogio.Stop();
            playerMusica.Pause();
            
            DialogResult msgb = MessageBox.Show(
                this,
                "Jogo pausado. Deseja Continuar?",
                "Pausa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information
            );

            if (msgb == DialogResult.No)
            {
                
                TelaInicial telaInicial = new TelaInicial(); //melhorar isso aqui mas tem que alterar mta coisa
                telaInicial.Show();
                this.Hide();
                playerMusica.Stop();
                return; 
            }
            partidaEmAndamento = true;
            relogio.Start();
            playerMusica.Play();
            timerJogo.Start();
        }
        #endregion

        #region Debug
        private void btn_skipPass_Click(object sender, EventArgs e)
        {
            partidaEmAndamento = false;

            timerJogo.Stop();
            relogio.Stop();
            playerMusica.Stop();
            pnl_Jogo.Invalidate();


            resultadoPartida.Perfect = resultadoPartida.tamanhoDaMusica;

            mostrarTelaResultados();
        }

        private void btn_skipFail_Click(object sender, EventArgs e)
        {
            partidaEmAndamento = false;

            timerJogo.Stop();
            relogio.Stop();
            playerMusica.Stop();
            pnl_Jogo.Invalidate();

            resultadoPartida.Miss = resultadoPartida.tamanhoDaMusica;

            mostrarTelaResultados();
        }
        #endregion
    }
}
