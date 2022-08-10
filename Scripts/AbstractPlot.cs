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
        [SerializeField]
        [Range(1, 100)]
        private int ticks = 25;
        private FixedSizeQueue<float> floats;
        protected Rect canvasRect;

        protected override void Awake()
        {
            base.Awake();
            floats = new FixedSizeQueue<float>(ticks);
            canvasRect = GetComponent<RectTransform>().rect;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            base.OnPopulateMesh(vh);
            vh.Clear();
            if (floats == null || floats.Count == 0)
            {
                // Not awaken yet
                return;
            }
            // TODO: Don't always redraw from zero
            var (verts, indices) = DrawAll(floats.ToList());
            vh.AddUIVertexStream(verts, indices);
        }

        (List<UIVertex>, List<int>) DrawAll(List<float> vals)
        {
            float max = vals.Max();
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

        public void Add(float x)
        {
            floats.Push(x);
            SetVerticesDirty();
        }
        // TODO set
    }
}
