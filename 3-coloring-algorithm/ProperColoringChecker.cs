using QuikGraph;

namespace _3_coloring_algorithm
{
    static class ProperColoringChecker
    {
        public static bool CheckColoring(UndirectedGraph<int, SUndirectedEdge<int>> g, int[] colors)
        {
            for (int vertex = 0; vertex < g.VertexCount; vertex++)
            {
                var neighbors = g.AdjacentVertices(vertex);

                foreach (var n in neighbors)
                {
                    if (colors[n] == colors[vertex]) return false;
                }
            }

            return true;
        }
    }
}
