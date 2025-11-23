using UnityEngine;
using UnityEngine.UI;
public class CubePuzzleManager : MonoBehaviour
{
    public static CubePuzzleManager instance;

    [SerializeField] private int[] imageColors = new int[3]; // randomized colors
    [SerializeField] private int[] cubeColors = new int[3];  // cube materials

    void Awake()
    {
        instance = this;
    }

    public void UpdateImageColor(int index, int colorIndex)
    {
        imageColors[index] = colorIndex;
        CheckForMatch();
    }

    public void UpdateCubeColor(int index, int colorIndex)
    {
        cubeColors[index] = colorIndex;
        CheckForMatch();
    }

    void CheckForMatch()
    {
        for (int i = 0; i < 3; i++)
        {
            if (cubeColors[i] != imageColors[i])
                return;
        }

        Debug.Log("Unlock!");
    }
}
