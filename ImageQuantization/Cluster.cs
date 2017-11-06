using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuantization
{  class Cluster
    {

        List<Node> MSTree;
        List<RGBPixel>[] clusters;
        RGBPixel[] colors_centroid;
        int[, ,] colors_knum;
        List<RGBPixel> colors;
        
        int clnum;
        int next_cluster;
        

        public Cluster(List<Node> m, List<RGBPixel> c, int k)
        {
            next_cluster = 0;
            MSTree = m;
            colors = c;
            clnum = k;
            clusters = new List<RGBPixel>[k];

        }
        public void function()
        {

            int k = clnum - 1;
            while (k > 0)
            {
                k--;
                double max = -1;
                int index = -1;
                for (int i = 0; i < MSTree.Count; i++)
                    if (MSTree[i].weight > max)
                    {
                        max = MSTree[i].weight;
                        index = i;
                    }
                MSTree[index].weight = -2;
                MSTree[index].source = -1;
            }
            k = 0;
            for (int j = 0; j < MSTree.Count; j++)
                if (MSTree[j].visted == false)
                    get_cluster(j);

            ;
        }
        
        private int get_cluster(int j)
        {
            if (MSTree[j].source == -1)
            {
                MSTree[j].knum = next_cluster;
                clusters[MSTree[j].knum] = new List<RGBPixel>();
                next_cluster++;
            }
            else if (MSTree[MSTree[j].source].visted)
            {
                MSTree[j].knum = MSTree[MSTree[j].source].knum;
            }
            else
            {
                MSTree[j].knum = get_cluster(MSTree[j].source);
            }
            clusters[MSTree[j].knum].Add(colors[j]);//
            MSTree[j].visted = true;
            return MSTree[j].knum;
        }
       
        public void Color_Pallette()//4-Find Representative color of each Cluster Time:O(D)D-->Clusters(distinct Colors)
        {

            colors_centroid = new RGBPixel[clusters.Length];
            colors_knum = new int[265, 265, 265];
            for (int i = 0; i < clusters.Length; i++)
            {
                int R = 0; int g = 0; int b = 0;

                List<RGBPixel> c = new List<RGBPixel>();
                
                c = clusters[i];
                
                for (int j = 0; j < c.Count; j++)
                {
                    R += c[j].red;
                    g += c[j].green;
                    b += c[j].blue;
                    colors_knum[c[j].red, c[j].green, c[j].blue]=i;
                    
                }
                RGBPixel color = new RGBPixel();
                color.red = (byte)(R / c.Count);
                color.green = (byte)(g / c.Count);
                color.blue = (byte)(b / c.Count);
                colors_centroid[i] = color;
            }

        }
        public void Quantize_the_image(RGBPixel[,] ImageMatrix) 
        {
            for (long i = 0; i < ImageOperations.GetHeight(ImageMatrix); i++)
            {
                for (long j = 0; j < ImageOperations.GetWidth(ImageMatrix); j++)
                {
                    ImageMatrix[i,j]=colors_centroid[colors_knum[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue]];
                }
            }
            
        }
    }
}
