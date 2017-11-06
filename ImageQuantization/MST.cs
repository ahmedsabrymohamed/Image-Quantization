using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    public class Node
    {
        public int source, knum, mainNode;
        public double weight;
        public bool visted;
        public Node(int i, int d, double j)
        {
            source = i;
            mainNode = d;
            weight = j;
            knum = -1;
            visted = false;
        }


    }
    public class MST
    {
        public double cost;
        int count;
        public List<Node> L;
        int V;
        List<RGBPixel> c;
        double weight, dr, dg, db;
        bool[] visited;
        double min;
        int y2, y1;
        public MST(int n, List<RGBPixel> d)
        {
            min = 1000000;
            count = 1;
            c = d;
            visited = new bool[n];
            V = n;
            L = new List<Node>(V + 2);
            L.Add(new Node(-1, 0, 0));
            for (int i = 1; i < V; i++)
            {
                dr = c[0].red - c[i].red; dr = dr * dr;
                dg = c[0].green - c[i].green;
                dg = dg * dg;
                db = c[0].blue - c[i].blue;
                db = db * db;
                weight = dr + dg + db;
                weight = Math.Sqrt(weight);
                if (weight < min)
                {
                    min = weight;
                    y1 = i;
                }
                L.Add(new Node(0, i, weight));
            }
            visited[0] = true;
            visited[y1] = true;
        }

        public void MST_Construct()
        {
            while (count != V)
            {
                min = 100000000;
                y2 = y1;
                for (int i = 1; i < V; i++)
                {
                    if (visited[i] == false)
                    {
                        dr = c[y2].red - c[i].red;
                        dr = dr * dr;
                        dg = c[y2].green - c[i].green;
                        dg = dg * dg;
                        db = c[y2].blue - c[i].blue;
                        db = db * db;
                        weight = dr + dg + db;
                        weight = Math.Sqrt(weight);
                        if (weight < L[i].weight)
                        {
                            L[i].weight = weight;
                            L[i].source = y2;
                        }

                        if (L[i].weight < min)
                        {
                            min = L[i].weight;
                            y1 = L[i].mainNode;
                        }
                    }
                }
                visited[y1] = true;
                count++;
            }
        }

        public void total_cost()
        {
            cost = 0;
            for (int i = 0; i < V; i++)
            {
                cost += L[i].weight;
            }

        }
    }
}
