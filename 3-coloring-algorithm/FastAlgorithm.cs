using QuikGraph;

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

            if (Lemma4(csp, out int node, out ColorEnum? color))
            {
                colors[node] = (int)color!;
                csp.RemoveNode(node);
                return Fast3ColoringRecursive(csp, colors);
            }

            colors = null;
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
                    var contraints = csp.VertexConstraints(n, col);

                    if (contraints.Count() == 0)
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
    }
}
