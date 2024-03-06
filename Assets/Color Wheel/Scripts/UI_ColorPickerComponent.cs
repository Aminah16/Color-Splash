using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public enum EUIColourComponent
{
    Hue,
    Saturation,
    Value
}
public class UI_ColorPickerComponent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ComponentLabel;
    [SerializeField] Slider ComponentSlider;
    [SerializeField] EUIColourComponent ComponentType;
    [SerializeField] UnityEvent<EUIColourComponent, float> onColorComponentChanged = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onSliderChanged(float newValue)
    {
        onColorComponentChanged.Invoke(ComponentType, newValue);
    }
}
