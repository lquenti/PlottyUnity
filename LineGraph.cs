using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// TODO handle negative values

public class LineGraph : MonoBehaviour
{
    [SerializeField] public Sprite circleSprite;

    [SerializeField] public float leftPadding = 0.0f;
    [SerializeField] public float rightPadding = 0.0f;
    [SerializeField] public float topPadding = 0.0f;
    [SerializeField] public float bottomPadding = 0.0f;

    [SerializeField] public float relativeRadius = 11f;
    


    private RectTransform rect;
    


    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        List<float> xs = new List<float>() { 95, 20, 32, 52, 19, 3, 80, 57, 0};
        ShowGraph(xs);
    }

    private GameObject CreateCircle(Vector2 anchoredPos)
    {
        GameObject c = new GameObject("circle", typeof(Image));
        c.transform.SetParent(rect, false);
        Debug.Log(circleSprite);
        c.GetComponent<Image>().sprite = circleSprite;
        Debug.Log("after");
        RectTransform rt = c.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = new Vector2(relativeRadius, relativeRadius); // TODO: Play around to understand
        rt.anchorMin = new Vector2(0,0);
        rt.anchorMax = new Vector2(0,0);
        return c;
    }

    private void ShowGraph(List<float> xs)
    {
        float yMax = xs.Max();

        float maxHeight = rect.sizeDelta.y;
        float maxWidth = rect.sizeDelta.x;

        // Step size: (width-xPadding)/(n-1)
        // (n-1) because we start at 0 and want to end at index (n-1)
        float stepSize = (maxWidth - leftPadding - rightPadding) / (xs.Count-1);

        // Generate Circles
        for (int i = 0; i < xs.Count; i++)
        {
            // Calculate Positions
            // This is based on our Anchoring (bottom left assumed)
            // x := (i * stepSize) + leftPadding
            // y := bottomPadding + (graphHeight * ratio)
            //    = bottomPadding + graphHeight * (xs[i]/yMax)
            float xPos = (i * stepSize) + leftPadding;
            float yPos = bottomPadding + maxHeight * (xs[i] / yMax);
            Debug.Log(new Vector2(xPos, yPos));
            GameObject circleObj = CreateCircle(new Vector2(xPos, yPos));
        }
        Debug.Log(maxWidth);
    }
}
