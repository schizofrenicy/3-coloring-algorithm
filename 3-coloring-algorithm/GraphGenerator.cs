using QuikGraph;

namespace _3_coloring_algorithm
{
    static class GraphGenerator
    {
        static Random rnd = new();

        public static UndirectedGraph<int, SUndirectedEdge<int>>? GenerateRandom(int size, double percentage)
        {
            UndirectedGraph<int, SUndirectedEdge<int>> result = new();

            if (size < 0 || size > 10000)
            {
                Console.WriteLine("Value of size must be between 0 and 10000!");
                return null;
            }
            if (percentage < 0 || percentage > 100)
            {
                Console.WriteLine("Value of percentage must be between 0 and 100!");
                return null;
            }

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

            var edgesSize = percentage / 100.0 * possibleEdges.Count;

            for (int i = 0; i < edgesSize; i++)
            {
                int idx = rnd.Next(possibleEdges.Count);
                var edge = possibleEdges[idx];
                possibleEdges.RemoveAt(idx);
                result.AddEdge(edge);
            }

            return result;
        }

        public static UndirectedGraph<int, SUndirectedEdge<int>>? GenerateConnected(int size, double percentage)
        {
            UndirectedGraph<int, SUndirectedEdge<int>> result = new();

            if (size < 0 || size > 10000)
            {
                Console.WriteLine("Value of size must be between 0 and 10000!");
                return null;
            }
            if (percentage < 0 || percentage > 100)
            {
                Console.WriteLine("Value of percentage must be between 0 and 100!");
                return null;
            }

            List<int> allVertices = new();
            List<int> usedVertices = new();

            for (int i = 0; i < size; i++)
            {
                result.AddVertex(i);
                allVertices.Add(i);
            }

            List<SUndirectedEdge<int>> possibleEdges = new();

            for (int i = 0; i < size; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    possibleEdges.Add(new SUndirectedEdge<int>(i, j));
                }
            }

            int idx = rnd.Next(allVertices.Count);
            int vertex = allVertices[idx];
            allVertices.RemoveAt(idx);
            usedVertices.Add(vertex);

            for (int i = 0; i < size - 1; i++)
            {
                idx = rnd.Next(allVertices.Count);
                vertex = allVertices[idx];
                allVertices.RemoveAt(idx);
                idx = rnd.Next(usedVertices.Count);
                int secondVertex = usedVertices[idx];
                usedVertices.Add(vertex);

                var edge = vertex > secondVertex ? new SUndirectedEdge<int>(secondVertex, vertex) : new SUndirectedEdge<int>(vertex, secondVertex);
                possibleEdges.Remove(edge);
                result.AddEdge(edge);
            }

            var edgesSize = percentage / 100.0 * possibleEdges.Count;
            for (int i = 0; i < edgesSize; i++)
            {
                idx = rnd.Next(possibleEdges.Count);
                var edge = possibleEdges[idx];
                possibleEdges.RemoveAt(idx);
                result.AddEdge(edge);
            }

            return result;
        }
    }
}
