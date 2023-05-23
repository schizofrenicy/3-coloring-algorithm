using QuikGraph;

namespace _3_coloring_algorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UndirectedGraph<int, SUndirectedEdge<int>> g = new UndirectedGraph<int, SUndirectedEdge<int>>();
            for (int i = 0; i < 10; i++)
            {
                g.AddVertex(i);
            }

            g.AddEdge(new SUndirectedEdge<int>(0, 1));
            g.AddEdge(new SUndirectedEdge<int>(0, 4));
            g.AddEdge(new SUndirectedEdge<int>(0, 5));
            g.AddEdge(new SUndirectedEdge<int>(1, 2));
            g.AddEdge(new SUndirectedEdge<int>(1, 6));
            g.AddEdge(new SUndirectedEdge<int>(2, 3));
            g.AddEdge(new SUndirectedEdge<int>(2, 7));
            g.AddEdge(new SUndirectedEdge<int>(3, 4));
            g.AddEdge(new SUndirectedEdge<int>(3, 8));
            g.AddEdge(new SUndirectedEdge<int>(4, 9));
            g.AddEdge(new SUndirectedEdge<int>(5, 7));
            g.AddEdge(new SUndirectedEdge<int>(5, 8));
            g.AddEdge(new SUndirectedEdge<int>(6, 8));
            g.AddEdge(new SUndirectedEdge<int>(6, 9));
            g.AddEdge(new SUndirectedEdge<int>(7, 9));
            //g.AddEdge(new SUndirectedEdge<int>(0, 2));
            //g.AddEdge(new SUndirectedEdge<int>(0, 3));
            //g.AddEdge(new SUndirectedEdge<int>(1, 3));

            var result = NaiveAlgorithm.Naive3Coloring(g, out int[] colors);
            Console.WriteLine(result);

            if (result)
            {
                for (int i = 0; i < g.VertexCount; i++)
                {
                    Console.WriteLine($"Vertex number: {i}, color number: {colors[i]}");
                }
            }
        }
    }
}