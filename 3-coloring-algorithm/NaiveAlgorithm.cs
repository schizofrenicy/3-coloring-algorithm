using QuikGraph;

namespace _3_coloring_algorithm
{
    static class NaiveAlgorithm
    {
        static readonly int numOfColors = 3;
        static int[] colors = Array.Empty<int>();

        public static bool Naive3Coloring(UndirectedGraph<int, SUndirectedEdge<int>> g, out int[] colorsResult)
        {
            colors = new int[g.VertexCount];

            if (Naive3ColoringRecursive(g, 0))
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

        static bool Naive3ColoringRecursive(UndirectedGraph<int, SUndirectedEdge<int>> g, int vertex)
        {
            if (vertex == g.VertexCount)
            {
                return ProperColoringChecker.CheckColoring(g, colors);
            }

            for (int color = 0; color < numOfColors; color++)
            {
                colors[vertex] = color;

                if (Naive3ColoringRecursive(g, vertex + 1)) return true;
            }

            return false;
        }
    }
}
