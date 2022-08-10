using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lquenti
{
    public class RNGPlot : MonoBehaviour
    {
        private readonly System.Random rnd = new();
        [SerializeField]
        private GameObject lineObj;
        [SerializeField]
        private GameObject barObj;
        [SerializeField]
        private GameObject radialObj;

        private LinePlot linePlot;
        private BarPlot barPlot;
        private RadialPlot radialBar;

        private void Awake()
        {
            linePlot = lineObj.GetComponent<LinePlot>();
            barPlot = barObj.GetComponent<BarPlot>();
            radialBar = radialObj.GetComponent<RadialPlot>();
        }

        private void Start()
        {
            InvokeRepeating(nameof(F), .1f, .1f);
        }

        void F()
        {
            float rng = 100f * (float)rnd.NextDouble();
            Debug.Log(rng);
            linePlot.Add(rng);
            Debug.Log($"barPlot {barPlot}");
            barPlot.Add(rng);
            radialBar.Set((int)rng);
        }
    }

}
