using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class ImageColoringGirl : MonoBehaviour
{
    public Image image;
    public Button button;
    public AudioSource clickSound;
    public float tolerance = 0.1f;


    private Stack<Color[]> colorChangeStack = new Stack<Color[]>();

    private void Start()
    {


        button.onClick.AddListener(OnClick);


        // Load the saved image if it exists
        LoadSavedImage();
    }

    private void LoadSavedImage()
    {
        string imagePath = Path.Combine(Application.persistentDataPath, "GirlsavedImage.png");


        byte[] imageBytes = File.ReadAllBytes(imagePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageBytes);
        image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);


    }

    private void OnClick()
    {
        if (image == null || !IsPointerOverImage() || Input.touchCount == 2 || Input.GetMouseButton(0))
        {
            return;
        }

        button.interactable = false;

        Color[] originalColors = image.sprite.texture.GetPixels();
        colorChangeStack.Push(originalColors);

        FloodFill(image.sprite.texture, ColorSelector.GetSelectedColor());

        SaveImage(image.sprite.texture);

        button.interactable = true;
    }
    private bool IsPointerOverImage()
    {
        RectTransform rt = image.rectTransform;
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, null, out localMousePos);
        return rt.rect.Contains(localMousePos);
    }

    private void FloodFill(Texture2D texture, Color fillColor)
    {

        Texture2D newTexture = new Texture2D(texture.width, texture.height);
        newTexture.SetPixels(texture.GetPixels());
        newTexture.Apply();

        RectTransform rt = image.rectTransform;
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, null, out localMousePos);
        Vector2 uv = new Vector2((localMousePos.x + rt.rect.width / 2) / rt.rect.width, (localMousePos.y + rt.rect.height / 2) / rt.rect.height);

        int x = Mathf.FloorToInt(uv.x * texture.width);
        int y = Mathf.FloorToInt(uv.y * texture.height);


        Color targetColor = texture.GetPixel(x, y);
        FloodFill(newTexture, x, y, targetColor, fillColor);
        clickSound.Play();

        image.sprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), Vector2.one * 0.5f);


    }

    private void FloodFill(Texture2D texture, int startX, int startY, Color targetColor, Color fillColor)
    {
        int width = texture.width;
        int height = texture.height;

        bool[,] visited = new bool[width, height];

        Stack<Vector2Int> pixels = new Stack<Vector2Int>();
        pixels.Push(new Vector2Int(startX, startY));

        Color[] pixelsArray = texture.GetPixels();
        while (pixels.Count > 0)
        {
            Vector2Int pixel = pixels.Pop();
            int x = pixel.x;
            int y = pixel.y;

            if (x < 0 || x >= width || y < 0 || y >= height || visited[x, y])
                continue;

            int index = y * width + x;
            Color currentColor = pixelsArray[index];
            // Skip coloring if the pixel is black or already visited
            if (ColorsMatch(currentColor, Color.black) || visited[x, y])
                continue;

            // Apply fill color if the current pixel is the target color
            if (ColorsMatch(currentColor, targetColor))
            {
                pixelsArray[index] = fillColor;
                visited[x, y] = true;

                pixels.Push(new Vector2Int(x + 1, y)); // Right
                pixels.Push(new Vector2Int(x - 1, y)); // Left
                pixels.Push(new Vector2Int(x, y + 1)); // Up
                pixels.Push(new Vector2Int(x, y - 1)); // Down
            }
            // Handle anti-aliased edges
            else
            {
                // Check neighboring pixels for anti-aliasing
                Color averageColor = GetAverageColor(texture, x, y);
                if (!ColorsMatch(averageColor, targetColor))
                {
                    pixelsArray[index] = averageColor;
                    visited[x, y] = true;

                    pixels.Push(new Vector2Int(x + 1, y)); // Right
                    pixels.Push(new Vector2Int(x - 1, y)); // Left
                    pixels.Push(new Vector2Int(x, y + 1)); // Up
                    pixels.Push(new Vector2Int(x, y - 1)); // Down
                }
            }
        }

        texture.SetPixels(pixelsArray);
        texture.Apply();
    }
    private Color GetAverageColor(Texture2D texture, int x, int y)
    {
        Color[] neighbors = new Color[4]; // Only considering 4 neighboring pixels

        neighbors[0] = texture.GetPixel(x + 1, y); // Right
        neighbors[1] = texture.GetPixel(x - 1, y); // Left
        neighbors[2] = texture.GetPixel(x, y + 1); // Up
        neighbors[3] = texture.GetPixel(x, y - 1); // Down

        // Calculate the average color
        Color averageColor = new Color(0, 0, 0, 0);
        foreach (Color neighbor in neighbors)
        {
            averageColor += neighbor;
        }
        averageColor /= neighbors.Length;

        return averageColor;
    }
    private void SaveImage(Texture2D modifiedTexture)
    {
        string savePath = Path.Combine(Application.persistentDataPath, "GirlsavedImage.png");

        var data = modifiedTexture.EncodeToPNG();

        File.WriteAllBytes(savePath, data);

        Debug.Log("Image saved at: " + savePath);
    }
    private bool ColorsMatch(Color color1, Color color2)
    {
        float deltaR = color1.r - color2.r;
        float deltaG = color1.g - color2.g;
        float deltaB = color1.b - color2.b;

        // Calculate squared magnitude for efficiency
        float sqrMagnitude = deltaR * deltaR + deltaG * deltaG + deltaB * deltaB;

        return sqrMagnitude < tolerance * tolerance;
    }

    private void UndoColorChange()
    {
        if (colorChangeStack.Count > 0)
        {
            Color[] originalColors = colorChangeStack.Pop();
            image.sprite.texture.SetPixels(originalColors);
            image.sprite.texture.Apply();
        }
    }

    public void UndoButtonClick()
    {
        UndoColorChange();
        SaveImage(image.sprite.texture);
    }
}