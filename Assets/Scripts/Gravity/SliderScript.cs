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
    private float z = 17;
    public float gravityForce;
    
    // Sprites
    public Sprite moon;
    public Sprite mars;
    public Sprite earth;
    public Sprite neptune;
    

    // Start is called before the first frame update
    void Start()
    {
        // set initial gravity to Earth's
        gravityForce = 9.81f;
        _slider.onValueChanged.AddListener((v) =>
        {
            float A = (x*z - Mathf.Pow(y,2)) / (x - 2*y + z);
            float B = Mathf.Pow((y-x),2) / (x - 2 * y + z);
            float C = 2 * Mathf.Log((z-y) / (y-x));

            gravityForce = (A + B * Mathf.Exp(C * v));
            _sliderText.text = gravityForce.ToString("0.00") + "m/s²";
        });
    }

    // Update is called once per frame
    void Update()
    {
        float A = (x * z - Mathf.Pow(y, 2)) / (x - 2 * y + z);
        float B = Mathf.Pow((y - x), 2) / (x - 2 * y + z);
        float C = 2 * Mathf.Log((z - y) / (y - x));

        // Moon
        if (gravityForce == 0)
        {
            // Find the slider handle and change its image
            Transform handleTransform = _slider.transform.Find("Handle Slide Area").Find("Handle");
            if (handleTransform != null)
            {
                Image handleImage = handleTransform.GetComponent<Image>();
                if (handleImage != null)
                {
                    handleImage.sprite = moon;
                }
            }
        }
        
        // Mars
        else if (gravityForce > 3 && gravityForce < 5)
        {
            _slider.value = Mathf.Log((3.7f - A) / B) / C;
            // Find the slider handle and change its image
            Transform handleTransform = _slider.transform.Find("Handle Slide Area").Find("Handle");
            if (handleTransform != null)
            {
                Image handleImage = handleTransform.GetComponent<Image>();
                if (handleImage != null)
                {
                    handleImage.sprite = mars;
                }
            }
        }

        // Earth
        else if (gravityForce > 9 && gravityForce < 10)
        {
            _slider.value = 0.5f;
            // Find the slider handle and change its image
            Transform handleTransform = _slider.transform.Find("Handle Slide Area").Find("Handle");
            if (handleTransform != null)
            {
                Image handleImage = handleTransform.GetComponent<Image>();
                if (handleImage != null)
                {
                    handleImage.sprite = earth;
                }
            }
        }

        // Neptune
        else if (gravityForce > 11 && gravityForce < 13)
        {
            _slider.value = Mathf.Log((11.15f - A) / B) / C;
            // Find the slider handle and change its image
            Transform handleTransform = _slider.transform.Find("Handle Slide Area").Find("Handle");
            if (handleTransform != null)
            {
                Image handleImage = handleTransform.GetComponent<Image>();
                if (handleImage != null)
                {
                    handleImage.sprite = neptune;
                }
            }
        }
    }
}