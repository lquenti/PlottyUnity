using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private RadialBar rb;
    // Start is called before the first frame update
    private void Start()
    {
        slider.onValueChanged.AddListener((v) =>
        {
            rb.Set((int)(v));
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
