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
            this.timerJogo = new System.Windows.Forms.Timer(this.components);
            this.pnl_Jogo = new BassVillainApp.PainelJogo();
            this.lbl_Perfect = new System.Windows.Forms.Label();
            this.lbl_Great = new System.Windows.Forms.Label();
            this.lbl_Good = new System.Windows.Forms.Label();
            this.lbl_Miss = new System.Windows.Forms.Label();
            this.pnl_Pontuacao.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_Pontuacao
            // 
            this.pnl_Pontuacao.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnl_Pontuacao.Controls.Add(this.lbl_Miss);
            this.pnl_Pontuacao.Controls.Add(this.lbl_Good);
            this.pnl_Pontuacao.Controls.Add(this.lbl_Great);
            this.pnl_Pontuacao.Controls.Add(this.lbl_Perfect);
            this.pnl_Pontuacao.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_Pontuacao.Location = new System.Drawing.Point(0, 0);
            this.pnl_Pontuacao.Name = "pnl_Pontuacao";
            this.pnl_Pontuacao.Size = new System.Drawing.Size(800, 100);
            this.pnl_Pontuacao.TabIndex = 0;
            // 
            // timerJogo
            // 
            this.timerJogo.Enabled = true;
            this.timerJogo.Interval = 10;
            // 
            // pnl_Jogo
            // 
            this.pnl_Jogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Jogo.Location = new System.Drawing.Point(0, 100);
            this.pnl_Jogo.Name = "pnl_Jogo";
            this.pnl_Jogo.Size = new System.Drawing.Size(800, 514);
            this.pnl_Jogo.TabIndex = 1;
            // 
            // lbl_Perfect
            // 
            this.lbl_Perfect.AutoSize = true;
            this.lbl_Perfect.Location = new System.Drawing.Point(112, 45);
            this.lbl_Perfect.Name = "lbl_Perfect";
            this.lbl_Perfect.Size = new System.Drawing.Size(13, 13);
            this.lbl_Perfect.TabIndex = 4;
            this.lbl_Perfect.Text = "0";
            // 
            // lbl_Great
            // 
            this.lbl_Great.AutoSize = true;
            this.lbl_Great.Location = new System.Drawing.Point(248, 45);
            this.lbl_Great.Name = "lbl_Great";
            this.lbl_Great.Size = new System.Drawing.Size(13, 13);
            this.lbl_Great.TabIndex = 5;
            this.lbl_Great.Text = "0";
            // 
            // lbl_Good
            // 
            this.lbl_Good.AutoSize = true;
            this.lbl_Good.Location = new System.Drawing.Point(390, 45);
            this.lbl_Good.Name = "lbl_Good";
            this.lbl_Good.Size = new System.Drawing.Size(13, 13);
            this.lbl_Good.TabIndex = 6;
            this.lbl_Good.Text = "0";
            // 
            // lbl_Miss
            // 
            this.lbl_Miss.AutoSize = true;
            this.lbl_Miss.Location = new System.Drawing.Point(517, 45);
            this.lbl_Miss.Name = "lbl_Miss";
            this.lbl_Miss.Size = new System.Drawing.Size(13, 13);
            this.lbl_Miss.TabIndex = 7;
            this.lbl_Miss.Text = "0";
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
            this.pnl_Pontuacao.ResumeLayout(false);
            this.pnl_Pontuacao.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_Pontuacao;
        private System.Windows.Forms.Timer timerJogo;
        private PainelJogo pnl_Jogo;
        private System.Windows.Forms.Label lbl_Miss;
        private System.Windows.Forms.Label lbl_Good;
        private System.Windows.Forms.Label lbl_Great;
        private System.Windows.Forms.Label lbl_Perfect;
    }
}