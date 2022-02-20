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
        
        public Color applyMask(Bitmap original, int[,] smoothingMask, int x, int y)
        {
            
            Color yComponent = original.GetPixel(x, y);
            x -= 2;
            int tempx = x;
            y -= 2;
            int gaussianBlurR = 0;
            int gaussianBlurG = 0;
            int gaussianBlurB = 0;
            Color colorBlur;
            for(int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    yComponent = original.GetPixel(x, y);
                    gaussianBlurR += (int)(yComponent.R * smoothingMask[i, j]);
                    gaussianBlurG += (int)(yComponent.G * smoothingMask[i, j]);
                    gaussianBlurB += (int)(yComponent.B * smoothingMask[i, j]);
                    x++;
                }
                x = tempx;
                y++;
            }
            gaussianBlurR += 136;
            gaussianBlurG += 136;
            gaussianBlurB += 136;
            gaussianBlurR /= 273;
            gaussianBlurG /= 273;
            gaussianBlurB /= 273;
            colorBlur = Color.FromArgb(gaussianBlurR, gaussianBlurG, gaussianBlurB);
            //yComponent.FromArgb(matrixTot);
            return colorBlur;
            
        }

        public Bitmap calcSmoothing(Bitmap original)
        {

            //Declarations
            //Color yComponent;
            Color yComponent;
            int[,] smoothingMatrix = new int[5, 5] { { 1, 4, 7, 4, 1}, { 4, 16, 26, 16, 4 }, { 7, 26, 41, 26, 7},
                                                {4, 16, 26, 16, 4}, {1, 4, 7, 4, 1 } };
            //for (int i = 0; i < 5; i++)
            //{
            //    for (int j = 0; j < 5; j++)
            //    {
            //        System.Console.WriteLine(smoothingMatrix[i, j]);
            //    }
            //    System.Console.WriteLine();
            //}

            //Compute Smoothing
            Bitmap smoothingBitmap = new Bitmap(original.Width, original.Height);
            for(int i = 5; i < (original.Width - 5); i++)
            {
                for(int j = 5; j < (original.Height - 5); j++)
                {
                    yComponent = applyMask(original, smoothingMatrix, i, j);
                    smoothingBitmap.SetPixel(i, j, yComponent);
                }
            }

            return smoothingBitmap;
            
            
        }

        //public Bitmap calcGradients(Bitmap original)
        //{
        //
        //}
        //
        //public Bitmap calcNonMaximalSuppression(Bitmap original)
        //{
        //
        //}
        //
        //public Bitmap calcThresholdingHysterias(Bitmap orginal)
        //{
        //
        //}

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
            Bitmap original = (Bitmap)pictureBox1.Image;

            //Smoothing
            Bitmap newImage = calcSmoothing(original);

            //Find Gradient
            //Non-maximal Suppression
            //Thresholding Hysterias
            PictureBox cannyPictureBox = new PictureBox();
            cannyPictureBox.Size = newImage.Size;
            cannyPictureBox.Image = newImage;
            Form edgeDetection = new Form2();
            edgeDetection.Controls.Add(cannyPictureBox);
            edgeDetection.Show();
        }
    }
}
