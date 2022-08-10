using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lquenti
{
    public static class UtilExtensions
    {
        // from unit space
        public static Vector2 ToLocalSpace(Vector2 coord, float width, float height)
        {
            return new Vector2(coord.x * width, coord.y * height);
        }

        public static Vector2 OrthogonalCW(Vector2 v)
        {
            return new Vector2(v.y, -v.x);
        }
        public static Vector2 OrthogonalCCW(Vector2 v)
        {
            return new Vector2(-v.y, v.x);
        }

        public static List<UIVertex> VecToUIVertex(List<Vector2> xs, Color32 color)
        {
            return xs.Select(x =>
            {
                UIVertex v = UIVertex.simpleVert;
                v.position = x;
                v.color = color;
                return v;
            }).ToList();
        }

        public static (List<UIVertex>, List<int>) JoinVertexStreams((List<UIVertex>, List<int>) init, (List<UIVertex>, List<int>) last)
        {
            if (init.Item1.Count == 0 && init.Item2.Count == 0) return last;
            int zeroBasedLen = init.Item1.Count;
            init.Item1.AddRange(last.Item1);
            init.Item2.AddRange(last.Item2.Select(x => x + zeroBasedLen));
            return init;
        }
    }
}

