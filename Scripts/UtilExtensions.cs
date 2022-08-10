using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO: use extension methods everywhere
namespace Lquenti
{
    public static class UtilExtensions
    {
        // from unit space
        public static Vector2 toLocalSpace(Vector2 coord, float width, float height)
        {
            return new Vector2(coord.x * width, coord.y * height);
        }

        public static Vector2 orthogonalCW(Vector2 v)
        {
            return new Vector2(v.y, -v.x);
        }
        public static Vector2 orthogonalCCW(Vector2 v)
        {
            return new Vector2(-v.y, v.x);
        }

        public static List<UIVertex> vecToUIVertex(List<Vector2> xs, Color32 color)
        {
            return xs.Select(x =>
            {
                UIVertex v = UIVertex.simpleVert;
                v.position = x;
                v.color = color;
                return v;
            }).ToList();
        }

        public static (List<UIVertex>, List<int>) joinVertexStreams((List<UIVertex>, List<int>) init, (List<UIVertex>, List<int>) last)
        {
            if (init.Item1.Count == 0 && init.Item2.Count == 0) return last;
            int zeroBasedLen = init.Item1.Count;
            init.Item1.AddRange(last.Item1);
            init.Item2.AddRange(last.Item2.Select(x => x + zeroBasedLen));
            return init;
        }
    }
}

