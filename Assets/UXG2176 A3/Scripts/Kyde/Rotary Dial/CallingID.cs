using TMPro;
using UnityEngine;

public class CallingID : MonoBehaviour
{
    public static CallingID instance; // Singleton of class

    public string callerID; // The correct number for the telephone puzzle

    string dialedNumber; // The number that the player dials
    int count; // The number of char in dialedNumber

    TextMeshProUGUI displayNumber;

    Canvas rotaryDialCanvas;

    bool puzzleSolved; // Boolean for when puzzle is solved

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;

        displayNumber = GetComponent<TextMeshProUGUI>();

        rotaryDialCanvas = transform.parent.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        displayNumber.text = dialedNumber;

        if(count == callerID.Length)
        {
            if(dialedNumber != callerID)
            {
                dialedNumber = "";
                count = 0;
            }
            else if(dialedNumber == callerID)
            {
                // Turns off the canvas
                rotaryDialCanvas.enabled = false;

                // Brings back the cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                // Increase clue counter
                Clues.instance.clueUnlockCountIncrease();

                puzzleSolved = true;

                // Resets the dialed number to become empty
                dialedNumber = "";
            }
        }
        
    }

    public void AddDialNumber(string number)
    {
        dialedNumber += number;
        count++;
    }
   

    public void ResetDialedNumber()
    {
        dialedNumber = "";
        count = 0;
    }

    public bool GetPuzzleSolved()
    {
        return puzzleSolved;
    }

}
