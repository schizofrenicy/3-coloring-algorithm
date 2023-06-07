namespace _3_coloring_algorithm
{
    internal class _42CSP : CSP
    {
        private _42CSPDualNode[] dualNodeInfo;

        public override int NodeSize
        {
            get { return 4; }
        }

        public _42CSP(_32CSP csp) 
        {
            var nodes = csp.Nodes();
            dualNodeInfo = new _42CSPDualNode[nodes.Count()];

            foreach (var n in nodes)
            {
                for (int i = 0; i < 3; i++)
                {
                    g.AddVertex(n * NodeSize + i);
                }
            }

            foreach (var n in nodes)
            {
                var colors = csp.NodeColors(n);

                foreach (var c in colors)
                {
                    var neighbors = csp.VertexConstraints(n, c);

                    foreach (var ne in neighbors)
                    {
                        AddConstraint((n, c), ne);
                    }
                }
            }
        }

        public bool IsDualNode(int node)
        {
            return dualNodeInfo[node] != null;
        }

        public bool MergeIntoDualNode(int node1, int node2, ColorEnum color)
        {
            if (IsDualNode(node1) || IsDualNode(node2))
            {
                return false;
            }

            var colors1 = NodeColors(node1);
            var colors2 = NodeColors(node2);

            if (colors1.Count() != 3 || colors2.Count() != 3)
            {
                throw new Exception("It was not supposed to happen :(");
            }

            dualNodeInfo[node1].j = node2;
            dualNodeInfo[node1].color = color;

            List<ColorEnum> possibleColors = new List<ColorEnum>() { ColorEnum.A, ColorEnum.B, ColorEnum.C, ColorEnum.D };

            foreach (var c1 in colors1)
            {
                if (c1 == color) continue;

                possibleColors.Remove(c1);
                dualNodeInfo[node1].prevColors[(int)c1].node = node1;
                dualNodeInfo[node1].prevColors[(int)c1].color = c1;
            }

            foreach(var c2 in colors2)
            {
                var freeColor = possibleColors.ElementAt(0);
                possibleColors.Remove(freeColor);

                var neighbors = VertexConstraints(node2, c2);
                foreach (var n in neighbors)
                {
                    if(!AddConstraint((node1, freeColor), n))
                    {
                        throw new Exception("It was not supposed to happen :(");
                    }
                }

                dualNodeInfo[node1].prevColors[(int)c2].node = node2;
                dualNodeInfo[node1].prevColors[(int)c2].color = freeColor;
            }

            if (possibleColors.Count() != 0)
            {
                throw new Exception("It was not supposed to happen :(");
            }

            return RemoveNode(node2);
        }

        public IEnumerable<(int i, int j)> TrueNodes()
        {
            List<(int i, int j)> toReturn = new();
            var nodes = Nodes();

            foreach (var n in nodes)
            {
                if (IsDualNode(n))
                {
                    toReturn.Add((n, dualNodeInfo[n].j));
                }
                else
                {
                    toReturn.Add((n, -1));
                }
            }

            return toReturn;
        }
    }
}
