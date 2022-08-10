using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lquenti
{
    public class BarPlot : AbstractPlot
    {
        [Header("Bar Settings")]
        [SerializeField]
        private Color32 baseColor = new(0, 255, 0, 255);
        [SerializeField]
        private Color32 tipColor = new(75, 192, 75, 255);
        [SerializeField]
        [Range(0, 1)]
        // percentage of maximum
        private float tipPercentage = .05f;
        protected override (List<UIVertex>, List<int>) DrawAll(List<float> vals)
        {
            float max = vals.Max();
            int n = vals.Count;
            float step = canvasRect.width / n;

            (List<UIVertex>, List<int>) acc = (new List<UIVertex>(), new List<int>());

            for (int offset = 0; offset < vals.Count; offset++)
            {
                var x = vals[offset];
                float height = (float)x / max * canvasRect.height;
                acc = UtilExtensions.JoinVertexStreams(acc, Draw(height, step, offset));
            }
            return acc;
        }
        protected (List<UIVertex>, List<int>) Draw(float height, float width, int offset)
        {
            float left_bound = offset * width;
            float right_bound = (offset + 1) * width;

            // TODO: Optimize double vectors
            List<Vector2> bottom = new()
            {
            new Vector2 (left_bound, 0),
            new Vector2 (right_bound, 0),
            new Vector2 (right_bound, height * (1-tipPercentage)),
            new Vector2 (left_bound, height * (1-tipPercentage))
            };
            List<Vector2> tip = new()
            {
            new Vector2 (left_bound, height * (1-tipPercentage)),
            new Vector2 (right_bound, height * (1 - tipPercentage)),
            new Vector2 (right_bound, height),
            new Vector2 (left_bound, height)
            };
            List<int> bottomTris = new()
            {
            2, 1, 0,
            0, 3, 2
            };
            List<int> tipTris = new()
            {
            2, 1, 0,
            0, 3, 2
            };

            return UtilExtensions.JoinVertexStreams(
                (UtilExtensions.VecToUIVertex(bottom, baseColor), bottomTris),
                (UtilExtensions.VecToUIVertex(tip, tipColor), tipTris)
            );
        }
    }
}
