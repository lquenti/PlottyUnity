using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO: Use namepsace
namespace Lquenti
{
    public class RadialPlot : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField]
        [Min(0)]
        private int minVal = 0;
        [SerializeField]
        [Min(0)]
        private int maxVal = 100;
        [SerializeField]
        [Min(0)]
        private int startVal = 0;
        [SerializeField]
        private Gradient gradient;
        [Header("References to used Objects")]
        [SerializeField]
        private Image inner;
        [SerializeField]
        private Image outerLow;
        [SerializeField]
        private Image outerMid;
        [SerializeField]
        private Image outerHigh;
        [SerializeField]
        private TextMeshProUGUI text;

        private int curr;

        void Awake()
        {
            ParseGradient();
            curr = startVal;
            UpdateGauge();
        }

        void ParseGradient()
        {
            if (gradient.colorKeys.Length != 3)
            {
                Debug.Log("TODO undefined behaviour");
            }
            outerLow.color = gradient.colorKeys[0].color;
            outerLow.fillAmount = gradient.colorKeys[0].time;
            outerMid.color = gradient.colorKeys[1].color;
            outerMid.fillAmount = gradient.colorKeys[1].time;
            outerHigh.color = gradient.colorKeys[2].color;
            outerHigh.fillAmount = gradient.colorKeys[2].time;
        }

        public void Add(int delta)
        {
            curr += delta;
            Clamp();
            UpdateGauge();
        }

        private void Clamp()
        {
            if (curr > maxVal)
                curr = maxVal;
            if (curr < minVal)
                curr = minVal;
        }

        public void Set(int val)
        {
            curr = val;
            Clamp();
            UpdateGauge();
        }

        private void UpdateGauge()
        {
            float unitInterval = (float)curr / maxVal;
            inner.fillAmount = unitInterval;
            inner.color = gradient.Evaluate(unitInterval);
            Debug.Log(unitInterval);

            text.text = $"{curr}/{maxVal}";
        }
    }

}
