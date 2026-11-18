namespace BassVillainApp
{
    partial class TelaConclusao
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnl_Estatisticas = new BassVillainApp.PainelJogo();
            this.btn_telaInicial = new BassVillainApp.BotaoTema();
            this.pnl_Estatisticas.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_Estatisticas
            // 
            this.pnl_Estatisticas.Controls.Add(this.btn_telaInicial);
            this.pnl_Estatisticas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Estatisticas.Location = new System.Drawing.Point(0, 0);
            this.pnl_Estatisticas.Name = "pnl_Estatisticas";
            this.pnl_Estatisticas.Size = new System.Drawing.Size(800, 450);
            this.pnl_Estatisticas.TabIndex = 0;
            // 
            // btn_telaInicial
            // 
            this.btn_telaInicial.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_telaInicial.Location = new System.Drawing.Point(250, 376);
            this.btn_telaInicial.Name = "btn_telaInicial";
            this.btn_telaInicial.Size = new System.Drawing.Size(300, 60);
            this.btn_telaInicial.TabIndex = 1;
            this.btn_telaInicial.Text = "CONTINUAR";
            this.btn_telaInicial.UseVisualStyleBackColor = true;
            // 
            // TelaConclusao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnl_Estatisticas);
            this.Name = "TelaConclusao";
            this.Text = "Estatísticas";
            this.pnl_Estatisticas.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BassVillainApp.PainelJogo pnl_Estatisticas;
        private BassVillainApp.BotaoTema btn_telaInicial;
    }
}
