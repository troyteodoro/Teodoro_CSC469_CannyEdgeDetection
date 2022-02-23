using System;
using System.Drawing;
using System.Windows.Forms;

namespace Teodoro_CSC469_CannyEdgeDetection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Color applyMask(Bitmap original, int[,] smoothingMask, int x, int y, bool isEdge)
        {

            Color yComponent = original.GetPixel(x, y);
            int tempx = x;
            int gaussianBlurR = 0;
            int gaussianBlurG = 0;
            int gaussianBlurB = 0;
            Color colorBlur;


            if (isEdge)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (y == 2)
                        {
                            //repeat x values
                            y = 0;
                        }
                        yComponent = original.GetPixel(x, y);
                        gaussianBlurR += (int)(yComponent.R * smoothingMask[i, j]);
                        gaussianBlurG += (int)(yComponent.G * smoothingMask[i, j]);
                        gaussianBlurB += (int)(yComponent.B * smoothingMask[i, j]);
                        y++;
                        if (y == original.Height)
                        {
                            //repeat x values at -2
                            y = original.Height - 3;

                        }
                    }
                    if (x == 2)
                    {
                        //repeat x values
                        x = 0;

                    }
                    
                    x++;

                    if (x == original.Width)
                    {
                        //repeat x values at -2
                        x = original.Width - 3;

                    }
                    x = tempx;
                }
                gaussianBlurR += 136;
                gaussianBlurG += 136;
                gaussianBlurB += 136;
                gaussianBlurR /= 273;
                gaussianBlurG /= 273;
                gaussianBlurB /= 273;
                colorBlur = Color.FromArgb(gaussianBlurR, gaussianBlurG, gaussianBlurB);
                return colorBlur;
            }
            else
            {
                x -= 2;
                y -= 2;
                tempx = x;
                for (int i = 0; i < 5; i++)
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
                return colorBlur;
            }

        }

        public Bitmap calcSmoothing(Bitmap original)
        {

            //Declarations
            //Color yComponent;
            Color yComponent;
            int[,] smoothingMatrix = new int[5, 5] { { 1, 4, 7, 4, 1}, { 4, 16, 26, 16, 4 }, { 7, 26, 41, 26, 7},
                                                {4, 16, 26, 16, 4}, {1, 4, 7, 4, 1 } };
            bool isEdge = false;   
            //Debugging
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
            for (int i = 0; i < (original.Width); i++)
            {
                for (int j = 0; j < (original.Height); j++)
                {
                    //if out of bounds repeat edges
                    if(i <= 2 || j <= 2 || i >= (original.Width-2) || j >= (original.Height-2))
                    {
                        //repeat edges
                        isEdge = true;
                    }
                    yComponent = applyMask(original, smoothingMatrix, i, j, isEdge);
                    smoothingBitmap.SetPixel(i, j, yComponent);
                    isEdge = false;
                }
            }

            return smoothingBitmap;


        }

        public void calcGradients(Bitmap original, float[,] gradientX, float[,] gradientY)
        {
            for(int i = 1; i < original.Width-3; i++)
            {
                for( int j = 1; j < original.Height-3; j++)
                {
                    gradientX[i, j] = (float)((original.GetPixel(i - 1, j)).A - (original.GetPixel(i + 1, j)).A) / 2;
                    gradientY[i, j] = (float)((original.GetPixel(i, j - 1)).A - (original.GetPixel(i, j + 1)).A) / 2;
                }
            }
        }
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
            float[,] gradientX = new float[original.Width, original.Height];
            float[,] gradientY = new float[original.Width, original.Height];
            calcGradients(original, gradientX, gradientY);

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
