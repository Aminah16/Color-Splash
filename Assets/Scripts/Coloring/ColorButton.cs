using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    public Color selectedColor; // Assign the color in the Unity Editor
    public AudioSource clip;
    public Button button;
    

    private void Start()
    {
        // Attach the button click event
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        
    }


    private void OnClick()
    {
        // Set the selected color when the button is clicked
        ColorSelector.SetSelectedColor(selectedColor, button);
        clip.Play();
    }
}