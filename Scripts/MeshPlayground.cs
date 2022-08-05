using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Expects pivot in lower left
public class MeshPlayground : Graphic
{

    [SerializeField]
    [Range(0, 1)]
    private float thickness = .05f;

    private Rect canvasRect;

    protected override void Awake()
    {
        base.Awake();
        canvasRect = GetComponent<RectTransform>().rect;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        base.OnPopulateMesh(vh);
        vh.Clear();

        Vector2 from = toLocalSpace(Vector2.one * .2f);
        Vector2 to = toLocalSpace(Vector2.one * .7f);

        List<Vector2> pts = new List<Vector2>
        {
            new Vector2(0f, 0f),
            new Vector2(canvasRect.width, 0f),
            new Vector2(0, canvasRect.height),
            new Vector2(canvasRect.width, canvasRect.height)
        };

        var (verts, indizes) = drawLine(from, to);
        vh.AddUIVertexStream(verts, indizes);
    }

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
    Vector2 localThickness()
    {
        return toLocalSpace(Vector2.one * thickness);
    }



    (List<UIVertex>, List<int>) drawLine(Vector2 from, Vector2 to)
    {
        Vector2 delta = (to - from);
        float len = delta.magnitude;
        Vector2 dir = delta.normalized;
        Vector2 cw = orthogonalCW(dir);
        Vector2 ccw = orthogonalCCW(dir);
        Vector2 mid = from + delta * .5f;
        Vector2 localThickness = this.localThickness();

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
}
