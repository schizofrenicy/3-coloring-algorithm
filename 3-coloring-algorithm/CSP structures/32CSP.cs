using QuikGraph;

namespace _3_coloring_algorithm
{
    internal class _32CSP : CSP, ICloneable
    {
        public override int NodeSize
        {
            get { return 3; }
        }

        public _32CSP() { }

        public object Clone()
        {
            _32CSP cp = new();
            cp.g = this.g.Clone();
            return cp;
        }

        public _32CSP(UndirectedGraph<int, SUndirectedEdge<int>> graph)
        {
            for (int i = 0; i < graph.VertexCount * NodeSize; i++)
            {
                g.AddVertex(i);
            }

            for (int i = 0; i < graph.VertexCount; i++)
            {
                var vertices = graph.AdjacentVertices(i);

                foreach (var v in vertices)
                {
                    if (i < v)
                    {
                        g.AddEdge(new SUndirectedEdge<int>(i * NodeSize, v * NodeSize));
                        g.AddEdge(new SUndirectedEdge<int>(i * NodeSize + 1, v * NodeSize + 1));
                        g.AddEdge(new SUndirectedEdge<int>(i * NodeSize + 2, v * NodeSize + 2));
                    }
                }
            }
        }

        public override List<(int index, ColorEnum color)> GetFinalColors((int index, ColorEnum color) vertex)
        {
            return new List<(int index, ColorEnum color)>() { vertex };
        }
    }
}
