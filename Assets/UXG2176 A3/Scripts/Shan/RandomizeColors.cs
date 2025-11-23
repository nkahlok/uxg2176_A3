using UnityEngine;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomizeColors : MonoBehaviour
{
    [SerializeField] private Image[] images; 
    [SerializeField] private Color a, b, c;

    private Color[] colorOptions;

    void Start()
    {
        // Store colors in an array for easier random picking
        colorOptions = new Color[] { a, b, c };

        RandomizeColor();
    }

    public void RandomizeColor()
    {
        for (int i = 0; i < images.Length; i++)
        {
            int colorIndex = Random.Range(0, colorOptions.Length);
            images[i].color = colorOptions[colorIndex];

            CubePuzzleManager.instance.UpdateImageColor(i, colorIndex);
        }
    }
}
