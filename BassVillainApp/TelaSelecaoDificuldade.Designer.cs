namespace BassVillainApp
{
    partial class TelaSelecaoDificuldade
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
            this.lbl_titulo = new System.Windows.Forms.Label();
            this.lbl_nomeDaMusica = new System.Windows.Forms.Label();
            this.btn_facil = new System.Windows.Forms.Button();
            this.btn_medio = new System.Windows.Forms.Button();
            this.btn_dificil = new System.Windows.Forms.Button();
            this.btn_voltar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_titulo
            // 
            this.lbl_titulo.AutoSize = true;
            this.lbl_titulo.Location = new System.Drawing.Point(313, 30);
            this.lbl_titulo.Name = "lbl_titulo";
            this.lbl_titulo.Size = new System.Drawing.Size(151, 13);
            this.lbl_titulo.TabIndex = 0;
            this.lbl_titulo.Text = "SELECIONE A DIFICULDADE";
            // 
            // lbl_nomeDaMusica
            // 
            this.lbl_nomeDaMusica.AutoSize = true;
            this.lbl_nomeDaMusica.Location = new System.Drawing.Point(370, 69);
            this.lbl_nomeDaMusica.Name = "lbl_nomeDaMusica";
            this.lbl_nomeDaMusica.Size = new System.Drawing.Size(40, 13);
            this.lbl_nomeDaMusica.TabIndex = 1;
            this.lbl_nomeDaMusica.Text = "musica";
            // 
            // btn_facil
            // 
            this.btn_facil.Location = new System.Drawing.Point(354, 132);
            this.btn_facil.Name = "btn_facil";
            this.btn_facil.Size = new System.Drawing.Size(75, 23);
            this.btn_facil.TabIndex = 2;
            this.btn_facil.Text = "Fácil";
            this.btn_facil.UseVisualStyleBackColor = true;
            // 
            // btn_medio
            // 
            this.btn_medio.Location = new System.Drawing.Point(354, 192);
            this.btn_medio.Name = "btn_medio";
            this.btn_medio.Size = new System.Drawing.Size(75, 23);
            this.btn_medio.TabIndex = 3;
            this.btn_medio.Text = "Médio";
            this.btn_medio.UseVisualStyleBackColor = true;
            // 
            // btn_dificil
            // 
            this.btn_dificil.Location = new System.Drawing.Point(354, 258);
            this.btn_dificil.Name = "btn_dificil";
            this.btn_dificil.Size = new System.Drawing.Size(75, 23);
            this.btn_dificil.TabIndex = 4;
            this.btn_dificil.Text = "Difícil";
            this.btn_dificil.UseVisualStyleBackColor = true;
            // 
            // btn_voltar
            // 
            this.btn_voltar.Location = new System.Drawing.Point(22, 20);
            this.btn_voltar.Name = "btn_voltar";
            this.btn_voltar.Size = new System.Drawing.Size(75, 23);
            this.btn_voltar.TabIndex = 5;
            this.btn_voltar.Text = "Voltar";
            this.btn_voltar.UseVisualStyleBackColor = true;
            // 
            // TelaSelecaoDificuldade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_voltar);
            this.Controls.Add(this.btn_dificil);
            this.Controls.Add(this.btn_medio);
            this.Controls.Add(this.btn_facil);
            this.Controls.Add(this.lbl_nomeDaMusica);
            this.Controls.Add(this.lbl_titulo);
            this.Name = "TelaSelecaoDificuldade";
            this.Text = "TelaSelecaoDificuldade";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_titulo;
        private System.Windows.Forms.Label lbl_nomeDaMusica;
        private System.Windows.Forms.Button btn_facil;
        private System.Windows.Forms.Button btn_medio;
        private System.Windows.Forms.Button btn_dificil;
        private System.Windows.Forms.Button btn_voltar;
    }
}