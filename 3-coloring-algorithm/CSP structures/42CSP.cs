namespace _3_coloring_algorithm
{
    internal class _42CSP : CSP, ICloneable
    {

        private _42CSPDualNode[] dualNodeInfo = Array.Empty<_42CSPDualNode>();

        public override int NodeSize
        {
            get { return 4; }
        }

        public _42CSP() { }

        public object Clone()
        {
            _42CSP cp = new();
            cp.g = this.g.Clone();
            cp.dualNodeInfo = new _42CSPDualNode[dualNodeInfo.Length];
            for (int i = 0; i < dualNodeInfo.Length; i++)
            {
                cp.dualNodeInfo[i] = (_42CSPDualNode)dualNodeInfo[i].Clone();
            }
            return cp;
        }

        public _42CSP(_32CSP csp)
        {
            var nodes = csp.Nodes();

            dualNodeInfo = new _42CSPDualNode[nodes.Count()];
            for (int i = 0; i < dualNodeInfo.Length; i++)
            {
                dualNodeInfo[i] = new _42CSPDualNode();
            }

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

        public bool AddVertex(int index, ColorEnum color)
        {
            int vertexRealIndex = index * NodeSize + (int)color;

            return g.AddVertex(vertexRealIndex);
        }

        public bool IsDualNode(int node)
        {
            return dualNodeInfo[node].constraintColor != null;
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
                throw new Exception("Nodes in MergeIntoDualNode don't have 3 colors available!");
            }

            dualNodeInfo[node1].j = node2;
            dualNodeInfo[node1].constraintColor = color;

            List<ColorEnum> possibleColors = new List<ColorEnum>() { ColorEnum.A, ColorEnum.B, ColorEnum.C, ColorEnum.D };

            if (!AddVertex(node1, ColorEnum.D))
            {
                throw new Exception("Unable to create forth vertex in 32CSP node!");
            }

            foreach (var c1 in colors1)
            {
                if (c1 == color) continue;

                possibleColors.Remove(c1);
                dualNodeInfo[node1].prevColors[(int)c1].node = node1;
                dualNodeInfo[node1].prevColors[(int)c1].color = c1;
            }

            foreach (var c2 in colors2)
            {
                if (c2 == color) continue;

                var freeColor = possibleColors.ElementAt(0);
                possibleColors.Remove(freeColor);

                var neighbors = VertexConstraints(node2, c2);
                foreach (var n in neighbors)
                {
                    if (!AddConstraint((node1, freeColor), n))
                    {
                        throw new Exception("Unable to add constraint from node2 to node1!");
                    }
                }

                dualNodeInfo[node1].prevColors[(int)freeColor].node = node2;
                dualNodeInfo[node1].prevColors[(int)freeColor].color = c2;
            }

            if (possibleColors.Count() != 0)
            {
                throw new Exception("After executing all the logic, there is still a possible color to be used!");
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

        public override List<(int index, ColorEnum color)> GetFinalColors((int index, ColorEnum color) vertex)
        {
            if (!IsDualNode(vertex.index))
            {
                return new List<(int index, ColorEnum color)>() { vertex };
            }

            var dualNode = dualNodeInfo[vertex.index];

            List<(int index, ColorEnum color)> colors = new()
            {
                ((int,ColorEnum))dualNode.prevColors[(int)vertex.color]!
            };

            if (colors.ElementAt(0).index == vertex.index)
            {
                colors.Add((dualNode.j, (ColorEnum)dualNode.constraintColor!));
            }
            else
            {
                colors.Add((vertex.index, (ColorEnum)dualNode.constraintColor!));
            }

            return colors;
        }
    }
}
