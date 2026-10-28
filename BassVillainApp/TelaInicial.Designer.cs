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
            this.lbl_Titulo = new System.Windows.Forms.Label();
            this.btn_Iniciar = new System.Windows.Forms.Button();
            this.btn_estatisticas = new System.Windows.Forms.Button();
            this.btn_config = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_Titulo
            // 
            this.lbl_Titulo.AutoSize = true;
            this.lbl_Titulo.Location = new System.Drawing.Point(369, 138);
            this.lbl_Titulo.Name = "lbl_Titulo";
            this.lbl_Titulo.Size = new System.Drawing.Size(60, 13);
            this.lbl_Titulo.TabIndex = 0;
            this.lbl_Titulo.Text = "Bass Villain";
            // 
            // btn_Iniciar
            // 
            this.btn_Iniciar.Location = new System.Drawing.Point(363, 190);
            this.btn_Iniciar.Name = "btn_Iniciar";
            this.btn_Iniciar.Size = new System.Drawing.Size(75, 23);
            this.btn_Iniciar.TabIndex = 1;
            this.btn_Iniciar.Text = "Iniciar";
            this.btn_Iniciar.UseVisualStyleBackColor = true;
            // 
            // btn_estatisticas
            // 
            this.btn_estatisticas.Location = new System.Drawing.Point(363, 245);
            this.btn_estatisticas.Name = "btn_estatisticas";
            this.btn_estatisticas.Size = new System.Drawing.Size(75, 23);
            this.btn_estatisticas.TabIndex = 2;
            this.btn_estatisticas.Text = "Estatisticas";
            this.btn_estatisticas.UseVisualStyleBackColor = true;
            // 
            // btn_config
            // 
            this.btn_config.Location = new System.Drawing.Point(731, 12);
            this.btn_config.Name = "btn_config";
            this.btn_config.Size = new System.Drawing.Size(57, 23);
            this.btn_config.TabIndex = 3;
            this.btn_config.Text = "Iniciar";
            this.btn_config.UseVisualStyleBackColor = true;
            // 
            // TelaInicial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_config);
            this.Controls.Add(this.btn_estatisticas);
            this.Controls.Add(this.btn_Iniciar);
            this.Controls.Add(this.lbl_Titulo);
            this.Name = "TelaInicial";
            this.Text = "frm_TelaInicial";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Titulo;
        private System.Windows.Forms.Button btn_Iniciar;
        private System.Windows.Forms.Button btn_estatisticas;
        private System.Windows.Forms.Button btn_config;
    }
}

