namespace CompGrLab3
{
    partial class Task1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnDraw = new System.Windows.Forms.Button();
            this.btnFillColor = new System.Windows.Forms.Button();
            this.btnFillTexture = new System.Windows.Forms.Button();
            this.btnTraceBoundary = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnChooseFillColor = new System.Windows.Forms.Button();
            this.btnLoadTexture = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 450);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnDraw
            // 
            this.btnDraw.Location = new System.Drawing.Point(36, 399);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(75, 23);
            this.btnDraw.TabIndex = 1;
            this.btnDraw.Text = "Рисование";
            this.btnDraw.UseVisualStyleBackColor = true;
            this.btnDraw.Click += new System.EventHandler(this.btnDraw_Click);
            // 
            // btnFillColor
            // 
            this.btnFillColor.Location = new System.Drawing.Point(117, 399);
            this.btnFillColor.Name = "btnFillColor";
            this.btnFillColor.Size = new System.Drawing.Size(113, 23);
            this.btnFillColor.TabIndex = 2;
            this.btnFillColor.Text = "Заливка цветом";
            this.btnFillColor.UseVisualStyleBackColor = true;
            this.btnFillColor.Click += new System.EventHandler(this.btnFillColor_Click);
            // 
            // btnFillTexture
            // 
            this.btnFillTexture.Location = new System.Drawing.Point(236, 399);
            this.btnFillTexture.Name = "btnFillTexture";
            this.btnFillTexture.Size = new System.Drawing.Size(113, 23);
            this.btnFillTexture.TabIndex = 3;
            this.btnFillTexture.Text = "Заливка текстурой";
            this.btnFillTexture.UseVisualStyleBackColor = true;
            this.btnFillTexture.Click += new System.EventHandler(this.btnFillTexture_Click);
            // 
            // btnTraceBoundary
            // 
            this.btnTraceBoundary.Location = new System.Drawing.Point(355, 399);
            this.btnTraceBoundary.Name = "btnTraceBoundary";
            this.btnTraceBoundary.Size = new System.Drawing.Size(113, 23);
            this.btnTraceBoundary.TabIndex = 4;
            this.btnTraceBoundary.Text = "Обход границы";
            this.btnTraceBoundary.UseVisualStyleBackColor = true;
            this.btnTraceBoundary.Click += new System.EventHandler(this.btnTraceBoundary_Click);
            // 
            // btnChooseFillColor
            // 
            this.btnChooseFillColor.Location = new System.Drawing.Point(474, 399);
            this.btnChooseFillColor.Name = "btnChooseFillColor";
            this.btnChooseFillColor.Size = new System.Drawing.Size(131, 23);
            this.btnChooseFillColor.TabIndex = 5;
            this.btnChooseFillColor.Text = "Выбор цвета заливки";
            this.btnChooseFillColor.UseVisualStyleBackColor = true;
            this.btnChooseFillColor.Click += new System.EventHandler(this.btnChooseFillColor_Click);
            // 
            // btnLoadTexture
            // 
            this.btnLoadTexture.Location = new System.Drawing.Point(611, 399);
            this.btnLoadTexture.Name = "btnLoadTexture";
            this.btnLoadTexture.Size = new System.Drawing.Size(131, 23);
            this.btnLoadTexture.TabIndex = 6;
            this.btnLoadTexture.Text = "Загрузка текстуры";
            this.btnLoadTexture.UseVisualStyleBackColor = true;
            this.btnLoadTexture.Click += new System.EventHandler(this.btnLoadTexture_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Task1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnLoadTexture);
            this.Controls.Add(this.btnChooseFillColor);
            this.Controls.Add(this.btnTraceBoundary);
            this.Controls.Add(this.btnFillTexture);
            this.Controls.Add(this.btnFillColor);
            this.Controls.Add(this.btnDraw);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Task1";
            this.Text = "Task1";
            this.Load += new System.EventHandler(this.Task1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnDraw;
        private System.Windows.Forms.Button btnFillColor;
        private System.Windows.Forms.Button btnFillTexture;
        private System.Windows.Forms.Button btnTraceBoundary;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnChooseFillColor;
        private System.Windows.Forms.Button btnLoadTexture;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}