﻿using QuikGraph;

namespace _3_coloring_algorithm
{
    static class FastAlgorithm
    {
        public static bool Fast3Coloring(UndirectedGraph<int, SUndirectedEdge<int>> g, out int[] colorsResult)
        {
            var csp = new _32CSP(g);

            colorsResult = new int[g.VertexCount];
            Array.Fill(colorsResult, -1);

            return _32CSPRecursive(csp, colorsResult);
        }

        static bool _32CSPRecursive(_32CSP csp, int[] colors)
        {
            //if (true) // sprawdzanie kolejnych warunków do reguł redukcyjnych
            //{
            //    // logika usunięcia wierzchołków itp. - usuwmay na csp, nie na kopii
            //    // return Fast3ColoringRecursive(csp, colors) - tutaj działamy na oryginale, zarówno csp, jak i colors
            //}
            //if (true) // kolejny warunek, WAŻNE - IF A NIE ELSE IF
            //{
            //    // jeżeli jest to warunek do reguły branchingu, to robimy nową kopię csp (2 albo więcej)
            //    // na każdej usuwamy to czego nie chcemy (NA KOPIACH)
            //    // wywołujemy rekurencyjnie obie (albo więcej) kopii (colors też jest kopią)
            //    // na koniec jakoś te wyniki łączymy i zwracamy
            //}
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

                return _32CSPRecursive(csp, colors);
            }
            if (Lemma2(csp, out node, out ColorEnum? color, out int node2, out ColorEnum? color2))
            {
                ColorVertices((node, (ColorEnum)color!), csp, colors);
                csp.RemoveNode(node);
                ColorVertices((node2, (ColorEnum)color2!), csp, colors);
                csp.RemoveNode(node2);
                return _32CSPRecursive(csp, colors);
            }
            if (Lemma3(csp, out node, out color))
            {
                csp.RemoveVertex(node, (ColorEnum)color!);
                return _32CSPRecursive(csp, colors);
            }
            if (Lemma4(csp, out node, out color))
            {
                ColorVertices((node, (ColorEnum)color!), csp, colors);
                csp.RemoveNode(node);
                return _32CSPRecursive(csp, colors);
            }
            if (Lemma5(csp, out node, out color))
            {
                csp.RemoveVertex(node, (ColorEnum)color!);
                return _32CSPRecursive(csp, colors);
            }

            _42CSP csp2 = new _42CSP(csp);
            return _42CSPRecursive(csp2, colors);
        }

        static bool _42CSPRecursive(_42CSP csp, int[] colors)
        {
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

                return _42CSPRecursive(csp, colors);
            }
            if (Lemma2(csp, out node, out ColorEnum? color, out int node2, out ColorEnum? color2))
            {
                ColorVertices((node, (ColorEnum)color!), csp, colors);
                csp.RemoveNode(node);
                ColorVertices((node2, (ColorEnum)color2!), csp, colors);
                csp.RemoveNode(node2);
                return _42CSPRecursive(csp, colors);
            }
            if (Lemma3(csp, out node, out color))
            {
                csp.RemoveVertex(node, (ColorEnum)color!);
                return _42CSPRecursive(csp, colors);
            }
            if (Lemma4(csp, out node, out color))
            {
                ColorVertices((node, (ColorEnum)color!), csp, colors);
                csp.RemoveNode(node);
                return _42CSPRecursive(csp, colors);
            }
            if (Lemma5(csp, out node, out color))
            {
                csp.RemoveVertex(node, (ColorEnum)color!);
                return _42CSPRecursive(csp, colors);
            }
            // LEMMA 6
            if (Lemma7(csp, out node, out node2, out color))
            {
                // używamy koloru (w,R)
                var csp1 = (_42CSP)csp.Clone();
                var colors1 = (int[])colors.Clone();
                csp1.RemoveVertex(node, (ColorEnum)color!);
                var node2Neighbors = csp1.VertexConstraints(node2, (ColorEnum)color!);
                foreach (var n in node2Neighbors)
                {
                    csp1.RemoveVertex(n);
                }
                ColorVertices((node2, (ColorEnum)color!), csp1, colors1);
                csp1.RemoveNode(node2);

                var result = _42CSPRecursive(csp1, colors1);
                if (result)
                {
                    RewriteArray(colors, colors1);
                    return true;
                }

                // odrzucamy kolor (w,R)
                // wybieramy (v,R)
                var csp2 = (_42CSP)csp.Clone();
                var colors2 = (int[])colors.Clone();
                csp2.RemoveVertex(node2, (ColorEnum)color!);
                ColorVertices((node, (ColorEnum)color!), csp2, colors2);
                csp2.RemoveNode(node);

                result = _42CSPRecursive(csp2, colors2);
                if (result)
                {
                    RewriteArray(colors, colors2);
                    return true;
                }

                return false;
            }

