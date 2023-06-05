using QuikGraph;

namespace _3_coloring_algorithm
{
    static class SimpleAlgorithm
    {
        static readonly int numOfColors = 3;
        static int[] colors = Array.Empty<int>();

        public static bool Simple3Coloring(UndirectedGraph<int, SUndirectedEdge<int>> g, out int[] colorsResult)
        {
            colors = new int[g.VertexCount];

            if (Simple3ColoringRecursive(g))
            {
                colorsResult = colors;
                return true;
            }
            else
            {
                colorsResult = Array.Empty<int>();
                return false;
            }
        }

        static bool Simple3ColoringRecursive(UndirectedGraph<int, SUndirectedEdge<int>> g, int vertex = 0)
        {
            if (vertex == g.VertexCount)
            {
                return true;
            }

            for (int color = 0; color < numOfColors; color++)
            {
                if (TryColor(g, vertex, (ColorEnum)color))
                {
                    colors[vertex] = color;
                }
                else
                {
                    continue;
                }

                if (Simple3ColoringRecursive(g, vertex + 1))
                {
                    return true;
                }
            }

            return false;
        }

        static bool TryColor(UndirectedGraph<int, SUndirectedEdge<int>> g, int vertex, ColorEnum color)
        {
            var neighbors = g.AdjacentVertices(vertex);

            foreach (var n in neighbors)
            {
                if (n < vertex && colors[n] == (int)color)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
