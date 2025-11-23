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
        bool allSame;

        do
        {
            allSame = true;

            // Roll first color and store its index
            int firstIndex = Random.Range(0, colorOptions.Length);

            images[0].color = colorOptions[firstIndex];
            CubePuzzleManager.instance.UpdateImageColor(0, firstIndex);

            // Roll the remaining colors
            for (int i = 1; i < images.Length; i++)
            {
                int index = Random.Range(0, colorOptions.Length);
                images[i].color = colorOptions[index];
                CubePuzzleManager.instance.UpdateImageColor(i, index);

                if (index != firstIndex)
                {
                    allSame = false; // at least one is different
                }
            }

        } while (allSame);

        CubePuzzleManager.instance.StartPuzzleInit();
    }
}
