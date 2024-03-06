using UnityEngine;
using UnityEngine.UI;

public class PaintBucket : MonoBehaviour
{
    public Color fillColor = Color.red; 
    public Image fillableImage; 

    private void Start()
    {
      
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
      
        if (fillableImage != null)
        {
           
            fillableImage.color = fillColor;
        }
        else
        {
            Debug.LogWarning("Fillable Image is not assigned.");
        }
    }
}