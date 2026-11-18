using System;
using System.Windows.Forms;

namespace BassVillainApp
{
    public partial class frm_Configuracoes : FormularioBase
    {
        #region Variaveis Globais
        private readonly TelaInicial telaInicial;
        private Button[] botoesTecla;

        // Pista que está esperando uma nova tecla (-1 = nenhuma).
        private int pistaAguardando = -1;
        #endregion

        #region Incialização
        public frm_Configuracoes(TelaInicial telaInicial)
        {
            InitializeComponent();
            this.telaInicial = telaInicial;

            botoesTecla = new[] { btn_Tecla0, btn_Tecla1, btn_Tecla2, btn_Tecla3 };
            configEstiloDosBotoes();

            carregarValoresNaTela();
            configEventos();
        }

        // Botões no mesmo estilo do menu (a cor de cada tecla é a da pista correspondente).
        private void configEstiloDosBotoes()
        {
            for (int i = 0; i < botoesTecla.Length; i++)
            {
                btn_TeclaN(i).Compacto = true;
                btn_TeclaN(i).TamanhoTexto = 24f;
                btn_TeclaN(i).CorDestaque = TemaVisual.Menu.CorNota(i);
            }

            btn_voltar.TamanhoTexto = 22f;
            btn_voltar.Text = "VOLTAR";
            btn_voltar.CorDestaque = TemaVisual.Menu.CorNota(2);

            btn_Restaurar.TamanhoTexto = 20f;
            btn_Restaurar.Text = "RESTAURAR PADRÃO";
            btn_Restaurar.CorDestaque = TemaVisual.Menu.CorNota(3);

            CancelButton = btn_voltar; // Esc volta (a não ser que uma tecla esteja sendo escolhida)
        }

        private BotaoTema btn_TeclaN(int i)
        {
            return (BotaoTema)botoesTecla[i];
        }

        private void carregarValoresNaTela()
        {
            trk_Volume.Value = Configuracoes.Volume;
            trk_Offset.Value = Configuracoes.OffsetMs;
            chk_Debug.Checked = Configuracoes.ModoDebug;
            atualizarTextos();
            atualizarBotoesTecla();
        }

        private void configEventos()
        {
            btn_voltar.Click += btn_voltar_Click;
            btn_Restaurar.Click += btn_Restaurar_Click;
            chk_Debug.CheckedChanged += delegate { Configuracoes.ModoDebug = chk_Debug.Checked; };

            trk_Volume.ValueChanged += trk_Volume_ValueChanged;
            trk_Offset.ValueChanged += trk_Offset_ValueChanged;

            foreach (Button botao in botoesTecla)
                botao.Click += btn_Tecla_Click;

            // Quem sai da tela (Voltar ou fechando o jogo) tem as escolhas gravadas em disco.
            FormClosed += delegate { Configuracoes.Salvar(); };
        }
        #endregion

        #region Áudio e sincronia
        private void trk_Volume_ValueChanged(object sender, EventArgs e)
        {
            Configuracoes.Volume = trk_Volume.Value;
            atualizarTextos();
        }

        private void trk_Offset_ValueChanged(object sender, EventArgs e)
        {
            Configuracoes.OffsetMs = trk_Offset.Value;
            atualizarTextos();
        }

        private void atualizarTextos()
        {
            lbl_volumeValor.Text = trk_Volume.Value + "%";
            lbl_offsetValor.Text = (trk_Offset.Value > 0 ? "+" : "") + trk_Offset.Value + " ms";
        }
        #endregion

        #region Teclas
        private void btn_Tecla_Click(object sender, EventArgs e)
        {
            pistaAguardando = Array.IndexOf(botoesTecla, (Button)sender);
            lbl_avisoTecla.Text = "Pressione a nova tecla da Pista " + (pistaAguardando + 1) + " (Esc cancela)...";
            atualizarBotoesTecla();
        }

        // Enquanto uma pista espera tecla, tudo que for digitado vem para cá antes dos botões
        // (senão Espaço/Enter/setas seriam "comidos" pelo botão que está com foco).
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (pistaAguardando >= 0)
            {
                receberTecla(keyData & Keys.KeyCode);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void receberTecla(Keys tecla)
        {
            // Teclas modificadoras sozinhas não servem: continua esperando.
            if (tecla == Keys.ShiftKey || tecla == Keys.ControlKey || tecla == Keys.Menu ||
                tecla == Keys.LWin || tecla == Keys.RWin)
                return;

            int pista = pistaAguardando;
            string aviso = "Clique em uma tecla e depois pressione a nova (Esc cancela).";

            if (tecla != Keys.Escape) // Esc é reservado para pausar o jogo
            {
                if (Configuracoes.TeclaEmUso(tecla, pista))
                    aviso = "A tecla " + Configuracoes.NomeDaTecla(tecla) + " já está em uso em outra pista.";
                else
                    Configuracoes.DefinirTecla(pista, tecla);
            }

            pistaAguardando = -1;
            lbl_avisoTecla.Text = aviso;
            atualizarBotoesTecla();
        }

        private void atualizarBotoesTecla()
        {
            Keys[] teclas = Configuracoes.Teclas;

            for (int i = 0; i < botoesTecla.Length; i++)
            {
                botoesTecla[i].Text = (i == pistaAguardando)
                    ? "..."
                    : Configuracoes.NomeDaTecla(teclas[i]);
            }
        }
        #endregion

        #region Botões
        private void btn_Restaurar_Click(object sender, EventArgs e)
        {
            Configuracoes.RestaurarPadrao();
            pistaAguardando = -1;
            lbl_avisoTecla.Text = "Valores padrão restaurados.";
            carregarValoresNaTela();
        }

        private void btn_voltar_Click(object sender, EventArgs e)
        {
            telaInicial.Show();
            FecharParaVoltar();
        }
        #endregion
    }
}
