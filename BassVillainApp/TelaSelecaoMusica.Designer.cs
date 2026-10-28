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
            this.lbl_msc1 = new System.Windows.Forms.Label();
            this.lbl_Msc2 = new System.Windows.Forms.Label();
            this.lbl_Msc3 = new System.Windows.Forms.Label();
            this.btn_Msc1 = new System.Windows.Forms.Button();
            this.btn_Msc2 = new System.Windows.Forms.Button();
            this.btn_Msc3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_msc1
            // 
            this.lbl_msc1.AutoSize = true;
            this.lbl_msc1.Location = new System.Drawing.Point(163, 97);
            this.lbl_msc1.Name = "lbl_msc1";
            this.lbl_msc1.Size = new System.Drawing.Size(86, 13);
            this.lbl_msc1.TabIndex = 0;
            this.lbl_msc1.Text = "Sultans of Swing";
            // 
            // lbl_Msc2
            // 
            this.lbl_Msc2.AutoSize = true;
            this.lbl_Msc2.Location = new System.Drawing.Point(331, 97);
            this.lbl_Msc2.Name = "lbl_Msc2";
            this.lbl_Msc2.Size = new System.Drawing.Size(87, 13);
            this.lbl_Msc2.TabIndex = 1;
            this.lbl_Msc2.Text = "Dark Necessities";
            // 
            // lbl_Msc3
            // 
            this.lbl_Msc3.AutoSize = true;
            this.lbl_Msc3.Location = new System.Drawing.Point(504, 97);
            this.lbl_Msc3.Name = "lbl_Msc3";
            this.lbl_Msc3.Size = new System.Drawing.Size(98, 13);
            this.lbl_Msc3.TabIndex = 2;
            this.lbl_Msc3.Text = "Seven Nation Army";
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
            // frm_TelaSelecaoMusica
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_Msc3);
            this.Controls.Add(this.btn_Msc2);
            this.Controls.Add(this.btn_Msc1);
            this.Controls.Add(this.lbl_Msc3);
            this.Controls.Add(this.lbl_Msc2);
            this.Controls.Add(this.lbl_msc1);
            this.Name = "frm_TelaSelecaoMusica";
            this.Text = "SelecaoMusica";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_msc1;
        private System.Windows.Forms.Label lbl_Msc2;
        private System.Windows.Forms.Label lbl_Msc3;
        private System.Windows.Forms.Button btn_Msc1;
        private System.Windows.Forms.Button btn_Msc2;
        private System.Windows.Forms.Button btn_Msc3;
    }
}