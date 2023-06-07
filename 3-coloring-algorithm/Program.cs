namespace _3_coloring_algorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int size = 15;
            double percentage = 10;

            var g = GraphGenerator.GenerateConnected(size, percentage);
            NaiveAlgorithm.Naive3Coloring(g!, out int[] colors);
            SimpleAlgorithm.Simple3Coloring(g!, out colors);
            for (int i = 0; i < 19; i++)
            {
                g = GraphGenerator.GenerateConnected(size, percentage);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var success = NaiveAlgorithm.Naive3Coloring(g!, out colors);
                watch.Stop();
                if (success)
                {
                    Console.WriteLine($"{size},{percentage},SUCC,NAIVE,{watch.Elapsed.TotalSeconds}");
                }
                else
                {
                    Console.WriteLine($"{size},{percentage},FAIL,NAIVE,{watch.Elapsed.TotalSeconds}");
                }

                watch = System.Diagnostics.Stopwatch.StartNew();
                success = SimpleAlgorithm.Simple3Coloring(g!, out colors);
                watch.Stop();
                if (success)
                {
                    Console.WriteLine($"{size},{percentage},SUCC,SIMPLE,{watch.Elapsed.TotalSeconds}");
                }
                else
                {
                    Console.WriteLine($"{size},{percentage},FAIL,SIMPLE,{watch.Elapsed.TotalSeconds}");
                }

            }
        }
    }
}