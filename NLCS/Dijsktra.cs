using System;
using System.Collections.Generic;
using System.Text;

namespace NLCS
{
    class Dijsktra
    {
        private int[] pi;
        private int[] p;
        private int[] mark;
        private const int inf = 9999999;
        public Dijsktra(Graph G, int start)
        {
            pi = new int[G.Vertices + 1];
            p = new int[G.Vertices + 1];
            mark = new int[G.Vertices + 1];
            int[,] GA = G.getGraph();
            for (int i = 1; i <= G.Vertices; i++)
            {
                pi[i] = inf;
            }
            for (int i = 1; i <= G.Vertices; i++)
            {
                mark[i] = 0;
            }
            pi[start] = 0;
            p[start] = -1;
            for (int it = 1; it < G.Vertices; it++)
            {
                int min_dist = inf;
                int min_u = 0;
                for (int i = 1; i <= G.Vertices; i++)
                {
                    if (mark[i] == 0 && pi[i] < min_dist)
                    {
                        min_dist = pi[i];
                        min_u = i;
                    }
                }
                mark[min_u] = 1;
                List<int> list = new List<int>();
                G.findNeighbor(ref list, min_u);
                for (int i = 0; i < list.Count; i++)
                {
                    int y = list[i];
                    if (mark[y] == 0 && pi[y] > pi[min_u] + GA[min_u, y])
                    {
                        pi[y] = GA[min_u, y] + pi[min_u];
                        p[y] = min_u;
                    }
                }
            }
        }

        public int getPiEnd(int end)
        {
            return pi[end];
        }
        public void Print(int end, ref List<int[]> list)
        {
            int path = end;
            while (path != -1)
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
