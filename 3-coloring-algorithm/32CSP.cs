using QuikGraph;

namespace _3_coloring_algorithm
{
    internal class _32CSP
    {
        private UndirectedGraph<int, SUndirectedEdge<int>> g;
        static readonly int a = 3;

        public _32CSP(UndirectedGraph<int, SUndirectedEdge<int>> graph)
        {
            // transform 3-coloring graph problem to 3,2-CSP instance

            g = new();

            for (int i = 0; i < graph.VertexCount * a; i++)
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
                        g.AddEdge(new SUndirectedEdge<int>(i * a, v * a));
                        g.AddEdge(new SUndirectedEdge<int>(i * a + 1, v * a + 1));
                        g.AddEdge(new SUndirectedEdge<int>(i * a + 2, v * a + 2));
                    }
                }
            }
        }

        public IEnumerable<int> Nodes()
        {
            var vertices = g.Vertices;
            int vertexMaxIndx = vertices.Max();
            int possibleNodes = (vertexMaxIndx / a) + 1;

            List<int> toReturn = new();
            for (int i = 0; i < possibleNodes; i++)
            {
                for (int j = 0; j < a; j++)
                {
                    if (vertices.Contains(i * a + j))
                    {
                        toReturn.Add(i);
                        break;
                    }
                }
            }

            return toReturn;
        }

        public int NodesCount()
        {
            return Nodes().Count();
        }

        public IEnumerable<ColorEnum> NodeColors(int node)
        {
            List<ColorEnum> toReturn = new();

            for (int i = 0; i < a; i++)
            {
                if (g.Vertices.Contains(node * a + i))
                {
                    toReturn.Add((ColorEnum)i);
                }
            }

            return toReturn;
        }

        public IEnumerable<(int index, ColorEnum color)> VertexConstraints(int index, ColorEnum color)
        {
            int vertexRealIndex = index * a + (int)color;

            if (!g.ContainsVertex(vertexRealIndex))
            {
                return Array.Empty<(int index, ColorEnum color)>();
            }

            var neighbors = g.AdjacentVertices(vertexRealIndex);

            List<(int index, ColorEnum color)> toReturn = new();
            foreach (var n in neighbors)
            {
                int neighborIndex = n / a;
                ColorEnum neighborColor = (ColorEnum)(n - neighborIndex * a);
                toReturn.Add((neighborIndex, neighborColor));
            }

            return toReturn;
        }

        public bool RemoveConstraint((int index, ColorEnum color) v1, (int index, ColorEnum color) v2)
        {
            int v1RealIndex = v1.index * a + (int)v1.color;
            int v2RealIndex = v2.index * a + (int)v2.color;

            return g.RemoveEdge(new SUndirectedEdge<int>(v1RealIndex, v2RealIndex)) || g.RemoveEdge(new SUndirectedEdge<int>(v2RealIndex, v1RealIndex));
        }

        public bool AddConstraint((int index, ColorEnum color) v1, (int index, ColorEnum color) v2)
        {
            int v1RealIndex = v1.index * a + (int)v1.color;
            int v2RealIndex = v2.index * a + (int)v2.color;

            if (g.ContainsEdge(new SUndirectedEdge<int>(v1RealIndex, v2RealIndex)) || g.ContainsEdge(new SUndirectedEdge<int>(v2RealIndex, v1RealIndex)))
            {
                return false;
            }

            if (v1RealIndex < v2RealIndex)
            {
                return g.AddEdge(new SUndirectedEdge<int>(v1RealIndex, v2RealIndex));
            }
            else if (v1RealIndex > v2RealIndex)
            {
                return g.AddEdge(new SUndirectedEdge<int>(v2RealIndex, v1RealIndex));
            }
            else
            {
                return false;
            }
        }

        public bool RemoveVertex(int index, ColorEnum color)
        {
            int vertexRealIndex = index * a + (int)color;
            var neighbors = VertexConstraints(index, color);

            foreach (var n in neighbors)
            {
                if (!RemoveConstraint((index, color), n))
                {
                    throw new Exception("Unable to remove edge!");
                }
            }

            return g.RemoveVertex(vertexRealIndex);
        }

        public IEnumerable<((int index, ColorEnum color) v1, (int index, ColorEnum color) v2)> Constraints()
        {
            List<((int index, ColorEnum color) v1, (int index, ColorEnum color) v2)> toReturn = new();

            var nodes = Nodes();
            foreach (var no in nodes)
            {
                var colors = NodeColors(no);
                foreach (var c in colors)
                {
                    var neighbors = VertexConstraints(no, c);
                    foreach (var n in neighbors)
                    {
                        if (no < n.index)
                        {
                            toReturn.Add(((no, c), n));
                        }
                    }
                }
            }

            return toReturn;
        }

        public bool RemoveNode(int index)
        {
            var colors = NodeColors(index);

            foreach (var c in colors)
            {
                if (!RemoveVertex(index, c))
                {
                    throw new Exception("Unable to remove vertex");
                }
            }

            return true;
        }

        public bool IsConstrained((int index, ColorEnum color) v1, (int index, ColorEnum color) v2)
        {
            int v1RealIndex = v1.index * a + (int)v1.color;
            int v2RealIndex = v2.index * a + (int)v2.color;

            return g.ContainsEdge(new SUndirectedEdge<int>(v1RealIndex, v2RealIndex)) || g.ContainsEdge(new SUndirectedEdge<int>(v2RealIndex, v1RealIndex));
        }
    }
}
