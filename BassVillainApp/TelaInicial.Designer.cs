namespace BassVillainApp
{
    partial class TelaInicial
    {

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.btn_Iniciar = new BassVillainApp.BotaoTema();
            this.btn_config = new BassVillainApp.BotaoTema();
            this.SuspendLayout();
            // 
            // btn_Iniciar
            // 
            this.btn_Iniciar.BackColor = System.Drawing.Color.Transparent;
            this.btn_Iniciar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Iniciar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Iniciar.Font = new System.Drawing.Font("Freestyle Script", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Iniciar.Location = new System.Drawing.Point(210, 190);
            this.btn_Iniciar.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Iniciar.Name = "btn_Iniciar";
            this.btn_Iniciar.Size = new System.Drawing.Size(380, 84);
            this.btn_Iniciar.TabIndex = 0;
            this.btn_Iniciar.Text = "INICIAR";
            this.btn_Iniciar.UseVisualStyleBackColor = false;
            // 
            // btn_config
            // 
            this.btn_config.BackColor = System.Drawing.Color.Transparent;
            this.btn_config.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_config.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_config.Font = new System.Drawing.Font("Microsoft Uighur", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_config.Location = new System.Drawing.Point(210, 296);
            this.btn_config.Margin = new System.Windows.Forms.Padding(0);
            this.btn_config.Name = "btn_config";
            this.btn_config.Size = new System.Drawing.Size(380, 72);
            this.btn_config.TabIndex = 1;
            this.btn_config.Text = "CONFIGURAÇÕES";
            this.btn_config.UseVisualStyleBackColor = false;
            // 
            // TelaInicial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_config);
            this.Controls.Add(this.btn_Iniciar);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "TelaInicial";
            this.Text = "Bass Villain";
            this.ResumeLayout(false);

        }

        #endregion

        private BassVillainApp.BotaoTema btn_Iniciar;
        private BassVillainApp.BotaoTema btn_config;
    }
}
