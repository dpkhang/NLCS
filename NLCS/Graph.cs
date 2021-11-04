using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace NLCS
{
    class Graph
    {
        private int vertices;
        private int[,] G;

        public int Vertices
        {
            set { this.vertices = value; }
            get { return this.vertices; }
        }

        public int[,] getGraph()
        {
            return this.G;
        }
        public Graph(int vertices)
        {
            this.vertices = vertices;
            this.G = new int[this.vertices + 1, this.vertices + 1];
            for (int i = 1; i <= this.vertices; i++)
                for(int j = 1; j <= this.vertices; j++)
                {
                    this.G[i,j] = 0;
                }
        }

        public void addEdge(int vertice1, int vertice2, int weigh)
        {
            this.G[vertice1, vertice2] = weigh;
            this.G[vertice2, vertice1] = weigh;
        }

        public void findNeighbor(ref List<int> list,int root)
        {
            list = new List<int>();
            for(int i = 1; i <= this.vertices; i++)
            {
                if(this.G[root, i]  != 0)
                {
                    list.Add(i);
                }
            }
        }

        public void printGraph()
        {
            string str = "";
            for(int i = 1; i <= this.vertices; i++)
            {
                for (int j = 1; j <= this.vertices; j++)
                {
                    str = str + this.G[i, j] + " ";
                }
                str = str + "\n";
            }
            MessageBox.Show(str);
        }
    }
}
