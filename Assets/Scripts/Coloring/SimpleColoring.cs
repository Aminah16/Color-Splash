using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleColoring : MonoBehaviour
{

    private Image image;
    public AudioSource clickSound; // Assign your audio clip in the Unity Editor


    // Stack to store color changes for undo
    private Stack<Color> colorChangeStack = new Stack<Color>();

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

    // Record the current color for undo
        colorChangeStack.Push(image.color);

        // Change the color of the Image component when clicked
        image.color = ColorSelector.GetSelectedColor();

        // Play click sound
        clickSound.Play();
    }

    // Implement the undo functionality
    private void UndoColorChange()
    {
        if (colorChangeStack.Count > 1) // Ensure there's at least one color change to undo
        {
            // Pop the last color from the stack
            colorChangeStack.Pop();

            // Set the previous color
            image.color = colorChangeStack.Peek();
        }
    }

    // Example: Trigger undo on a button click
    public void UndoButtonClick()
    {
        UndoColorChange();
    }
}