using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ColorWheel : MonoBehaviour
{
    [SerializeField] RawImage ColourWheelImage;
    [SerializeField] int WheelBufferInPixels = 5;

    // Start is called before the first frame update
    void Start()
    {
        BuildColourWheelTexture();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void BuildColourWheelTexture()
    {
        Canvas OwningCanvas = ColourWheelImage.GetComponentInParent<Canvas>();
        Rect ActualRectangle=  RectTransformUtility.PixelAdjustRect(ColourWheelImage.rectTransform, OwningCanvas);
        Debug.Log(ActualRectangle);
        int WheelTextureSize = Mathf.FloorToInt( Mathf.Min(ActualRectangle.width, ActualRectangle.height));
        Texture2D WheelTexture = new Texture2D(WheelTextureSize, WheelTextureSize, TextureFormat.RGB24, false);
        float MaxDistance = WheelTextureSize - WheelBufferInPixels;
        float MaxDistanceSq = MaxDistance * MaxDistance;
        // build the texture
        for (int y = 0; y < WheelTextureSize; y++) 
        {
            for (int x = 0; x < WheelTextureSize; x++) 
            {
                Vector2 vectorFromCenter = new Vector2(x - (WheelTextureSize / 2f),
                                                       y - (WheelTextureSize / 2f));

                float DistanceFromCenterSq = vectorFromCenter.sqrMagnitude;

                if (DistanceFromCenterSq < MaxDistanceSq)
                {
                    float Angle  = Mathf.Atan2(vectorFromCenter.y, vectorFromCenter.x);
                    if (Angle < 0)
                    {
                        Angle += Mathf.PI * 2f;

                        float Hue = Mathf.Clamp01(Angle / (Mathf.PI * 2));
                        float Saturation = Mathf.Clamp01(Mathf.Sqrt(DistanceFromCenterSq) / MaxDistance);

                        WheelTexture.SetPixel(x, y, Color.HSVToRGB(Hue, Saturation, 1f));
                    }
                }
                else
                    WheelTexture.SetPixel(x, y, Color.white);
            }
        }

        WheelTexture.Apply();
        ColourWheelImage.texture = WheelTexture;
    }
}
