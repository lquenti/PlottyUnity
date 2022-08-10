using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lquenti
{
    public class BarPlot : AbstractPlot
    {
        [Header("Bar Settings")]
        [SerializeField]
        private Color32 baseColor = new Color32(0, 255, 0, 255);
        [SerializeField]
        private Color32 tipColor = new Color32(75, 192, 75, 255);
        [SerializeField]
        [Range(0, 1)]
        // percentage of maximum
        private float tipPercentage = .05f;
        protected override (List<UIVertex>, List<int>) Draw(float height, float width, int offset)
        {
            float left_bound = offset * width;
            float right_bound = (offset + 1) * width;

            // TODO: Optimize double vectors
            List<Vector2> bottom = new List<Vector2>
        {
            new Vector2 (left_bound, 0),
            new Vector2 (right_bound, 0),
            new Vector2 (right_bound, height * (1-tipPercentage)),
            new Vector2 (left_bound, height * (1-tipPercentage))
        };
            List<Vector2> tip = new List<Vector2>
        {
            new Vector2 (left_bound, height * (1-tipPercentage)),
            new Vector2 (right_bound, height * (1 - tipPercentage)),
            new Vector2 (right_bound, height),
            new Vector2 (left_bound, height)
        };
            List<int> bottomTris = new List<int>
        {
            2, 1, 0,
            0, 3, 2
        };
            // TODO double
            List<int> tipTris = new List<int>
        {
            2, 1, 0,
            0, 3, 2
        };

            return UtilExtensions.joinVertexStreams(
                (UtilExtensions.vecToUIVertex(bottom, baseColor), bottomTris),
                (UtilExtensions.vecToUIVertex(tip, tipColor), tipTris)
            );

        }
    }
}
