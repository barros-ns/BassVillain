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


namespace BassVillainApp
{
    public partial class TelaJogo : FormularioBase
    {

        #region Variaveis do Jogo
        private readonly Stopwatch relogio = new Stopwatch();

        private readonly Nota notaTeste = new Nota(1, 3000);

        private const int TempoDeQuedaMs = 1500;

        private const int ToleranciaAcertoMs = 150;

        ResultadoPartida resultadoPartida = new ResultadoPartida();

        private readonly MediaPlayer playerMusica = new MediaPlayer();

        private bool partidaEmAndamento;

        List<Nota> notas = new List<Nota>();

        String caminho;

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

            pnl_Jogo.Paint += pnl_Jogo_Paint;

            timerJogo.Interval = 16;
            timerJogo.Tick += timerJogo_Tick;

            Shown += TelaJogo_Shown;
            FormClosed += TelaJogo_FormClosed;

            KeyDown += TelaJogo_KeyDown;
        }

        private void DeclararComponentesMusica() 
        {
            playerMusica.Volume = 0.7;

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

            // Depois vamos finalizar os resultados e abrir a próxima tela.
        }


        #endregion

        #region Eventos do Jogo
        private void TelaJogo_Shown(object sender, EventArgs e)
        {
            caminho = CatalogoMusicas.GetMusica();
        


            if (!File.Exists(caminho))
            {
                MessageBox.Show(
                    this,
                    "Arquivo de música não encontrado:\n" + caminho
                );

                return;
            }

            const string nomeDoMapa = "SultansOfSwing.Medio.json";

            try
            {
                notas = CarregarMapa(nomeDoMapa);
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
            long tempoAtualMs = relogio.ElapsedMilliseconds;

            foreach (Nota nota in notas)
            {
                if (!nota.Acertada &&
                    !nota.Perdida &&
                    tempoAtualMs > nota.TempoAcertoMs + ToleranciaAcertoMs)
                {
                    nota.Perdida = true;
                    resultadoPartida.Miss++;
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

            using (Pen contorno = new Pen(Color.Gray, 2))
            using (Pen linhaAlvo = new Pen(Color.Red, 3))
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

                    const float alturaNota = 20f;

                    using (Brush pincel = new SolidBrush(Color.DeepSkyBlue))
                    {
                        g.FillRectangle(
                            pincel,
                            xNota + 10f,
                            yNota - alturaNota / 2f,
                            larguraPista - 20f,
                            alturaNota
                        );
                    }
                

            }


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
        #endregion

        #region Comandos do Jogo
        private void TelaJogo_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.A:
                    VerificarAcerto(0);
                    break;
                case Keys.S:
                    VerificarAcerto(1); 
                    break;
                case Keys.K:
                    VerificarAcerto(2); 
                    break;
                case Keys.L:
                    VerificarAcerto(3); 
                    break;
            }
        }
        #endregion


        #region Verificador de Acertos
        private void VerificarAcerto(int pista)
        {
            long tempoAtualMs = relogio.ElapsedMilliseconds;

            Nota notaMaisProxima = null;
            long menorDiferenca = long.MaxValue;

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
                if (menorDiferenca <= 50)
                    resultadoPartida.Perfect++;
                else if (menorDiferenca <= 100)
                    resultadoPartida.Great++;
                else
                    resultadoPartida.Good++;
                notaMaisProxima.Acertada = true;

                atualizarPlacar();
                pnl_Jogo.Invalidate();
            }
        }
        #endregion

        #region Placar

        private void atualizarPlacar()
        {
            lbl_Perfect.Text = $"Perfect: {resultadoPartida.Perfect}";
            lbl_Great.Text = $"Great: {resultadoPartida.Great}";
            lbl_Good.Text = $"Good: {resultadoPartida.Good}";
            lbl_Miss.Text = $"Miss: {resultadoPartida.Miss}";
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
    }
}
