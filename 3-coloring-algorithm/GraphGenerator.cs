using QuikGraph;

namespace _3_coloring_algorithm
{
    static class GraphGenerator
    {
        static Random rnd = new();

        public static UndirectedGraph<int, SUndirectedEdge<int>> Generate(int size, double percentage)
        {
            UndirectedGraph<int, SUndirectedEdge<int>> result = new();

            for (int i = 0; i < size; i++)
            {
                result.AddVertex(i);
            }

            List<SUndirectedEdge<int>> possibleEdges = new();

            for (int i = 0; i < size; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    possibleEdges.Add(new SUndirectedEdge<int>(i, j));
                }
            }

            var edgesSize = percentage * possibleEdges.Count;

            for (int i = 0; i < edgesSize; i++)
            {
                int idx = rnd.Next(possibleEdges.Count);
                var edge = possibleEdges[idx];
                possibleEdges.RemoveAt(idx);
                result.AddEdge(edge);
            }

            return result;
        }
    }
}
