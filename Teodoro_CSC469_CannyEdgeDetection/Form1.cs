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

        public Bitmap grayScale(Bitmap original)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    //get the pixels from the original image
                    Color originalColor = original.GetPixel(i, j);
                    //create the gray scale version of each pixel
                    int grayScale = (int)((originalColor.R * 0.3) + (originalColor.G * 0.59) + (originalColor.B * 0.11));
                    Color newColor = Color.FromArgb(grayScale, grayScale, grayScale);
                    newBitmap.SetPixel(i, j, newColor);
                }
            }
            return newBitmap;
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

        public void calcGradients(Bitmap original, float[,] gradient, float[,] gradientX, float[,] gradientY)
        {
            Color pixelbackrow;
            Color pixelbackcol;
            Color pixelfrontrow;
            Color pixelfrontcol;
            for(int i = 1; i < original.Width-3; i++)
            {
                for( int j = 1; j < original.Height-3; j++)
                {
                    pixelbackrow = original.GetPixel(i-1, j);
                    pixelbackcol = original.GetPixel(i, j-1);
                    pixelfrontrow = original.GetPixel(i+1, j);
                    pixelfrontcol = original.GetPixel(i, j+1);
                    gradientX[i, j] = (float)(((pixelfrontrow.R + pixelfrontrow.G + pixelfrontrow.B) / 3) - ((pixelbackrow.R + pixelfrontrow.G + pixelfrontrow.B) / 3) / 2);
                    gradientY[i, j] = (float)(((pixelfrontcol.R + pixelfrontrow.G + pixelfrontrow.B) / 3) - ((pixelbackcol.R + pixelfrontrow.G + pixelfrontrow.B) / 3) / 2);
                    gradient[i, j] = gradientX[i, j] + gradientY[i, j];
                }
            }
        }
        public void calcNonMaximalSuppression(Bitmap original, float[,] gradientX, float[,] gradientY, float[,] edgeDirection)
        {
            const double PI = 3.14159;
            float currAngle;
            decimal temp;
            double tempX, tempY;
            for(int i = 1; i < original.Width-2; i++)
            {
                for(int j = 1; j < original.Height-2; j++)
                {
                    //Convert float to double
                    temp = new decimal(gradientX[i, j]);
                    tempX = (double)temp;
                    temp = new decimal(gradientY[i, j]);
                    tempY = (double)temp;

                    //Convert to Deg
                    currAngle = (float)(Math.Atan2(tempY, tempX) / PI) * 180; //Convert to Deg

                    //Determine Non-maximal Suppression
                    if ((currAngle <= 22.5 && currAngle > -22.5) || (currAngle >= 157.5) || currAngle < -157.5 )
                    {
                        edgeDirection[i, j] = 0;
                    }else if ((currAngle >= 22.5 && currAngle < 67.5) || (currAngle <= -112.5) || currAngle > -157.5 )
                    {
                        edgeDirection[i, j] = 45;
                    }else if ((currAngle >= 67.5 && currAngle < 112.5) || (currAngle <= -67.5) || currAngle > -112.5 )
                    {
                        edgeDirection[i, j] = 90;
                    }else if ((currAngle >= 112.5 && currAngle < 157.5) || (currAngle <= -22.5) || currAngle > -67.5 )
                    {
                        edgeDirection[i, j] = 135;
                    }
                }
            }
        }

        public Bitmap drawBitmap(Bitmap original, float[,] pixelAngle, float[,] gradient, int upperThreshold, int lowerThreshold)
        {
            bool isEdgeEnd = false;
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            Color background = Color.Black;
            for(int i = 1; i < original.Width-2; i++)
            {
                for(int j = 1; j < original.Height-2; j++)
                {
                    isEdgeEnd = false;
                    if(gradient[i,j] > upperThreshold)
                    {
                        switch(pixelAngle[i,j])
                        {
                            case 0:
                                newBitmap = findEdge(original, newBitmap, pixelAngle, gradient, 0, 1, i, j, 0, lowerThreshold);
                                break;
                            case 45:
                                newBitmap = findEdge(original, newBitmap, pixelAngle, gradient, 1, 1, i, j, 45, lowerThreshold);
                                break;
                            case 90:
                                newBitmap = findEdge(original, newBitmap, pixelAngle, gradient, 1, 0, i, j, 90, lowerThreshold);
                                break;
                            case 135:
                                newBitmap = findEdge(original, newBitmap, pixelAngle, gradient, 1, -1, i, j, 135, lowerThreshold);
                                break;
                            default:
                                //make black
                                newBitmap.SetPixel(i, j, background);
                                break;
                        }
                    }
                    else
                    {
                        newBitmap.SetPixel(i,j, background);
                    }
                }
            }
            return newBitmap;
        }

        private Bitmap findEdge(Bitmap original, Bitmap newBitmap, float[,] edgeDir, float[,] gradients, int rowShift, int colShift, int row, int col, int dir, int lowerThreshold)
        {
            int newRow = 0, newCol = 0;
            bool edgeEnd = false;
            Color foreground = Color.White;
            //Find the end of the edge
            if (colShift < 0)
            {
                if (col > 0)
                {
                    newCol = col + colShift;
                }
                else
                {
                    edgeEnd = true;
                }
            } 
            else if (col < original.Width - 1)
            {
                newCol = col + colShift;
            } 
            else
            {
                edgeEnd = true;
            }

            if(rowShift < 0) 
            {
                if (row > 0)
                {
                    newRow = row + rowShift;
                }
                else
                {
                    edgeEnd = true;
                }
            } else if (row < original.Height - 1)
            {
                newRow = row + rowShift;
            } else
            {
                edgeEnd = true;
            }

            while ( (edgeDir[newRow, newCol] == dir) && !edgeEnd && (gradients[newRow, newCol] > lowerThreshold))
            {
                newBitmap.SetPixel(newRow, newCol, foreground);
                if (colShift < 0)
                {
                    if (col > 0)
                    {
                        newCol = col + colShift;
                    }
                    else
                    {
                        edgeEnd = true;
                    }
                }
                else if (col < original.Width - 1)
                {
                    newCol = col + colShift;
                }
                else
                {
                    edgeEnd = true;
                }

                if (rowShift < 0)
                {
                    if (row > 0)
                    {
                        newRow = row + rowShift;
                    }
                    else
                    {
                        edgeEnd = true;
                    }
                }
                else if (row < original.Height - 1)
                {
                    newRow = row + rowShift;
                }
                else
                {
                    edgeEnd = true;
                }

            }
            
            return newBitmap;
        }
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

            //GrayScale 
            Bitmap newImage = grayScale(original);

            //Smoothing
            newImage = calcSmoothing(newImage);

            //Find Gradient
            float[,] gradient = new float[newImage.Width, newImage.Height];
            float[,] gradientX = new float[newImage.Width, newImage.Height];
            float[,] gradientY = new float[newImage.Width, newImage.Height];
            calcGradients(newImage, gradient, gradientX, gradientY);

            //Non-maximal Suppression
            float[,] pixelAngle = new float[newImage.Width, newImage.Height]; 
            calcNonMaximalSuppression(newImage, gradientX, gradientY, pixelAngle);
           
            //Draw Bitmap
            int upperThreshold = 60, lowerThreshold = 30;
            newImage = drawBitmap(newImage, pixelAngle, gradient, upperThreshold, lowerThreshold);
 

            //Thresholding Hysterias

            //Display 
            PictureBox cannyPictureBox = new PictureBox();
            cannyPictureBox.Size = newImage.Size;
            cannyPictureBox.Image = newImage;
            Form edgeDetection = new Form2();
            edgeDetection.Controls.Add(cannyPictureBox);
            edgeDetection.Show();
        }
    }
}
