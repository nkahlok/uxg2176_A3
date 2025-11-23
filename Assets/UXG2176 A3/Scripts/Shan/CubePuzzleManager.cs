using UnityEngine;
using UnityEngine.UI;
public class CubePuzzleManager : MonoBehaviour
{
    public static CubePuzzleManager instance;

    [SerializeField] private int[] imageColors = new int[3]; // Randomized colors
    [SerializeField] private int[] cubeColors = new int[3];  // Cube materials
    [SerializeField] private AudioSource puzzleUnlockSfx;
    [SerializeField] private GameObject colorObj;
    [SerializeField] private GameObject colorTxt;
    [SerializeField] private GameObject objectiveTxt;
    [SerializeField] private GameObject exitObjTxt;
    [SerializeField] private GameObject exitObj;

    private bool isPuzzleInit;
    void Awake()
    {
        instance = this;

        exitObj.SetActive(false);
        exitObjTxt.SetActive(false);
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
        // If has not randomize colors, return
        if (!isPuzzleInit) return;

        // If colors mismatch, return
        for (int i = 0; i < 3; i++)
        {
            if (cubeColors[i] != imageColors[i])
                return;
        }

        Debug.Log("Unlock!");
        // Play unlock sfx
        puzzleUnlockSfx.Play();

        // Set exit txt active
        exitObjTxt.SetActive(true);

        // Set other txts false
        objectiveTxt.SetActive(false);
        colorTxt.SetActive(false);
        colorObj.SetActive(false);

        // Set exit active
        exitObj.SetActive(true);
    }

    public void StartPuzzleInit()
    {
        isPuzzleInit = true;
    }
}
