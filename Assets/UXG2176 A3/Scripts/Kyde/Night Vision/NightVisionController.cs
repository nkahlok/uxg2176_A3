using UnityEngine;
using UnityEngine.Rendering;

public class NightVisionController : MonoBehaviour
{
    public Volume nightVisionVolume;
    private bool isNightVisionOn;

    public GameObject[] nightVisionClues;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Turn off night vision at the start of the game
        nightVisionVolume.weight = isNightVisionOn ? 1 : 0;
        
        // Turn off all clues at the start
        foreach(GameObject clues in nightVisionClues)
        {
            clues.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {


        // Boolean switch
        // Weight of pp volume = 1 when boolean true, else 0

        if (Player.Instance.playerState != Player.PlayerState.DRONE)
        {
            isNightVisionOn = false;
            nightVisionVolume.weight = isNightVisionOn ? 1 : 0;

            // Turn on clues when it is in night vision
            foreach (GameObject clues in nightVisionClues)
            {
                clues.SetActive(false);
            }

        }
        else if (Player.Instance.playerState == Player.PlayerState.DRONE)
        {
            isNightVisionOn = true;
            nightVisionVolume.weight = isNightVisionOn ? 1 : 0;

            // Turn on clues when it is in night vision
            foreach (GameObject clues in nightVisionClues)
            {
                clues.SetActive(true);
            }
        }
    }
    

}
