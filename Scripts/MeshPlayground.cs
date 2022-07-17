using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// TODO: SET PIVOT TO (0,0) for sane meshes
public class MeshPlayground : Graphic
{
    
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        base.OnPopulateMesh(vh);
        vh.Clear();
        UIVertex v = UIVertex.simpleVert;
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 space = new Vector2(rt.rect.width, rt.rect.height);

        List<Vector2> pts = new List<Vector2>
        {
            new Vector2(0f, 0f),
            new Vector2(space.x, 0f),
            new Vector2(0, space.y),
            new Vector2(space.x, space.y)
        };

        var verts = pts.Select(v => { var x = UIVertex.simpleVert; x.position = v; return x; }).ToList();
        vh.AddUIVertexStream(verts, new List<int>(new int[] { 0, 1, 2, 1, 3, 2 }));
        }
}
