namespace _3_coloring_algorithm
{
    internal class _42CSPDualNode
    {
        public (int node, ColorEnum color)[] prevColors = new (int node, ColorEnum color)[4];
        public int j { get; set; }
        public ColorEnum constrainColor { get; set; }
    }
}