            return false;
        }


        static bool Lemma1(CSP csp, out int node)
        {
            node = -1;

            var nodes = csp.Nodes();
            foreach (var n in nodes)
            {
                if (csp.NodeColors(n).Count() == 2)
                {
                    node = n;
                    return true;
                }
            }

            return false;
        }

        static bool Lemma2(CSP csp, out int node, out ColorEnum? color1, out int node2, out ColorEnum? color2)
        {
            var nodes = csp.Nodes();
            foreach (var n in nodes)
            {
                var colors = csp.NodeColors(n);
                foreach (var c in colors)
                {
                    var neighbors = csp.VertexConstraints(n, c);

                    if (neighbors.Count() >= csp.NodeSize) continue;
                    if (neighbors.Count() == 0) continue; // will be dealt with in lemma 4

                    int nextNodeIndex = neighbors.ElementAt(0).index;
                    bool stop = false;
                    for (int i = 1; i < neighbors.Count(); i++)
                    {
                        if (neighbors.ElementAt(i).index != nextNodeIndex)
                        {
                            stop = true;
                        }
                    }
                    if (stop) continue;
                    var neighborColors = csp.NodeColors(nextNodeIndex);

                    foreach (var col in neighborColors)
                    {
                        if (csp.IsConstrained((n, c), (nextNodeIndex, col))) continue;

                        var newNeighbors = csp.VertexConstraints(nextNodeIndex, col);

                        if (newNeighbors.Count() >= csp.NodeSize) continue;
                        if (newNeighbors.Count() == 0) continue; // will be dealt with in lemma 4

                        bool cond = newNeighbors.ElementAt(0).index == n && newNeighbors.ElementAt(0).color != c;
                        for (int i = 1; i < newNeighbors.Count(); i++)
                        {
                            cond &= newNeighbors.ElementAt(i).index == n && newNeighbors.ElementAt(i).color != c;
                        }

                        if (cond)
                        {
                            node = n;
                            color1 = c;
                            node2 = nextNodeIndex;
                            color2 = col;
                            return true;

                        }
                    }
                }
            }

            node = -1;
            color1 = null;
            node2 = -1;
            color2 = null;
            return false;
        }

        static bool Lemma3(CSP csp, out int node, out ColorEnum? color)
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
                            if (csp.IsConstrained(c, (n, (ColorEnum)j)))
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

        static bool Lemma4(CSP csp, out int node, out ColorEnum? color)
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

        static bool Lemma5(CSP csp, out int node, out ColorEnum? color)
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
                        bool cond = true;
                        for (int i = 0; i < csp.NodeSize; i++)
                        {
                            cond &= csp.IsConstrained((n, col), (c.index, (ColorEnum)i));
                        }
                        if (cond)
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

        static bool Lemma7(_42CSP csp, out int node, out int node2, out ColorEnum? color)
        {
            var nodes = csp.Nodes();

            foreach (var n in nodes)
            {
                var colors = csp.NodeColors(n);

                foreach (var col in colors)
                {
                    var constraints = csp.VertexConstraints(n, col);
                    if (constraints.Count() != 1) continue;
                    var neighbour = constraints.ElementAt(0);

                    if (neighbour.color != col) continue;

                    var wConstraints = csp.VertexConstraints(neighbour);
                    if (wConstraints.Count() > 1)
                    {
                        node = n;
                        node2 = neighbour.index;
                        color = neighbour.color;
                        return true;
                    }
                }
            }

            node = -1;
            node2 = -1;
            color = null;
            return false;
        }

        static void RewriteArray(int[] arr1, int[] arr2)
        {
            if (arr1.Length != arr2.Length)
            {
                throw new ArgumentException("Arrays should have same length!");
            }
            for (int i = 0; i < arr1.Length; i++)
            {
                arr1[i] = arr2[i];
            }
        }

        static void ColorVertices((int index, ColorEnum color) vertex, CSP csp, int[] colors)
        {
            var result = csp.GetFinalColors(vertex);

            foreach (var r in result)
            {
                if (colors[r.index] != -1)
                {
                    throw new Exception("Trying to color vertex that is already colored!");
                }

                colors[r.index] = (int)r.color;
            }
        }
    }
}
