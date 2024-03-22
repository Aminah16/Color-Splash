using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class ImageLoaderCat : MonoBehaviour
{
    private Image image;
    private string imagePath;
    private DateTime lastWriteTime;

    private void Start()
    {
        image = GetComponent<Image>();
        imagePath = Path.Combine(Application.persistentDataPath, "CatsavedImage.png");

        // Get the initial last write time of the image file
        lastWriteTime = File.GetLastWriteTime(imagePath);

        // Load the image when the script starts
        LoadSavedImage();
    }

    private void Update()
    {
        // Check if the image file has been modified
        DateTime currentWriteTime = File.GetLastWriteTime(imagePath);
        if (currentWriteTime != lastWriteTime)
        {
            // Update the last write time
            lastWriteTime = currentWriteTime;

            // Load the image
            LoadSavedImage();
        }
    }

    private void LoadSavedImage()
    {
        // Check if the file exists
        if (File.Exists(imagePath))
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }
    }
}