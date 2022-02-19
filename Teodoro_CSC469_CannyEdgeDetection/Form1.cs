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
        
        public Color applyMask(Bitmap original, int[][] smoothingMask, int x, int y)
        {
            try
            {
                int matrixTot = 0;
                Color yComponent.Value = 0;
                x -= 2;
                y -= 2;
                for(int i = 0; i < smoothingMask.Length; i++)
                {
                    for (int j = 0; j < smoothingMask.Length; j++)
                    {
                        yComponent.Value += original.GetPixel(x, y) * smoothingMask[i][j];
                    }
                }
                return yComponent;
            }
            catch
            { 
                throw new NotImplementedException();
            }
        }

        public Bitmap calcSmoothing(Bitmap original)
        {
            try
            {
                //Declarations
                Color yComponent;
                int[,] smoothingMatrix = new int[5, 5] { { 1, 4, 7, 4, 1}, { 4, 16, 26, 16, 4 }, { 7, 26, 41, 26, 7},
                                                    {4, 16, 26, 16, 4}, {1, 4, 7, 4, 1 } };
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        System.Console.WriteLine(smoothingMatrix[i, j]);
                    }
                    System.Console.WriteLine();
                }

                //Compute Smoothing
                Bitmap smoothingBitmap = new Bitmap(original.Width, original.Height);
                for(int i = 0; i < original.Width; i++)
                {
                    for(int j = 0; j < original.Height; j++)
                    {   
                        //Finds yComponent
                        //  Edge Cases: copy edges
                        //if(i < 2 || j  < 2 || i > original.Width || j > original.Height)
                        //{
                        //    for(int m = 0; m < 5; m++)
                        //    {
                        //        for(int n = 0; n < 5; n++)
                        //        {
                        //            Color originalColor = original.GetPixel(i, j); 
                        //        }
                        //    }
                        //}
                        yComponent = original.GetPixel(i, j);
                        //Compute math
                        for(int m = 0; m < 5; m++)
                        {
                            for(int n = 0; n < 5; n++)
                            {
                                
                            }
                        }
                    }
                }

                return original;
            }
            catch
            {
                throw new NotImplementedException();
            }
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
