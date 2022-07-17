using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshPlayground : Graphic
{
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        base.OnPopulateMesh(vh);
        
        UIVertex v = UIVertex.simpleVert;
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 space = new Vector2(rt.rect.width, rt.rect.height);



        v.position = new Vector2(.5f, .5f) * space;
        vh.AddVert(v);
        v.position = new Vector2(-.5f,.5f) * space;
        vh.AddVert(v);
        v.position = new Vector2(-.5f, -.5f) * space;
        vh.AddVert(v);
        v.position = new Vector2(.5f, -.5f) * space;
        vh.AddVert(v);

        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
    }
}
