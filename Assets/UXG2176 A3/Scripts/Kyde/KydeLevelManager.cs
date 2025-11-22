using System.Runtime.ExceptionServices;
using UnityEngine;

public class KydeLevelManager : MonoBehaviour
{

    [SerializeField]
    private GameObject firstDoor;
    [SerializeField]
    private GameObject secondDoor;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Remove 1st door after getting all 3 clues from 1st 3 telephone
        if(Clues.instance.clueUnlockCount >= Clues.instance.clues.Length - 1)
        {
            firstDoor.SetActive(false);
        }
        // Remove 2nd door after getting the 4th telephone right
        if(Clues.instance.clueUnlockCount > Clues.instance.clues.Length - 1)
        {
            secondDoor.SetActive(false);
        }
    }
}
