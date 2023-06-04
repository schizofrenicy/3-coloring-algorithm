using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_coloring_algorithm
{
    internal class _32CSP
    {
        private UndirectedGraph<int, SUndirectedEdge<int>> g;
        static readonly int A = 3;

        public _32CSP(UndirectedGraph<int, SUndirectedEdge<int>> graph)
        {
            g = new();

            for (int i = 0; i < g.VertexCount * A; i++)
                g.AddVertex(i);

            for (int i = 0; i < g.VertexCount; i++)
            {
                var vertices = g.AdjacentVertices(i);

                foreach (var v in vertices)
                    if (i < v)
                    {
                        g.AddEdge(new SUndirectedEdge<int>(i * A, v * A));
                        g.AddEdge(new SUndirectedEdge<int>(i * A + 1, v * A + 1));
                        g.AddEdge(new SUndirectedEdge<int>(i * A + 2, v * A + 2));
                    }
            }
        }
    }
}
