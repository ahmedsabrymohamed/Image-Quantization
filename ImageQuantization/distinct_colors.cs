using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class distinct_colors
    {
        public List<RGBPixel> colors;
        public int count;
        public distinct_colors()
        {
            colors = new List<RGBPixel>();
        }
        //function to find the difrent colors
        public void get_colors(RGBPixel[,] ImageMatrix)
        {
            bool[, ,] pixels = new bool[265, 265, 265];
            int height = ImageOperations.GetHeight(ImageMatrix);
            int width = ImageOperations.GetWidth(ImageMatrix);
            for (long i = 0; i <height ; i++)
            {
                for (long j = 0; j <width ; j++)
                {
                    if (pixels[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue] == false)
                    {
                        pixels[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue] = true;
                        colors.Add(ImageMatrix[i, j]);
                        count++;
                    }
                }
            }

        }
    }
    
}
