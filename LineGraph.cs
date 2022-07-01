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
    [SerializeField] public float lineThickness = 3f;

    private static readonly Vector4 lineColor = new Vector4(0, 0, 0, .5f);


    private RectTransform rect;
    List<(Vector2, GameObject)> circles;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        circles = new List<(Vector2, GameObject)>();

        List<float> xs = new List<float>() { 95, 20, 32, 52, 19, 3, 80, 57, 0};
        ShowGraph(xs);
    }

    private GameObject CreateCircle(Vector2 anchoredPos)
    {
        GameObject c = new GameObject("circle", typeof(Image));
        c.transform.SetParent(rect, false);
        c.GetComponent<Image>().sprite = circleSprite;
        RectTransform rt = c.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = new Vector2(relativeRadius, relativeRadius);
        rt.anchorMin = new Vector2(0,0);
        rt.anchorMax = new Vector2(0,0);
        return c;
    }


    private GameObject CreateLine(Vector2 from, Vector2 to)
    {
        GameObject line = new GameObject("line", typeof(Image));
        line.transform.SetParent(rect, false);
        line.GetComponent<Image>().color = lineColor;
        RectTransform rt = line.GetComponent<RectTransform>();

        Vector2 direction = (to - from).normalized;
        float length = Vector2.Distance(to, from);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(0, 0);
        rt.sizeDelta = new Vector2(length, lineThickness);

        // Calculate Position
        // If we think about vectors, we would want
        // startingpoint + length * direction
        // but since our rectangle measures its position by its center,
        // we just need half of the length, thus
        rt.anchoredPosition = from + direction * (length * .5f);

        // Now we have to rotate it in order to connect both points
        rt.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        return line;
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
            Vector2 pos = new Vector2(
                (i * stepSize) + leftPadding,
                bottomPadding + maxHeight * (xs[i] / yMax)
            );
            circles.Add((pos, CreateCircle(pos)));

            // Generate line from last to current one
            if (i != 0)
            {
                CreateLine(circles[i - 1].Item1, circles[i].Item1);
            }
        }
    }
}
