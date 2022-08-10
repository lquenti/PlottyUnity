using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Lquenti
{
    public class LinePlot : AbstractPlot
    {
        [Header("Bar Settings")]
        [SerializeField]
        private Color32 lineColor = new(0, 255, 0, 255);

        protected override (List<UIVertex>, List<int>) DrawAll(List<float> vals)
        {
            float max = vals.Max();
            float step = canvasRect.width / ticks;

            (List<UIVertex>, List<int>) acc = (new List<UIVertex>(), new List<int>());

            // we roll over the points
            // thus, we need to initialize the last point manually
            Vector2 last = new(0f, (float)vals[0] / max * canvasRect.height);
            Vector2 curr = new(0f, 0f);
            for (int offset = 1; offset < vals.Count; offset++)
            {
                curr.x = step * offset;
                curr.y = (float)vals[offset] / max * canvasRect.height;
                acc = UtilExtensions.JoinVertexStreams(acc, DrawLine(last, curr));
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
            Vector2 dir = delta.normalized;
            Vector2 cw = UtilExtensions.OrthogonalCW(dir);
            Vector2 ccw = UtilExtensions.OrthogonalCCW(dir);


            List<Vector2> line = new()
            {
                from + cw * thickness / 2,
                from + ccw * thickness / 2
            };
            line.Add(line[0] + delta);
            line.Add(line[1] + delta);

            List<int> indizes = new()
            {
                0, 1, 2,
                1, 2, 3
            };
            return (UtilExtensions.VecToUIVertex(line, lineColor), indizes);
        }
    }
}