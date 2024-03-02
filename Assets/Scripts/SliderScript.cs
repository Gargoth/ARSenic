using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderText;
    private float x = 0;
    private float y = 9.81f;
    private float z = 274;
    private float sliderText;

    // Start is called before the first frame update
    void Start()
    {
        _slider.onValueChanged.AddListener((v) =>
        {
            float A = (x*z - Mathf.Pow(y,2)) / (x - 2*y + z);
            float B = Mathf.Pow((y-x),2) / (x - 2 * y + z);
            float C = 2 * Mathf.Log((z-y) / (y-x));

            sliderText = (A + B * Mathf.Exp(C * v));
            _sliderText.text = sliderText.ToString("0.00") + "m/s²";
        });
    }

    // Update is called once per frame
    void Update()
    {
        float A = (x * z - Mathf.Pow(y, 2)) / (x - 2 * y + z);
        float B = Mathf.Pow((y - x), 2) / (x - 2 * y + z);
        float C = 2 * Mathf.Log((z - y) / (y - x));

        // Mars
        if (sliderText > 1 && sliderText < 5)
        {
            _slider.value = Mathf.Log((3.7f - A) / B) / C;
        }

        // Earth
        if (sliderText > 8 && sliderText < 17)
        {
            _slider.value = 0.5f;
        }

        // Jupiter
        else if (sliderText > 20 && sliderText < 40)
        {
            _slider.value = Mathf.Log((24.8f - A) / B) / C;
        }

        // Sun
        else if (sliderText > 200 && sliderText < 274)
        {
            _slider.value = 1;
        }

    }
}