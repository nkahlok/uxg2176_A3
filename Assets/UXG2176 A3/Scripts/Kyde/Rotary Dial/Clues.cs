using UnityEngine;

public class Clues : MonoBehaviour
{
    public static Clues instance; // Singleton 

    public int clueUnlockCount = -1;
    public GameObject[] clues = new GameObject[3];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        
        // Assigns the clues automatically from the child count
        for(int i = 1; i <= clues.Length; i++)
        {
            clues[i-1] = transform.GetChild(i).gameObject;
        }

        // Turns off all the clues after
        for(int i = 0; i < clues.Length; i++)
        {
            clues[i].SetActive(false);
        }

    }

    // Update is called once per frame  
    void Update()
    {
        // Unlocks the clues based on clue unlock count
        if(clueUnlockCount >= 0 && clueUnlockCount <= clues.Length - 1)
            clues[clueUnlockCount].SetActive(true);
    }

    public void clueUnlockCountIncrease()
    {
        clueUnlockCount++;
    }

}
