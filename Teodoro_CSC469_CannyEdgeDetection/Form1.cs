using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teodoro_CSC469_CannyEdgeDetection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void choosesImageButton_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog imagefileopen = new OpenFileDialog();
            imagefileopen.Filter = "Image Files(*.jpg;*.jpeg; *.gif; *.bmp; *.png)|*.jpg;*.jpeg; *.gif; *.bmp ; *.png";
            if (imagefileopen.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(imagefileopen.FileName);
                pictureBox1.Size = pictureBox1.Image.Size;
            }
        }

        private void cannyEdgeDetectionButton_Click(object sender, EventArgs e)
        {
            //Perform Canny Edge Detection

            //Smoothing
            //Find Gradient
            //Non-maximal Suppression
            //Thresholding Hysterias
        }
    }
}
