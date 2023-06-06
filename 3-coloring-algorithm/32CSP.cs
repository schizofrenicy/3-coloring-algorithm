using QuikGraph;

namespace _3_coloring_algorithm
{
    internal class _32CSP : CSP
    {
        public override int NodeSize
        {
            get { return 3; }
        }

        public _32CSP(UndirectedGraph<int, SUndirectedEdge<int>> graph) : base(graph) { }
    }
}
