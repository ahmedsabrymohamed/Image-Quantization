using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuantization
{
    class Better_Way_Cluster
    {
        List<Node> MSTree;
        List<RGBPixel>[] clusters;
        List<RGBPixelD>[] cluster_centroid;
        List<RGBPixel> colors;
        MST tree;
        bool[] visted;

        int clnum;
        int clnum2;
        int next_cluster;
        double total;
        double stddiv;
        double mean;
        public Better_Way_Cluster(List<RGBPixel> c, int k)
        {
            colors = c;
            clnum = k - 1;
            calculate_std_mean();
        }
        private void calculate_std_mean()
        {
            double segma = 0;
            tree = new MST(colors.Count, colors);
            tree.MST_Construct();
            MSTree = tree.L;
            tree.total_cost();
            total = tree.cost;
            mean = total / (MSTree.Count);
            for (int i = 1; i < MSTree.Count; i++)
            {
                segma += (MSTree[i].weight * MSTree[i].weight) - (mean * mean);
            }
            stddiv = Math.Sqrt(segma / MSTree.Count);
            clnum2 = 0;
        }
        public void function()
        {
            while (clnum2 != clnum)
            {
                next_cluster = 0;
                calculate_std_mean();
                for (int i = 0; i < MSTree.Count; i++)
                    if (MSTree[i].weight > (stddiv + mean))
                    {
                        MSTree[i].weight = -2;
                        MSTree[i].source = -1;
                        clnum2++;
                    }
                while (clnum2 < clnum)
                {
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
                    clnum2++;
                }
                clusters = new List<RGBPixel>[clnum2 + 1];
                cluster_centroid = new List<RGBPixelD>[clnum2 + 1];
                for (int j = 0; j < MSTree.Count; j++)
                    if (MSTree[j].visted == false)
                        get_cluster(j);
                if (clnum2 > clnum)
                {
                    colors = new List<RGBPixel>();
                    for (int i = 0; i < clnum2; i++)
                    {
                        double min = 10000000;
                        int index = -1;
                        double weight, dr, dg, db;
                        for (int j = 0; j < clusters[i].Count; j++)
                        {
                            dr = clusters[i][j].red - cluster_centroid[i][0].red;
                            dr = dr * dr;
                            dg = clusters[i][j].green - cluster_centroid[i][0].green;
                            dg = dg * dg;
                            db = clusters[i][j].blue - cluster_centroid[i][0].blue;
                            db = db * db;
                            weight = dr + dg + db;
                            weight = Math.Sqrt(weight);
                            if (min > weight)
                            {
                                min = weight;
                                index = j;
                            }
                        }
                        colors.Add(clusters[i][index]);
                    }


                }


            }
        }
        //DP RECURION FUNCTION
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
            if (cluster_centroid[MSTree[j].knum] == null)
            {
                cluster_centroid[MSTree[j].knum] = new List<RGBPixelD>();
                RGBPixelD color = new RGBPixelD();
                color.red = colors[j].red;
                color.blue = colors[j].blue;
                color.green = colors[j].green;
                cluster_centroid[MSTree[j].knum].Add(color);
            }
            else
            {
                RGBPixelD color = new RGBPixelD();
                color.red = (cluster_centroid[MSTree[j].knum][0].red + colors[j].red) / 2;
                color.blue = (cluster_centroid[MSTree[j].knum][0].blue + colors[j].blue) / 2;
                color.green = (cluster_centroid[MSTree[j].knum][0].green + colors[j].green) / 2;
                cluster_centroid[MSTree[j].knum][0] = color;
                
            }
            clusters[MSTree[j].knum].Add(colors[j]);
            MSTree[j].visted = true;
            return MSTree[j].knum;
        }
        public void display()
        {
            for (int i = 0; i < clusters.Length; i++)
            {
                Console.WriteLine("Cluster");
                foreach (RGBPixel c in clusters[i])
                    Console.WriteLine(c.red + " " + c.green + " " + c.blue);
                Console.WriteLine("--------------------------------------------------------------------------");
            }
        }
        public void Color_Pallette()//4-Find Representative color of each Cluster Time:O(D)D-->Clusters(distinct Colors)
        {
            //Remove Contains in colors
            colors.RemoveRange(0, colors.Count);


            for (int i = 0; i < clusters.Length; i++)
            {
                double R = 0; double g = 0; double b = 0;
                List<RGBPixel> c = new List<RGBPixel>();
                c = clusters[i];
                for (int j = 0; j < c.Count; j++)
                {
                    R += c[j].red;
                    g += c[j].green;
                    b += c[j].blue;
                }
                RGBPixel color = new RGBPixel();
                color.red = (byte)(R / 2);
                color.green = (byte)(g / 2);
                color.blue = (byte)(b / 2);
                colors.Add(color);
            }
           
        }
    }
}
