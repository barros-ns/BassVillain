namespace BassVillainApp
{
    partial class TelaJogo
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
            this.components = new System.ComponentModel.Container();
            this.pnl_Pontuacao = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnl_Jogo = new BassVillainApp.PainelJogo();
            this.SuspendLayout();
            // 
            // pnl_Pontuacao
            // 
            this.pnl_Pontuacao.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnl_Pontuacao.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_Pontuacao.Location = new System.Drawing.Point(0, 0);
            this.pnl_Pontuacao.Name = "pnl_Pontuacao";
            this.pnl_Pontuacao.Size = new System.Drawing.Size(800, 100);
            this.pnl_Pontuacao.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            // 
            // pnl_Jogo
            // 
            this.pnl_Jogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Jogo.Location = new System.Drawing.Point(0, 100);
            this.pnl_Jogo.Name = "pnl_Jogo";
            this.pnl_Jogo.Size = new System.Drawing.Size(800, 514);
            this.pnl_Jogo.TabIndex = 1;
            // 
            // TelaJogo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 614);
            this.Controls.Add(this.pnl_Jogo);
            this.Controls.Add(this.pnl_Pontuacao);
            this.Name = "TelaJogo";
            this.Text = "TelaJogo";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_Pontuacao;
        private System.Windows.Forms.Timer timer1;
        private PainelJogo pnl_Jogo;
    }
}