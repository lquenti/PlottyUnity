using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// TODO: Do I need to inherit?
namespace Lquenti
{
    public abstract class AbstractPlot : Graphic
    {
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
            List<uint> ints = new List<uint> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var (verts, indices) = DrawAll(ints);
            vh.AddUIVertexStream(verts, indices);
        }

        (List<UIVertex>, List<int>) DrawAll(List<uint> vals)
        {
            uint max = vals.Max();
            int n = vals.Count;
            float step = canvasRect.width / n;

            (List<UIVertex>, List<int>) acc = (new List<UIVertex>(), new List<int>());

            for (int offset = 0; offset < vals.Count; offset++)
            {
                var x = vals[offset];
                // TODO used canvasrect inline
                float height = (float)x / max * canvasRect.height;
                // TODO: Outsource drawBar into draw, use inheritance
                acc = UtilExtensions.joinVertexStreams(acc, Draw(height, step, offset));
            }
            return acc;
        }

        protected abstract (List<UIVertex>, List<int>) Draw(float height, float step, int offset);
    }
}
