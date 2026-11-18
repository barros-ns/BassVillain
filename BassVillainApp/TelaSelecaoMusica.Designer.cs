namespace BassVillainApp
{
    partial class frm_TelaSelecaoMusica
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
            this.btn_Msc1 = new BassVillainApp.BotaoTema();
            this.btn_Msc2 = new BassVillainApp.BotaoTema();
            this.btn_Msc3 = new BassVillainApp.BotaoTema();
            this.btn_voltar = new BassVillainApp.BotaoTema();
            this.SuspendLayout();
            // 
            // btn_Msc1
            // 
            this.btn_Msc1.Location = new System.Drawing.Point(166, 162);
            this.btn_Msc1.Name = "btn_Msc1";
            this.btn_Msc1.Size = new System.Drawing.Size(75, 23);
            this.btn_Msc1.TabIndex = 3;
            this.btn_Msc1.Text = "Iniciar";
            this.btn_Msc1.UseVisualStyleBackColor = true;
            // 
            // btn_Msc2
            // 
            this.btn_Msc2.Location = new System.Drawing.Point(343, 162);
            this.btn_Msc2.Name = "btn_Msc2";
            this.btn_Msc2.Size = new System.Drawing.Size(75, 23);
            this.btn_Msc2.TabIndex = 4;
            this.btn_Msc2.Text = "Iniciar";
            this.btn_Msc2.UseVisualStyleBackColor = true;
            // 
            // btn_Msc3
            // 
            this.btn_Msc3.Location = new System.Drawing.Point(516, 162);
            this.btn_Msc3.Name = "btn_Msc3";
            this.btn_Msc3.Size = new System.Drawing.Size(75, 23);
            this.btn_Msc3.TabIndex = 5;
            this.btn_Msc3.Text = "Iniciar";
            this.btn_Msc3.UseVisualStyleBackColor = true;
            // 
            // btn_voltar
            // 
            this.btn_voltar.Location = new System.Drawing.Point(22, 27);
            this.btn_voltar.Name = "btn_voltar";
            this.btn_voltar.Size = new System.Drawing.Size(75, 23);
            this.btn_voltar.TabIndex = 6;
            this.btn_voltar.Text = "Voltar";
            this.btn_voltar.UseVisualStyleBackColor = true;
            // 
            // frm_TelaSelecaoMusica
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_voltar);
            this.Controls.Add(this.btn_Msc3);
            this.Controls.Add(this.btn_Msc2);
            this.Controls.Add(this.btn_Msc1);
            this.Name = "frm_TelaSelecaoMusica";
            this.Text = "SelecaoMusica";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BassVillainApp.BotaoTema btn_Msc1;
        private BassVillainApp.BotaoTema btn_Msc2;
        private BassVillainApp.BotaoTema btn_Msc3;
        private BassVillainApp.BotaoTema btn_voltar;
    }
}