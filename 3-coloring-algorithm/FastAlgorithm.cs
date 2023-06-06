using QuikGraph;
using System.Diagnostics.Contracts;

namespace _3_coloring_algorithm
{
    static class FastAlgorithm
    {
        public static bool Fast3Coloring(UndirectedGraph<int, SUndirectedEdge<int>> g, out int[] colorsResult)
        {
            var csp = new _32CSP(g);

            colorsResult = new int[g.VertexCount];
            Array.Fill(colorsResult, -1);

            return Fast3ColoringRecursive(csp, colorsResult);
        }

        static bool Fast3ColoringRecursive(_32CSP csp, int[] colors)
        {
            if (true) // sprawdzanie kolejnych warunków do reguł redukcyjnych
            {
                // logika usunięcia wierzchołków itp. - usuwmay na csp, nie na kopii
                // return Fast3ColoringRecursive(csp, colors) - tutaj działamy na oryginale, zarówno csp, jak i colors
            }
            if (true) // kolejny warunek, WAŻNE - IF A NIE ELSE IF
            {
                // jeżeli jest to warunek do reguły branchingu, to robimy nową kopię csp (2 albo więcej)
                // na każdej usuwamy to czego nie chcemy (NA KOPIACH)
                // wywołujemy rekurencyjnie obie (albo więcej) kopii (colors też jest kopią)
                // na koniec jakoś te wyniki łączymy i zwracamy
            }
            if (Lemma1(csp, out int node))
            {
                var col = csp.NodeColors(node);
                var constraints1 = csp.VertexConstraints(node, col.ElementAt(0));
                var constraints2 = csp.VertexConstraints(node, col.ElementAt(1));

                foreach (var c1 in constraints1)
                {
                    foreach (var c2 in constraints2)
                    {
                        if (c1.index != c2.index)
                        {
                            csp.AddConstraint(c1, c2);
                        }
                    }
                }

                csp.RemoveNode(node);

                return Fast3ColoringRecursive(csp, colors);
            }
            if (Lemma3(csp, out node, out ColorEnum? color))
            {
                csp.RemoveVertex(node, (ColorEnum)color!);
                return Fast3ColoringRecursive(csp, colors);
            }
            if (Lemma4(csp, out node, out color))
            {
                colors[node] = (int)color!;
                csp.RemoveNode(node);
                return Fast3ColoringRecursive(csp, colors);
            }
            if (Lemma5(csp, out node, out color))
            {
                csp.RemoveVertex(node, (ColorEnum)color!);
                return Fast3ColoringRecursive(csp, colors);
            }
            
            colors = null;
            return false;
        }
      
        static bool Lemma1(_32CSP csp, out int index)
        {
            index = -1;
            var nodes = csp.Nodes();
            foreach(var n in nodes)
            {
                if (csp.NodeColors(n).Count() == 2)
                {
                    index = n;
                    return true;
                }
            }
          
            return false;
        }

        static bool Lemma3(_32CSP csp, out int node, out ColorEnum? color)
        {
            var nodes = csp.Nodes();
            foreach (var n in nodes)
            {
                var colors = csp.NodeColors(n);
                for (int i = 0; i < colors.Count(); i++)
                {
                    var constraints = csp.VertexConstraints(n, (ColorEnum)i);
                    for (int j = i + 1; j < colors.Count(); j++)
                    {
                        foreach (var c in constraints)
                        {
                            if (csp.IsConstrained((c.index, c.color), (n, (ColorEnum)j)))
                            {
                                node = n;
                                color = (ColorEnum)j;
                                return true;
                            }
                        }
                    }
                }
            }

            node = -1;
            color = null;
            return false;
        }

        static bool Lemma4(_32CSP csp, out int node, out ColorEnum? color)
        {
            var nodes = csp.Nodes();
            foreach (var n in nodes)
            {
                var colors = csp.NodeColors(n);
                foreach (var col in colors)
                {
                    var constraints = csp.VertexConstraints(n, col);

                    if (constraints.Count() == 0)
                    {
                        node = n;
                        color = col;
                        return true;
                    }
                }
            }
            node = -1;
            color = null;
            return false;
        }

        static bool Lemma5(_32CSP csp, out int node, out ColorEnum? color)
        {
            var nodes = csp.Nodes();
            foreach (var n in nodes)
            {
                var colors = csp.NodeColors(n);
                foreach (var col in colors)
                {
                    var constraints = csp.VertexConstraints(n, col);

                    foreach (var c in constraints)
                    {
                        if (csp.IsConstrained((n, col), (c.index, ColorEnum.A)) && csp.IsConstrained((n, col), (c.index, ColorEnum.B)) && csp.IsConstrained((n, col), (c.index, ColorEnum.C)))
                        {
                            node = n;
                            color = col;
                            return true;
                        }
                    }
                }
            }

            node = -1;
            color = null;
            return false;
        }
    }
}
