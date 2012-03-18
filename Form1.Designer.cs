namespace PreubaNETSH
{
    partial class AirWin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AirWin));
            this.diccionario = new System.Windows.Forms.TextBox();
            this.essid = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Autenticacion = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.auth = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tipo1 = new System.Windows.Forms.ComboBox();
            this.auth2 = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.ip = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.delay = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // diccionario
            // 
            this.diccionario.Location = new System.Drawing.Point(134, 10);
            this.diccionario.Name = "diccionario";
            this.diccionario.ReadOnly = true;
            this.diccionario.Size = new System.Drawing.Size(133, 20);
            this.diccionario.TabIndex = 0;
            // 
            // essid
            // 
            this.essid.Location = new System.Drawing.Point(134, 36);
            this.essid.Name = "essid";
            this.essid.Size = new System.Drawing.Size(133, 20);
            this.essid.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nombre Diccionario";
            // 
            // Autenticacion
            // 
            this.Autenticacion.AutoSize = true;
            this.Autenticacion.Location = new System.Drawing.Point(26, 62);
            this.Autenticacion.Name = "Autenticacion";
            this.Autenticacion.Size = new System.Drawing.Size(72, 13);
            this.Autenticacion.TabIndex = 2;
            this.Autenticacion.Text = "Autenticación";
            this.Autenticacion.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "ESSID";
            this.label3.Click += new System.EventHandler(this.label2_Click);
            // 
            // auth
            // 
            this.auth.AutoSize = true;
            this.auth.Location = new System.Drawing.Point(26, 90);
            this.auth.Name = "auth";
            this.auth.Size = new System.Drawing.Size(66, 13);
            this.auth.TabIndex = 2;
            this.auth.Text = "Encriptación";
            this.auth.Click += new System.EventHandler(this.label2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(192, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Empezar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(280, 144);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Ayuda";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(131, 186);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Probando clave: ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(224, 186);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "key";
            // 
            // tipo1
            // 
            this.tipo1.FormattingEnabled = true;
            this.tipo1.Items.AddRange(new object[] {
            "OPEN",
            "WPAPSK",
            "WPA2PSK"});
            this.tipo1.Location = new System.Drawing.Point(134, 62);
            this.tipo1.Name = "tipo1";
            this.tipo1.Size = new System.Drawing.Size(133, 21);
            this.tipo1.TabIndex = 5;
            this.tipo1.Text = "Seleccione:";
            // 
            // auth2
            // 
            this.auth2.FormattingEnabled = true;
            this.auth2.Items.AddRange(new object[] {
            "WEP",
            "TKIP",
            "AES"});
            this.auth2.Location = new System.Drawing.Point(134, 87);
            this.auth2.Name = "auth2";
            this.auth2.Size = new System.Drawing.Size(133, 21);
            this.auth2.TabIndex = 5;
            this.auth2.Text = "Seleccione:";
            this.auth2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(275, 11);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(67, 19);
            this.button3.TabIndex = 6;
            this.button3.Text = "Abrir";
            this.button3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // ip
            // 
            this.ip.FormattingEnabled = true;
            this.ip.Items.AddRange(new object[] {
            "192.168.0.1",
            "192.168.1.1",
            "192.168.2.1"});
            this.ip.Location = new System.Drawing.Point(134, 114);
            this.ip.Name = "ip";
            this.ip.Size = new System.Drawing.Size(133, 21);
            this.ip.TabIndex = 7;
            this.ip.Text = "192.168.0.1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Puerta Enlace";
            this.label2.Click += new System.EventHandler(this.label2_Click_1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Delay (seg).";
            this.label4.Click += new System.EventHandler(this.label2_Click_1);
            // 
            // delay
            // 
            this.delay.FormattingEnabled = true;
            this.delay.Items.AddRange(new object[] {
            "2",
            "4",
            "6",
            "8",
            "10",
            "12",
            "14",
            "16",
            "18",
            "20"});
            this.delay.Location = new System.Drawing.Point(134, 141);
            this.delay.Name = "delay";
            this.delay.Size = new System.Drawing.Size(36, 21);
            this.delay.TabIndex = 7;
            this.delay.Text = "10";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "txt";
            this.openFileDialog1.FileName = "Diccionario.txt";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk_1);
            // 
            // AirWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 218);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.delay);
            this.Controls.Add(this.ip);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.auth2);
            this.Controls.Add(this.tipo1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.auth);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Autenticacion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.essid);
            this.Controls.Add(this.diccionario);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AirWin";
            this.Text = "AirWin Alpha";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox diccionario;
        private System.Windows.Forms.TextBox essid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Autenticacion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label auth;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox tipo1;
        private System.Windows.Forms.ComboBox auth2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox ip;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox delay;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;

    }
}

