using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace NLCS
{
    class Prim
    {
        private int[] pi;
        private int[] p;
        private int[] mark;
        private const int inf = 9999999;
        public Prim(Graph G,ref Graph T, ref int sum_w, int start)
        {
            T = new Graph(G.Vertices);
            pi = new int[G.Vertices + 1];
            p = new int[G.Vertices + 1];
            mark = new int[G.Vertices + 1];
            int[,] GA = G.getGraph();
            for( int i = 1; i <= G.Vertices; i++)
            {
                pi[i] = inf;
            }
            for(int i = 1; i <= G.Vertices; i++)
            {
                if (GA[start, i] != 0)
                {
                    pi[i] = GA[start, i];
                    p[i] = start;
                }
            }
            for( int i = 1; i <= G.Vertices; i++)
            {
                mark[i] = 0;
            }
            mark[start] = 1;
            sum_w = 0;
            for(int it = 1; it < G.Vertices; it++)
            {
                int min_dist = inf;
                int min_u = 0;
                for(int i = 1; i <= G.Vertices; i++)
                {
                    if(mark[i]== 0 && pi[i] < min_dist)
                    {
                        min_dist = pi[i];
                        min_u = i;
                    }
                }

                mark[min_u] = 1;
                T.addEdge(min_u, p[min_u], min_dist);
                sum_w += min_dist;
                List<int> list = new List<int>();
                G.findNeighbor(ref list, min_u);
                for(int i = 0; i < list.Count; i++) 
                {
                    int y = list[i];
                    if(mark[y] == 0 && pi[y] > GA[min_u, y]) 
                    {
                        pi[y] = GA[min_u, y];
                        p[y] = min_u;
                    }
                }
            }
        }

        public void Print(int end, ref List<int[]> list)
        {
            int path = end;
            while(path != 1)
            {
                int[] array = new int[2];
                array[0] = this.p[path];
                array[1] = path;
                path = this.p[path];
                list.Add(array);
            }

        }
    }
}
