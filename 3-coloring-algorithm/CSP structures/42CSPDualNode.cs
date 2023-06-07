namespace _3_coloring_algorithm
{
    internal class _42CSPDualNode : ICloneable
    {
        public (int node, ColorEnum? color)[] prevColors;
        public int j;
        public ColorEnum? constraintColor;

        public _42CSPDualNode()
        {
            prevColors = new (int node, ColorEnum? color)[4];
            for (int i = 0; i < prevColors.Length; i++)
            {
                prevColors[i].node = -1;
            }
            j = -1;
            constraintColor = null;
        }

        public object Clone()
        {
            _42CSPDualNode cp = new _42CSPDualNode();
            cp.j = j;
            cp.constraintColor = constraintColor;
            for (int i = 0; i < prevColors.Length; i++)
            {
                cp.prevColors[i].color = prevColors[i].color;
                cp.prevColors[i].node = prevColors[i].node;
            }
            return cp;
        }
    }
}
