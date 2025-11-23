using UnityEngine;
using UnityEngine.UI;
public class CubePuzzleManager : MonoBehaviour
{
    public static CubePuzzleManager instance;

    private Color[] imageColors = new Color[3]; // randomized colors
    private Color[] cubeColors = new Color[3];  // cube materials

    void Awake()
    {
        instance = this;
    }

    public void UpdateImageColor(int index, Color newColor)
    {
        imageColors[index] = newColor;
        CheckForMatch();
    }

    public void UpdateCubeColor(int index, Color newColor)
    {
        cubeColors[index] = newColor;
        CheckForMatch();
    }

    void CheckForMatch()
    {
        for (int i = 0; i < 3; i++)
        {
            if (cubeColors[i] != imageColors[i])
                return;
        }

        Debug.Log("UNLOCK!");
    }
}
