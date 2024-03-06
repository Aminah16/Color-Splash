using UnityEngine;
using UnityEngine.UI;

public class ImageColoring : MonoBehaviour
{
    private Image image;
    public AudioSource clickSound; // Assign your audio clip in the Unity Editor


    private void Start()
    {
        // Attach the image click event
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        // Get the Image component
        image = GetComponent<Image>();
    }

    private void OnClick()
    {
        // Change the color of the Image component when clicked
        image.color = ColorSelector.GetSelectedColor();
        clickSound.Play();
    }
}