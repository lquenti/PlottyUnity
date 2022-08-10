using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Lquenti
{
    public abstract class AbstractPlot : Graphic
    {
        [SerializeField]
        [Range(1, 100)]
        protected int ticks = 25;

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
            var (verts, indices) = DrawAll(floats.ToList());
            vh.AddUIVertexStream(verts, indices);
        }

        protected abstract (List<UIVertex>, List<int>) DrawAll(List<float> vals);

        public void Add(float x)
        {
            floats.Push(x);
            SetVerticesDirty();
        }
        public void Set(IEnumerable<float> xs)
        {
            floats = new FixedSizeQueue<float>(xs, ticks);
            SetVerticesDirty();
        }
    }
}
