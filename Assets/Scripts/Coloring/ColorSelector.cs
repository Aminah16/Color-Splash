using UnityEngine;
using UnityEngine.UI;

public static class ColorSelector
{
    private static Color selectedColor = Color.yellow; // Default color
    private static Button selectedButton; // Reference to the selected color button
    private static Outline outline; // Reference to the outline component
    private static bool canApplyColor = true; // Flag to indicate whether coloring should be applied

    // Method to initialize the default selected button
    public static void InitializeDefaultButton(Button defaultButton)
    {
        SetSelectedColor(defaultButton.colors.normalColor, defaultButton);
    }

    public static void SetSelectedColor(Color color, Button button)
    {
        // Set the selected color with alpha set to 1 (opaque)
        selectedColor = new Color(color.r, color.g, color.b, 1f);

        // Update the selected button and highlight it
        if (selectedButton != null)
        {
            // Reset the color of the previous selected button (assuming it has an Image component)
            var previousImage = selectedButton.GetComponent<Image>();

            // Destroy the outline component of the previous selected button, if it exists
            if (outline != null)
            {
                GameObject.Destroy(outline);
                outline = null;
            }
        }

        selectedButton = button;

        // Change the color of the new selected button (assuming it has an Image component)
        var selectedImage = selectedButton.GetComponent<Image>();
        if (selectedImage != null)
        {
            selectedImage.color = selectedColor;

            // Add black color outline to the selected button
            outline = selectedButton.gameObject.AddComponent<Outline>();
            outline.effectColor = Color.white; // Set outline color to black
            outline.effectDistance = new Vector2(7f, -7f); // Increase the thickness to 10f or adjust as desired
        }
    }

    public static Color GetSelectedColor()
    {
        // Get the selected color
        return selectedColor;
    }

    // Method to set whether coloring should be applied or not
    public static void SetCanApplyColor(bool value)
    {
        canApplyColor = value;
    }

    // Method to check whether coloring should be applied
    public static bool CanApplyColor()
    {
        return canApplyColor;
    }
}