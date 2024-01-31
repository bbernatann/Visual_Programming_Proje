namespace Fitness_Center
{
    partial class UyelikSuresiGrafiği
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UyelikSuresiGrafiği));
            this.btnGeri = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label2Ay = new System.Windows.Forms.Label();
            this.label1Ay = new System.Windows.Forms.Label();
            this.label1Yil = new System.Windows.Forms.Label();
            this.label6Ay = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGeri
            // 
            this.btnGeri.BackColor = System.Drawing.Color.White;
            this.btnGeri.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGeri.BackgroundImage")));
            this.btnGeri.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGeri.Location = new System.Drawing.Point(-3, 0);
            this.btnGeri.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGeri.Name = "btnGeri";
            this.btnGeri.Size = new System.Drawing.Size(43, 43);
            this.btnGeri.TabIndex = 44;
            this.btnGeri.UseVisualStyleBackColor = false;
            this.btnGeri.Click += new System.EventHandler(this.btnGeri_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("High Tower Text", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label5.Location = new System.Drawing.Point(422, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(456, 36);
            this.label5.TabIndex = 53;
            this.label5.Text = "Üyelerin Üyelik Süreleri Verileri";
            // 
            // label2Ay
            // 
            this.label2Ay.AutoSize = true;
            this.label2Ay.BackColor = System.Drawing.Color.White;
            this.label2Ay.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2Ay.Location = new System.Drawing.Point(626, 509);
            this.label2Ay.Name = "label2Ay";
            this.label2Ay.Size = new System.Drawing.Size(35, 16);
            this.label2Ay.TabIndex = 57;
            this.label2Ay.Text = "2 AY";
            // 
            // label1Ay
            // 
            this.label1Ay.AutoSize = true;
            this.label1Ay.BackColor = System.Drawing.Color.White;
            this.label1Ay.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1Ay.Location = new System.Drawing.Point(562, 509);
            this.label1Ay.Name = "label1Ay";
            this.label1Ay.Size = new System.Drawing.Size(38, 16);
            this.label1Ay.TabIndex = 56;
            this.label1Ay.Text = "1 AY ";
            // 
            // label1Yil
            // 
            this.label1Yil.AutoSize = true;
            this.label1Yil.BackColor = System.Drawing.Color.White;
            this.label1Yil.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1Yil.Location = new System.Drawing.Point(749, 509);
            this.label1Yil.Name = "label1Yil";
            this.label1Yil.Size = new System.Drawing.Size(36, 16);
            this.label1Yil.TabIndex = 55;
            this.label1Yil.Text = "1 YIL";
            // 
            // label6Ay
            // 
            this.label6Ay.AutoSize = true;
            this.label6Ay.BackColor = System.Drawing.Color.White;
            this.label6Ay.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label6Ay.Location = new System.Drawing.Point(686, 509);
            this.label6Ay.Name = "label6Ay";
            this.label6Ay.Size = new System.Drawing.Size(35, 16);
            this.label6Ay.TabIndex = 54;
            this.label6Ay.Text = "6 AY";
            // 
            // UyelikSuresiGrafiği
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1073, 584);
            this.Controls.Add(this.label2Ay);
            this.Controls.Add(this.label1Ay);
            this.Controls.Add(this.label1Yil);
            this.Controls.Add(this.label6Ay);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnGeri);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UyelikSuresiGrafiği";
            this.Text = "UyelikSuresiGrafiği";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.UyelikSuresiGrafiği_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGeri;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2Ay;
        private System.Windows.Forms.Label label1Ay;
        private System.Windows.Forms.Label label1Yil;
        private System.Windows.Forms.Label label6Ay;
    }
}