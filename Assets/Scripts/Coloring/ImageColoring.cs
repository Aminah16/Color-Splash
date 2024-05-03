using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Rendering.Universal;

public class ImageColoring : MonoBehaviour
{
    public Image image;
    public Button button;
    public AudioSource clickSound;
    public float tolerance = 0.1f;
    private static readonly Color BLACK_COLOR = Color.black;
    private static readonly Color TRANSPARENT_COLOR = new Color(0, 0, 0, 0);
    public float animationSpeed;
    public string saveFile;

    private Stack<Color[]> colorChangeStack = new Stack<Color[]>();

    private void Start()
    {
        button.onClick.AddListener(OnClick);
        LoadSavedImage();
    }

    private void LoadSavedImage()
    {
        string imagePath = Path.Combine(Application.persistentDataPath, saveFile + "savedImage.png");

        if (File.Exists(imagePath))
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }
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
        RectTransform rt = image.rectTransform;
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, null, out localMousePos);
        Vector2 uv = new Vector2((localMousePos.x + rt.rect.width / 2) / rt.rect.width, (localMousePos.y + rt.rect.height / 2) / rt.rect.height);

        int x = Mathf.FloorToInt(uv.x * texture.width);
        int y = Mathf.FloorToInt(uv.y * texture.height);

        Color targetColor = texture.GetPixel(x, y);

        StartCoroutine(FloodFill(texture, x, y, targetColor, fillColor, CalculateAnimationSpeed(texture.width * texture.height), 30000));
    }

    private float CalculateAnimationSpeed(int pixelCount)
    {
        // Adjust these values to control the animation speed based on the pixel count
        float minSpeed = 0.0001f;
        float maxSpeed = 0.01f;
        float minPixelCount = 20000; // Adjust the threshold as needed

        // Calculate the animation speed based on the pixel count
        float speed = Mathf.Lerp(maxSpeed, minSpeed, Mathf.Clamp01((float)pixelCount / minPixelCount));
        return speed;
    }

    private IEnumerator FloodFill(Texture2D texture, int startX, int startY, Color targetColor, Color fillColor, float speed, int pixelsPerGroup)
    {
        int width = texture.width;
        int height = texture.height;

        bool[,] visited = new bool[width, height];

        Queue<IntVector2> pixels = new Queue<IntVector2>();
        pixels.Enqueue(new IntVector2(startX, startY));

        int pixelsPaintedInGroup = 0;

        while (pixels.Count > 0)
        {
            IntVector2 pixel = pixels.Dequeue();
            int x = pixel.x;
            int y = pixel.y;

            if (x < 0 || x >= width || y < 0 || y >= height || visited[x, y])
                continue;

            int index = y * width + x;
            Color currentColor = texture.GetPixel(x, y);

            // Skip coloring if the pixel is black or already visited
            if (ColorsMatch(currentColor, BLACK_COLOR) || visited[x, y])
                continue;

            // Apply fill color if the current pixel is the target color
            if (ColorsMatch(currentColor, targetColor))
            {
                visited[x, y] = true;

                // Animate the filling process
                texture.SetPixel(x, y, fillColor);
                pixelsPaintedInGroup++;

                // If enough pixels have been painted in this group, yield and reset the counter
                if (pixelsPaintedInGroup >= pixelsPerGroup)
                {
                    texture.Apply();
                    yield return new WaitForSeconds(speed);
                    pixelsPaintedInGroup = 0;
                }

                // Add neighboring pixels to the queue, prioritizing those closer to the center
                pixels.Enqueue(new IntVector2(x + 1, y)); // Right
                pixels.Enqueue(new IntVector2(x - 1, y)); // Left
                pixels.Enqueue(new IntVector2(x, y + 1)); // Up
                pixels.Enqueue(new IntVector2(x, y - 1)); // Down
            }
            else
            {
                // Check neighboring pixels for anti-aliasing
                Color averageColor = GetAverageColor(texture, x, y);
                if (!ColorsMatch(averageColor, targetColor))
                {
                    texture.SetPixel(x, y, averageColor);
                    visited[x, y] = true;

                    pixels.Enqueue(new IntVector2(x + 1, y)); // Right
                    pixels.Enqueue(new IntVector2(x - 1, y)); // Left
                    pixels.Enqueue(new IntVector2(x, y + 1)); // Up
                    pixels.Enqueue(new IntVector2(x, y - 1)); // Down
                }
            }
        }

        // Apply remaining changes if any
        texture.Apply();
        SaveImage(texture);

        yield return null; // Ensure the coroutine finishes properly
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
        string savePath = Path.Combine(Application.persistentDataPath, saveFile + "savedImage.png");

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

    private struct IntVector2
    {
        public int x;
        public int y;

        public IntVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
