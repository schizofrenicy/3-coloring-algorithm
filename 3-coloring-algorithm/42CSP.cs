using QuikGraph;

namespace _3_coloring_algorithm
{
    internal class _42CSP : CSP
    {
        public override int NodeSize
        {
            get { return 4; }
        }

        public _42CSP(UndirectedGraph<int, SUndirectedEdge<int>> graph) : base(graph) { }
    }
}
