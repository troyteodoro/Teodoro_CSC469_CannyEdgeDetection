namespace Teodoro_CSC469_CannyEdgeDetection
{
    partial class Form1
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
            this.choosesImageButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cannyEdgeDetectionButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // choosesImageButton
            // 
            this.choosesImageButton.Location = new System.Drawing.Point(12, 12);
            this.choosesImageButton.Name = "choosesImageButton";
            this.choosesImageButton.Size = new System.Drawing.Size(97, 23);
            this.choosesImageButton.TabIndex = 0;
            this.choosesImageButton.Text = "Choose Image";
            this.choosesImageButton.UseVisualStyleBackColor = true;
            this.choosesImageButton.Click += new System.EventHandler(this.choosesImageButton_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 41);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(383, 316);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // cannyEdgeDetectionButton
            // 
            this.cannyEdgeDetectionButton.Location = new System.Drawing.Point(115, 12);
            this.cannyEdgeDetectionButton.Name = "cannyEdgeDetectionButton";
            this.cannyEdgeDetectionButton.Size = new System.Drawing.Size(140, 23);
            this.cannyEdgeDetectionButton.TabIndex = 2;
            this.cannyEdgeDetectionButton.Text = "Canny Edge Detection";
            this.cannyEdgeDetectionButton.UseVisualStyleBackColor = true;
            this.cannyEdgeDetectionButton.Click += new System.EventHandler(this.cannyEdgeDetectionButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cannyEdgeDetectionButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.choosesImageButton);
            this.Name = "Form1";
            this.Text = "Teodoro_CCS469_CannyEdgeDetection";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button choosesImageButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button cannyEdgeDetectionButton;
    }
}

