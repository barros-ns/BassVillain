namespace BassVillainApp
{
    partial class frm_Configuracoes
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
            this.btn_voltar = new BassVillainApp.BotaoTema();
            this.lbl_titulo = new System.Windows.Forms.Label();
            this.lbl_secaoAudio = new System.Windows.Forms.Label();
            this.lbl_volume = new System.Windows.Forms.Label();
            this.trk_Volume = new System.Windows.Forms.TrackBar();
            this.lbl_volumeValor = new System.Windows.Forms.Label();
            this.lbl_secaoSincronia = new System.Windows.Forms.Label();
            this.lbl_offset = new System.Windows.Forms.Label();
            this.trk_Offset = new System.Windows.Forms.TrackBar();
            this.lbl_offsetValor = new System.Windows.Forms.Label();
            this.lbl_offsetDica = new System.Windows.Forms.Label();
            this.lbl_secaoTeclas = new System.Windows.Forms.Label();
            this.lbl_pista0 = new System.Windows.Forms.Label();
            this.btn_Tecla0 = new BassVillainApp.BotaoTema();
            this.lbl_pista1 = new System.Windows.Forms.Label();
            this.btn_Tecla1 = new BassVillainApp.BotaoTema();
            this.lbl_pista2 = new System.Windows.Forms.Label();
            this.btn_Tecla2 = new BassVillainApp.BotaoTema();
            this.lbl_pista3 = new System.Windows.Forms.Label();
            this.btn_Tecla3 = new BassVillainApp.BotaoTema();
            this.lbl_avisoTecla = new System.Windows.Forms.Label();
            this.btn_Restaurar = new BassVillainApp.BotaoTema();
            this.chk_Debug = new System.Windows.Forms.CheckBox();
            this.btn_TelaCheia = new System.Windows.Forms.Button();

            this.btn_TelaCheia.Name = "btn_TelaCheia";
            this.btn_TelaCheia.Text = "TELA CHEIA";
            this.btn_TelaCheia.Size = new System.Drawing.Size(150, 35);
            this.btn_TelaCheia.Location = new System.Drawing.Point(600, 14);
            this.btn_TelaCheia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_TelaCheia.ForeColor = System.Drawing.Color.White;
            this.btn_TelaCheia.BackColor = System.Drawing.Color.FromArgb(25, 35, 55);
            this.btn_TelaCheia.FlatAppearance.BorderSize = 0;
            this.btn_TelaCheia.Cursor = System.Windows.Forms.Cursors.Hand;

            this.Controls.Add(this.btn_TelaCheia);
            ((System.ComponentModel.ISupportInitialize)(this.trk_Volume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trk_Offset)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_voltar
            // 
            this.btn_voltar.BackColor = System.Drawing.Color.Transparent;
            this.btn_voltar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_voltar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_voltar.Location = new System.Drawing.Point(22, 14);
            this.btn_voltar.Name = "btn_voltar";
            this.btn_voltar.Size = new System.Drawing.Size(160, 50);
            this.btn_voltar.TabIndex = 22;
            this.btn_voltar.Text = "Voltar";
            this.btn_voltar.UseVisualStyleBackColor = true;
            // 
            // lbl_titulo
            // 
            this.lbl_titulo.AutoSize = true;
            this.lbl_titulo.Font = new System.Drawing.Font("Bauhaus 93", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_titulo.Location = new System.Drawing.Point(300, 25);
            this.lbl_titulo.Name = "lbl_titulo";
            this.lbl_titulo.Size = new System.Drawing.Size(233, 30);
            this.lbl_titulo.TabIndex = 21;
            this.lbl_titulo.Text = "CONFIGURAÇÕES";
            // 
            // lbl_secaoAudio
            // 
            this.lbl_secaoAudio.AutoSize = true;
            this.lbl_secaoAudio.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_secaoAudio.Location = new System.Drawing.Point(50, 100);
            this.lbl_secaoAudio.Name = "lbl_secaoAudio";
            this.lbl_secaoAudio.Size = new System.Drawing.Size(71, 21);
            this.lbl_secaoAudio.TabIndex = 20;
            this.lbl_secaoAudio.Text = "ÁUDIO";
            // 
            // lbl_volume
            // 
            this.lbl_volume.AutoSize = true;
            this.lbl_volume.Font = new System.Drawing.Font("Bauhaus 93", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_volume.Location = new System.Drawing.Point(50, 130);
            this.lbl_volume.Name = "lbl_volume";
            this.lbl_volume.Size = new System.Drawing.Size(112, 15);
            this.lbl_volume.TabIndex = 19;
            this.lbl_volume.Text = "Volume da música";
            // 
            // trk_Volume
            // 
            this.trk_Volume.Location = new System.Drawing.Point(200, 130);
            this.trk_Volume.Maximum = 100;
            this.trk_Volume.Name = "trk_Volume";
            this.trk_Volume.Size = new System.Drawing.Size(420, 45);
            this.trk_Volume.TabIndex = 18;
            this.trk_Volume.TickFrequency = 10;
            // 
            // lbl_volumeValor
            // 
            this.lbl_volumeValor.AutoSize = true;
            this.lbl_volumeValor.Font = new System.Drawing.Font("Bauhaus 93", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_volumeValor.Location = new System.Drawing.Point(640, 140);
            this.lbl_volumeValor.Name = "lbl_volumeValor";
            this.lbl_volumeValor.Size = new System.Drawing.Size(29, 13);
            this.lbl_volumeValor.TabIndex = 17;
            this.lbl_volumeValor.Text = "70%";
            // 
            // lbl_secaoSincronia
            // 
            this.lbl_secaoSincronia.AutoSize = true;
            this.lbl_secaoSincronia.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_secaoSincronia.Location = new System.Drawing.Point(50, 230);
            this.lbl_secaoSincronia.Name = "lbl_secaoSincronia";
            this.lbl_secaoSincronia.Size = new System.Drawing.Size(111, 21);
            this.lbl_secaoSincronia.TabIndex = 16;
            this.lbl_secaoSincronia.Text = "SINCRONIA";
            // 
            // lbl_offset
            // 
            this.lbl_offset.AutoSize = true;
            this.lbl_offset.Font = new System.Drawing.Font("Bauhaus 93", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_offset.Location = new System.Drawing.Point(50, 260);
            this.lbl_offset.Name = "lbl_offset";
            this.lbl_offset.Size = new System.Drawing.Size(75, 15);
            this.lbl_offset.TabIndex = 15;
            this.lbl_offset.Text = "Calibração";
            // 
            // trk_Offset
            // 
            this.trk_Offset.Location = new System.Drawing.Point(200, 260);
            this.trk_Offset.Maximum = 300;
            this.trk_Offset.Minimum = -300;
            this.trk_Offset.Name = "trk_Offset";
            this.trk_Offset.Size = new System.Drawing.Size(420, 45);
            this.trk_Offset.TabIndex = 14;
            this.trk_Offset.TickFrequency = 50;
            // 
            // lbl_offsetValor
            // 
            this.lbl_offsetValor.AutoSize = true;
            this.lbl_offsetValor.Font = new System.Drawing.Font("Bauhaus 93", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_offsetValor.Location = new System.Drawing.Point(640, 270);
            this.lbl_offsetValor.Name = "lbl_offsetValor";
            this.lbl_offsetValor.Size = new System.Drawing.Size(29, 13);
            this.lbl_offsetValor.TabIndex = 13;
            this.lbl_offsetValor.Text = "0 ms";
            // 
            // lbl_offsetDica
            // 
            this.lbl_offsetDica.Font = new System.Drawing.Font("Bauhaus 93", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_offsetDica.Location = new System.Drawing.Point(50, 310);
            this.lbl_offsetDica.Name = "lbl_offsetDica";
            this.lbl_offsetDica.Size = new System.Drawing.Size(700, 36);
            this.lbl_offsetDica.TabIndex = 12;
            this.lbl_offsetDica.Text = "Notas chegando tarde demais em relação à música? Aumente. Cedo demais? Diminua.\nS" +
    "e o som chega atrasado (ex.: fone Bluetooth), use valores negativos.";
            // 
            // lbl_secaoTeclas
            // 
            this.lbl_secaoTeclas.AutoSize = true;
            this.lbl_secaoTeclas.Font = new System.Drawing.Font("Bauhaus 93", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_secaoTeclas.Location = new System.Drawing.Point(50, 375);
            this.lbl_secaoTeclas.Name = "lbl_secaoTeclas";
            this.lbl_secaoTeclas.Size = new System.Drawing.Size(74, 21);
            this.lbl_secaoTeclas.TabIndex = 11;
            this.lbl_secaoTeclas.Text = "TECLAS";
            // 
            // lbl_pista0
            // 
            this.lbl_pista0.AutoSize = true;
            this.lbl_pista0.Font = new System.Drawing.Font("Bauhaus 93", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_pista0.Location = new System.Drawing.Point(155, 413);
            this.lbl_pista0.Name = "lbl_pista0";
            this.lbl_pista0.Size = new System.Drawing.Size(45, 15);
            this.lbl_pista0.TabIndex = 10;
            this.lbl_pista0.Text = "Pista 1";
            // 
            // btn_Tecla0
            // 
            this.btn_Tecla0.BackColor = System.Drawing.Color.Transparent;
            this.btn_Tecla0.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Tecla0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Tecla0.Location = new System.Drawing.Point(120, 440);
            this.btn_Tecla0.Name = "btn_Tecla0";
            this.btn_Tecla0.Size = new System.Drawing.Size(110, 52);
            this.btn_Tecla0.TabIndex = 9;
            this.btn_Tecla0.Text = "A";
            this.btn_Tecla0.UseVisualStyleBackColor = true;
            // 
            // lbl_pista1
            // 
            this.lbl_pista1.AutoSize = true;
            this.lbl_pista1.Font = new System.Drawing.Font("Bauhaus 93", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_pista1.Location = new System.Drawing.Point(295, 413);
            this.lbl_pista1.Name = "lbl_pista1";
            this.lbl_pista1.Size = new System.Drawing.Size(45, 15);
            this.lbl_pista1.TabIndex = 8;
            this.lbl_pista1.Text = "Pista 2";
            // 
            // btn_Tecla1
            // 
            this.btn_Tecla1.BackColor = System.Drawing.Color.Transparent;
            this.btn_Tecla1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Tecla1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Tecla1.Location = new System.Drawing.Point(260, 440);
            this.btn_Tecla1.Name = "btn_Tecla1";
            this.btn_Tecla1.Size = new System.Drawing.Size(110, 52);
            this.btn_Tecla1.TabIndex = 7;
            this.btn_Tecla1.Text = "A";
            this.btn_Tecla1.UseVisualStyleBackColor = true;
            // 
            // lbl_pista2
            // 
            this.lbl_pista2.AutoSize = true;
            this.lbl_pista2.Font = new System.Drawing.Font("Bauhaus 93", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_pista2.Location = new System.Drawing.Point(435, 413);
            this.lbl_pista2.Name = "lbl_pista2";
            this.lbl_pista2.Size = new System.Drawing.Size(45, 15);
            this.lbl_pista2.TabIndex = 6;
            this.lbl_pista2.Text = "Pista 3";
            // 
            // btn_Tecla2
            // 
            this.btn_Tecla2.BackColor = System.Drawing.Color.Transparent;
            this.btn_Tecla2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Tecla2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Tecla2.Location = new System.Drawing.Point(400, 440);
            this.btn_Tecla2.Name = "btn_Tecla2";
            this.btn_Tecla2.Size = new System.Drawing.Size(110, 52);
            this.btn_Tecla2.TabIndex = 5;
            this.btn_Tecla2.Text = "A";
            this.btn_Tecla2.UseVisualStyleBackColor = true;
            // 
            // lbl_pista3
            // 
            this.lbl_pista3.AutoSize = true;
            this.lbl_pista3.Font = new System.Drawing.Font("Bauhaus 93", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_pista3.Location = new System.Drawing.Point(575, 413);
            this.lbl_pista3.Name = "lbl_pista3";
            this.lbl_pista3.Size = new System.Drawing.Size(45, 15);
            this.lbl_pista3.TabIndex = 4;
            this.lbl_pista3.Text = "Pista 4";
            // 
            // btn_Tecla3
            // 
            this.btn_Tecla3.BackColor = System.Drawing.Color.Transparent;
            this.btn_Tecla3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Tecla3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Tecla3.Location = new System.Drawing.Point(540, 440);
            this.btn_Tecla3.Name = "btn_Tecla3";
            this.btn_Tecla3.Size = new System.Drawing.Size(110, 52);
            this.btn_Tecla3.TabIndex = 3;
            this.btn_Tecla3.Text = "A";
            this.btn_Tecla3.UseVisualStyleBackColor = true;
            // 
            // lbl_avisoTecla
            // 
            this.lbl_avisoTecla.Font = new System.Drawing.Font("Bauhaus 93", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_avisoTecla.Location = new System.Drawing.Point(50, 500);
            this.lbl_avisoTecla.Name = "lbl_avisoTecla";
            this.lbl_avisoTecla.Size = new System.Drawing.Size(700, 20);
            this.lbl_avisoTecla.TabIndex = 2;
            this.lbl_avisoTecla.Text = "Clique em uma tecla e depois pressione a nova (Esc cancela).";
            // 
            // btn_Restaurar
            // 
            this.btn_Restaurar.BackColor = System.Drawing.Color.Transparent;
            this.btn_Restaurar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Restaurar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Restaurar.Location = new System.Drawing.Point(50, 560);
            this.btn_Restaurar.Name = "btn_Restaurar";
            this.btn_Restaurar.Size = new System.Drawing.Size(260, 52);
            this.btn_Restaurar.TabIndex = 1;
            this.btn_Restaurar.Text = "Restaurar padrão";
            this.btn_Restaurar.UseVisualStyleBackColor = true;
            // 
            // chk_Debug
            // 
            this.chk_Debug.AutoSize = true;
            this.chk_Debug.Font = new System.Drawing.Font("Bauhaus 93", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_Debug.Location = new System.Drawing.Point(50, 620);
            this.chk_Debug.Name = "chk_Debug";
            this.chk_Debug.Size = new System.Drawing.Size(402, 19);
            this.chk_Debug.TabIndex = 0;
            this.chk_Debug.Text = "Modo debug (mostra os botões Vencer / Perder durante a partida)";
            this.chk_Debug.UseVisualStyleBackColor = true;
            // 
            // frm_Configuracoes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 642);
            this.Controls.Add(this.chk_Debug);
            this.Controls.Add(this.btn_Restaurar);
            this.Controls.Add(this.lbl_avisoTecla);
            this.Controls.Add(this.btn_Tecla3);
            this.Controls.Add(this.lbl_pista3);
            this.Controls.Add(this.btn_Tecla2);
            this.Controls.Add(this.lbl_pista2);
            this.Controls.Add(this.btn_Tecla1);
            this.Controls.Add(this.lbl_pista1);
            this.Controls.Add(this.btn_Tecla0);
            this.Controls.Add(this.lbl_pista0);
            this.Controls.Add(this.lbl_secaoTeclas);
            this.Controls.Add(this.lbl_offsetDica);
            this.Controls.Add(this.lbl_offsetValor);
            this.Controls.Add(this.trk_Offset);
            this.Controls.Add(this.lbl_offset);
            this.Controls.Add(this.lbl_secaoSincronia);
            this.Controls.Add(this.lbl_volumeValor);
            this.Controls.Add(this.trk_Volume);
            this.Controls.Add(this.lbl_volume);
            this.Controls.Add(this.lbl_secaoAudio);
            this.Controls.Add(this.lbl_titulo);
            this.Controls.Add(this.btn_voltar);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "frm_Configuracoes";
            this.Text = "Configurações";
            ((System.ComponentModel.ISupportInitialize)(this.trk_Volume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trk_Offset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BassVillainApp.BotaoTema btn_voltar;
        private System.Windows.Forms.Label lbl_titulo;
        private System.Windows.Forms.Label lbl_secaoAudio;
        private System.Windows.Forms.Label lbl_volume;
        private System.Windows.Forms.TrackBar trk_Volume;
        private System.Windows.Forms.Label lbl_volumeValor;
        private System.Windows.Forms.Label lbl_secaoSincronia;
        private System.Windows.Forms.Label lbl_offset;
        private System.Windows.Forms.TrackBar trk_Offset;
        private System.Windows.Forms.Label lbl_offsetValor;
        private System.Windows.Forms.Label lbl_offsetDica;
        private System.Windows.Forms.Label lbl_secaoTeclas;
        private System.Windows.Forms.Label lbl_pista0;
        private BassVillainApp.BotaoTema btn_Tecla0;
        private System.Windows.Forms.Label lbl_pista1;
        private BassVillainApp.BotaoTema btn_Tecla1;
        private System.Windows.Forms.Label lbl_pista2;
        private BassVillainApp.BotaoTema btn_Tecla2;
        private System.Windows.Forms.Label lbl_pista3;
        private BassVillainApp.BotaoTema btn_Tecla3;
        private System.Windows.Forms.Label lbl_avisoTecla;
        private BassVillainApp.BotaoTema btn_Restaurar;
        private System.Windows.Forms.CheckBox chk_Debug;
        private System.Windows.Forms.Button btn_TelaCheia;
    }
}
