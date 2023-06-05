using QuikGraph;

namespace _3_coloring_algorithm
{
    internal class _32CSP
    {
        private UndirectedGraph<int, SUndirectedEdge<int>> g;
        static readonly int a = 3;

        public _32CSP(UndirectedGraph<int, SUndirectedEdge<int>> graph)
        {
            g = new();

            for (int i = 0; i < g.VertexCount * a; i++)
            {
                g.AddVertex(i);
            }

            for (int i = 0; i < g.VertexCount; i++)
            {
                var vertices = g.AdjacentVertices(i);

                foreach (var v in vertices)
                {
                    if (i < v)
                    {
                        g.AddEdge(new SUndirectedEdge<int>(i * a, v * a));
                        g.AddEdge(new SUndirectedEdge<int>(i * a + 1, v * a + 1));
                        g.AddEdge(new SUndirectedEdge<int>(i * a + 2, v * a + 2));
                    }
                }
            }
        }
    }
}
