using UnityEngine;

public static class ColorSelector
{
    private static Color selectedColor = Color.white; // Default color

    public static void SetSelectedColor(Color color)
    {
        // Set the selected color with alpha set to 1 (opaque)
        selectedColor = new Color(color.r, color.g, color.b, 1f);
    }

    public static Color GetSelectedColor()
    {
        // Get the selected color
        return selectedColor;
    }
}