using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Expects pivot in lower left
// TODO consistent variable naming
public class MeshPlayground : Graphic
{
    #region Properties
    [Header("Line Settings")]
    [SerializeField]
    [Range(0, 1)]
    private float lineThickness = .05f;

    [Header("Bar Settings")]
    [SerializeField]
    private Color32 baseColor = new Color32(0, 255, 0, 255);
    [SerializeField]
    private Color32 tipColor = new Color32(75, 192, 75, 255);


    private Rect canvasRect;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        canvasRect = GetComponent<RectTransform>().rect;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        base.OnPopulateMesh(vh);
        vh.Clear();
        /* Line Example
        Vector2 from = toLocalSpace(Vector2.one * .2f);
        Vector2 to = toLocalSpace(Vector2.one * .7f);
        var (verts, indizes) = drawLine(from, to);
        vh.AddUIVertexStream(verts, indizes);
        */
        List<uint> ints = new List<uint> { 1,2,3,4,5,6,7,8,9,10 };
        var (verts, indizes) = drawBars(ints);
        vh.AddUIVertexStream(verts, indizes);
    }

    #region Helper
    // Normal interval: [0,1]
    Vector2 toLocalSpace(Vector2 coord)
    {
        return new Vector2(coord.x * canvasRect.width, coord.y * canvasRect.height);
    }

    Vector2 orthogonalCW(Vector2 v)
    {
        return new Vector2(v.y, -v.x);
    }
    Vector2 orthogonalCCW(Vector2 v)
    {
        return new Vector2(-v.y, v.x);
    }
    Vector2 localLineThickness()
    {
        return toLocalSpace(Vector2.one * lineThickness);
    }

    List<UIVertex> vecToUIVertex(List<Vector2> xs)
    {
        return xs.Select(x =>
        {
            UIVertex v = UIVertex.simpleVert;
            v.position = x;
            v.color = baseColor;
            return v;
        }).ToList();
    }

    (List<UIVertex>, List<int>) joinVertexStreams((List<UIVertex>, List<int>) init, (List<UIVertex>, List<int>) last)
    {
        if (init.Item1.Count == 0 && init.Item2.Count == 0) return last;
        int zeroBasedLen = init.Item1.Count;
        init.Item1.AddRange(last.Item1);
        init.Item2.AddRange(last.Item2.Select(x => x + zeroBasedLen));
        return init;
    }
    #endregion

    #region Linelogic
    (List<UIVertex>, List<int>) drawLine(Vector2 from, Vector2 to)
    {
        Vector2 delta = (to - from);
        float len = delta.magnitude;
        Vector2 dir = delta.normalized;
        Vector2 cw = orthogonalCW(dir);
        Vector2 ccw = orthogonalCCW(dir);
        Vector2 mid = from + delta * .5f;
        Vector2 localThickness = this.localLineThickness();

        // init
        UIVertex v1 = UIVertex.simpleVert, v2 = UIVertex.simpleVert, v3 = UIVertex.simpleVert, v4 = UIVertex.simpleVert;

        // we get a 90 degree rotation by taking the orthogonal vector to our
        // normalized direction vector.
        Vector2 pt1 = from + cw * localThickness / 2;
        Vector2 pt2 = from + ccw * localThickness / 2;
        // Now, instead of computing the inverse direction, we can just
        // move the delta from our side points
        Vector3 pt3 = pt1 + delta;
        Vector3 pt4 = pt2 + delta;

        v1.position = pt1;
        v2.position = pt2;
        v3.position = pt3;
        v4.position = pt4;

        List<int> indizes = new List<int>()
        {
            0,1,2,
            1,2,3
        };
        return (new List<UIVertex> { v1,v2,v3,v4 }, indizes);
    }
    #endregion

    #region BarLogic
    (List<UIVertex>, List<int>) drawBars(List<uint> vals)
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
            acc = joinVertexStreams(acc, drawBar(height, step, offset));
        }
        return acc;
    }

    (List<UIVertex>, List<int>) drawBar(float height, float width, int offset)
    {
        float left_bound = offset * width;
        float right_bound = (offset + 1) * width;
        List<Vector2> vals = new List<Vector2>
        {
            new Vector2 (left_bound, 0),
            new Vector2 (right_bound, 0),
            new Vector2 (right_bound, height),
            new Vector2 (left_bound, height)
        };
        List<int> tris = new List<int>
        {
            2, 1, 0,
            0, 3, 2
        };
        return (vecToUIVertex(vals), tris);
        
    }
    #endregion
}
