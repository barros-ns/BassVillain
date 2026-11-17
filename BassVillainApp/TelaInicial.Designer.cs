namespace BassVillainApp
{
    partial class TelaInicial
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
            this.btn_Iniciar = new BassVillainApp.BotaoTema();
            this.btn_config = new BassVillainApp.BotaoTema();
            this.SuspendLayout();
            // 
            // btn_Iniciar
            // 
            this.btn_Iniciar.Location = new System.Drawing.Point(210, 190);
            this.btn_Iniciar.Name = "btn_Iniciar";
            this.btn_Iniciar.Size = new System.Drawing.Size(380, 84);
            this.btn_Iniciar.TabIndex = 0;
            this.btn_Iniciar.Text = "INICIAR";
            this.btn_Iniciar.UseVisualStyleBackColor = true;
            // 
            // btn_config
            // 
            this.btn_config.Location = new System.Drawing.Point(210, 296);
            this.btn_config.Name = "btn_config";
            this.btn_config.Size = new System.Drawing.Size(380, 72);
            this.btn_config.TabIndex = 1;
            this.btn_config.Text = "CONFIGURAÇÕES";
            this.btn_config.UseVisualStyleBackColor = true;
            // 
            // TelaInicial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_config);
            this.Controls.Add(this.btn_Iniciar);
            this.Name = "TelaInicial";
            this.Text = "Bass Villain";
            this.ResumeLayout(false);

        }

        #endregion

        private BassVillainApp.BotaoTema btn_Iniciar;
        private BassVillainApp.BotaoTema btn_config;
    }
}
