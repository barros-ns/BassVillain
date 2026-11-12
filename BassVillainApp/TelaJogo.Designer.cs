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
            this.lbl_Pontuacao = new System.Windows.Forms.Label();
            this.lbl_ComboMax = new System.Windows.Forms.Label();
            this.lbl_ComboAtual = new System.Windows.Forms.Label();
            this.lbl_Erros = new System.Windows.Forms.Label();
            this.lbl_Acertos = new System.Windows.Forms.Label();
            this.timerJogo = new System.Windows.Forms.Timer(this.components);
            this.pnl_Jogo = new BassVillainApp.PainelJogo();
            this.btn_skip = new System.Windows.Forms.Button();
            this.pnl_Pontuacao.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_Pontuacao
            // 
            this.pnl_Pontuacao.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnl_Pontuacao.Controls.Add(this.btn_skip);
            this.pnl_Pontuacao.Controls.Add(this.lbl_Pontuacao);
            this.pnl_Pontuacao.Controls.Add(this.lbl_ComboMax);
            this.pnl_Pontuacao.Controls.Add(this.lbl_ComboAtual);
            this.pnl_Pontuacao.Controls.Add(this.lbl_Erros);
            this.pnl_Pontuacao.Controls.Add(this.lbl_Acertos);
            this.pnl_Pontuacao.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_Pontuacao.Location = new System.Drawing.Point(0, 0);
            this.pnl_Pontuacao.Name = "pnl_Pontuacao";
            this.pnl_Pontuacao.Size = new System.Drawing.Size(800, 100);
            this.pnl_Pontuacao.TabIndex = 0;
            // 
            // lbl_Pontuacao
            // 
            this.lbl_Pontuacao.AutoSize = true;
            this.lbl_Pontuacao.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Pontuacao.Location = new System.Drawing.Point(537, 9);
            this.lbl_Pontuacao.Name = "lbl_Pontuacao";
            this.lbl_Pontuacao.Size = new System.Drawing.Size(24, 25);
            this.lbl_Pontuacao.TabIndex = 8;
            this.lbl_Pontuacao.Text = "0";
            // 
            // lbl_ComboMax
            // 
            this.lbl_ComboMax.AutoSize = true;
            this.lbl_ComboMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ComboMax.Location = new System.Drawing.Point(319, 52);
            this.lbl_ComboMax.Name = "lbl_ComboMax";
            this.lbl_ComboMax.Size = new System.Drawing.Size(24, 25);
            this.lbl_ComboMax.TabIndex = 7;
            this.lbl_ComboMax.Text = "0";
            // 
            // lbl_ComboAtual
            // 
            this.lbl_ComboAtual.AutoSize = true;
            this.lbl_ComboAtual.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ComboAtual.Location = new System.Drawing.Point(319, 9);
            this.lbl_ComboAtual.Name = "lbl_ComboAtual";
            this.lbl_ComboAtual.Size = new System.Drawing.Size(24, 25);
            this.lbl_ComboAtual.TabIndex = 6;
            this.lbl_ComboAtual.Text = "0";
            // 
            // lbl_Erros
            // 
            this.lbl_Erros.AutoSize = true;
            this.lbl_Erros.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Erros.Location = new System.Drawing.Point(58, 52);
            this.lbl_Erros.Name = "lbl_Erros";
            this.lbl_Erros.Size = new System.Drawing.Size(24, 25);
            this.lbl_Erros.TabIndex = 5;
            this.lbl_Erros.Text = "0";
            // 
            // lbl_Acertos
            // 
            this.lbl_Acertos.AutoSize = true;
            this.lbl_Acertos.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Acertos.Location = new System.Drawing.Point(58, 9);
            this.lbl_Acertos.Name = "lbl_Acertos";
            this.lbl_Acertos.Size = new System.Drawing.Size(24, 25);
            this.lbl_Acertos.TabIndex = 4;
            this.lbl_Acertos.Text = "0";
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
            // btn_skip
            // 
            this.btn_skip.Location = new System.Drawing.Point(518, 56);
            this.btn_skip.Name = "btn_skip";
            this.btn_skip.Size = new System.Drawing.Size(75, 23);
            this.btn_skip.TabIndex = 0;
            this.btn_skip.Text = "Skipar";
            this.btn_skip.UseVisualStyleBackColor = true;
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
        private System.Windows.Forms.Label lbl_ComboMax;
        private System.Windows.Forms.Label lbl_ComboAtual;
        private System.Windows.Forms.Label lbl_Erros;
        private System.Windows.Forms.Label lbl_Acertos;
        private System.Windows.Forms.Label lbl_Pontuacao;
        private System.Windows.Forms.Button btn_skip;
    }
}