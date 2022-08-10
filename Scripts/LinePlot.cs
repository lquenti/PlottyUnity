using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Lquenti
{
    public class LinePlot : Graphic
    {
        [Header("Bar Settings")]
        [SerializeField]
        private Color32 color = new Color32(0, 255, 0, 255);

        protected Rect canvasRect;

        protected override void Awake()
        {
            base.Awake();
            canvasRect = GetComponent<RectTransform>().rect;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            base.OnPopulateMesh(vh);
            vh.Clear();
            List<uint> ints = new List<uint> { 1,9,2,8,3,7,4,5,5,6 };
            var (verts, indices) = DrawAll(ints);
            vh.AddUIVertexStream(verts, indices);
        }

        (List<UIVertex>, List<int>) DrawAll(List<uint> vals)
        {
            uint max = vals.Max();
            int n = vals.Count;
            float step = canvasRect.width / n;

            (List<UIVertex>, List<int>) acc = (new List<UIVertex>(), new List<int>());

            if (n == 0)
            {
                throw new ArgumentException("lol");
            }

            // we roll over the points
            // thus, we need to initialize the last point manually
            Vector2 last = new Vector2(0f, (float)vals[0] / max * canvasRect.height);
            Vector2 curr = new Vector2(0f, 0f);
            for (int offset = 1; offset < vals.Count; offset++)
            {
                curr.x = step * offset;
                curr.y = (float)vals[offset] / max * canvasRect.height;
                acc = UtilExtensions.joinVertexStreams(acc, DrawLine(last, curr));
                last = curr;
            }
            return acc;
        }

        // TODO: Thiccness
        (List<UIVertex>, List<int>) DrawLine(Vector2 from, Vector2 to)
        {
            // TODO
            float thickness = 5f;

            Vector2 delta = (to - from);
            float len = delta.magnitude;
            Vector2 dir = delta.normalized;
            Vector2 cw = UtilExtensions.orthogonalCW(dir);
            Vector2 ccw = UtilExtensions.orthogonalCCW(dir);
            Vector2 midPoint = from + delta * .5f;


            List<Vector2> line = new List<Vector2>();
            line.Add(from + cw * thickness / 2);
            line.Add(from + ccw * thickness / 2);
            line.Add(line[0] + delta);
            line.Add(line[1] + delta);

            List<int> indizes = new List<int>()
            {
                0, 1, 2,
                1, 2, 3
            };
            return (UtilExtensions.vecToUIVertex(line, color), indizes);
        }
    }
}