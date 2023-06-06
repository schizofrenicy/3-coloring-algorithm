using QuikGraph;

namespace _3_coloring_algorithm
{
    abstract class CSP
    {
        private UndirectedGraph<int, SUndirectedEdge<int>> g;
        abstract public int NodeSize { get; }

        public CSP(UndirectedGraph<int, SUndirectedEdge<int>> graph)
        {
            g = new();

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

        public IEnumerable<int> Nodes()
        {
            var vertices = g.Vertices;
            if (vertices.Count() == 0)
            {
                return Enumerable.Empty<int>();
            }

            int vertexMaxIndx = vertices.Max();
            int possibleNodes = (vertexMaxIndx / NodeSize) + 1;

            List<int> toReturn = new();
            for (int i = 0; i < possibleNodes; i++)
            {
                for (int j = 0; j < NodeSize; j++)
                {
                    if (vertices.Contains(i * NodeSize + j))
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

            for (int i = 0; i < NodeSize; i++)
            {
                if (g.Vertices.Contains(node * NodeSize + i))
                {
                    toReturn.Add((ColorEnum)i);
                }
            }

            return toReturn;
        }

        public IEnumerable<(int index, ColorEnum color)> VertexConstraints(int index, ColorEnum color)
        {
            int vertexRealIndex = index * NodeSize + (int)color;

            if (!g.ContainsVertex(vertexRealIndex))
            {
                return Array.Empty<(int index, ColorEnum color)>();
            }

            var neighbors = g.AdjacentVertices(vertexRealIndex);

            List<(int index, ColorEnum color)> toReturn = new();
            foreach (var n in neighbors)
            {
                int neighborIndex = n / NodeSize;
                ColorEnum neighborColor = (ColorEnum)(n - neighborIndex * NodeSize);
                toReturn.Add((neighborIndex, neighborColor));
            }

            return toReturn;
        }

        public bool RemoveConstraint((int index, ColorEnum color) v1, (int index, ColorEnum color) v2)
        {
            int v1RealIndex = v1.index * NodeSize + (int)v1.color;
            int v2RealIndex = v2.index * NodeSize + (int)v2.color;

            return g.RemoveEdge(new SUndirectedEdge<int>(v1RealIndex, v2RealIndex)) || g.RemoveEdge(new SUndirectedEdge<int>(v2RealIndex, v1RealIndex));
        }

        public bool AddConstraint((int index, ColorEnum color) v1, (int index, ColorEnum color) v2)
        {
            int v1RealIndex = v1.index * NodeSize + (int)v1.color;
            int v2RealIndex = v2.index * NodeSize + (int)v2.color;

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
            int vertexRealIndex = index * NodeSize + (int)color;
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
            int v1RealIndex = v1.index * NodeSize + (int)v1.color;
            int v2RealIndex = v2.index * NodeSize + (int)v2.color;

            return g.ContainsEdge(new SUndirectedEdge<int>(v1RealIndex, v2RealIndex)) || g.ContainsEdge(new SUndirectedEdge<int>(v2RealIndex, v1RealIndex));
        }
    }
}
