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
            this.lbl_NomeMusica = new System.Windows.Forms.Label();
            this.btn_telaInicial = new System.Windows.Forms.Button();
            this.btn_estatisticas = new System.Windows.Forms.Button();
            this.lb_resultados = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbl_NomeMusica
            // 
            this.lbl_NomeMusica.AutoSize = true;
            this.lbl_NomeMusica.Location = new System.Drawing.Point(382, 38);
            this.lbl_NomeMusica.Name = "lbl_NomeMusica";
            this.lbl_NomeMusica.Size = new System.Drawing.Size(41, 13);
            this.lbl_NomeMusica.TabIndex = 0;
            this.lbl_NomeMusica.Text = "Musica";
            // 
            // btn_telaInicial
            // 
            this.btn_telaInicial.Location = new System.Drawing.Point(701, 33);
            this.btn_telaInicial.Name = "btn_telaInicial";
            this.btn_telaInicial.Size = new System.Drawing.Size(75, 23);
            this.btn_telaInicial.TabIndex = 1;
            this.btn_telaInicial.Text = "Continuar";
            this.btn_telaInicial.UseVisualStyleBackColor = true;
            // 
            // btn_estatisticas
            // 
            this.btn_estatisticas.Location = new System.Drawing.Point(701, 79);
            this.btn_estatisticas.Name = "btn_estatisticas";
            this.btn_estatisticas.Size = new System.Drawing.Size(75, 23);
            this.btn_estatisticas.TabIndex = 2;
            this.btn_estatisticas.Text = "Estatisticas";
            this.btn_estatisticas.UseVisualStyleBackColor = true;
            // 
            // lb_resultados
            // 
            this.lb_resultados.FormattingEnabled = true;
            this.lb_resultados.Location = new System.Drawing.Point(12, 131);
            this.lb_resultados.Name = "lb_resultados";
            this.lb_resultados.Size = new System.Drawing.Size(764, 303);
            this.lb_resultados.TabIndex = 3;
            // 
            // TelaConclusao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lb_resultados);
            this.Controls.Add(this.btn_estatisticas);
            this.Controls.Add(this.btn_telaInicial);
            this.Controls.Add(this.lbl_NomeMusica);
            this.Name = "TelaConclusao";
            this.Text = "TelaConclusao";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_NomeMusica;
        private System.Windows.Forms.Button btn_telaInicial;
        private System.Windows.Forms.Button btn_estatisticas;
        private System.Windows.Forms.ListBox lb_resultados;
    }
}