using UnityEngine;

public class KydeSoundManager : MonoBehaviour
{
    AudioSource audioSource;


    public AudioClip telephone;
    public AudioSource drone;

    public static KydeSoundManager instance; // Singleton

    void Start()
    {   
        audioSource = GetComponent<AudioSource>();
        instance = this;
    }

    // Example: Play sound when a key is pressed
    void Update()
    {
        // Play drone SFX if we press 3 while not in drone state
        if(Player.Instance.playerState != Player.PlayerState.DRONE && Input.GetKeyDown(KeyCode.Alpha3))
        //if(Player.Instance.playerState == Player.PlayerState.DRONE)
        {
            PlayDroneSound();
        }
        // Stop drone SFX if we press any keys to get out of drone state while in drone state
        else if (Player.Instance.playerState == Player.PlayerState.DRONE && Input.GetKeyDown(KeyCode.Alpha1) || Player.Instance.playerState == Player.PlayerState.DRONE && Input.GetKeyDown(KeyCode.Alpha2))
        //else if (Player.Instance.playerState != Player.PlayerState.DRONE)
        {
            StopDroneSound();
        }
    }

    public void PlayDialSound()
    {
       
        audioSource.PlayOneShot(telephone); // Plays the sound effect once
  
    }

    public void PlayDroneSound()
    {
        // Enable looping for the drone SFX
        drone.Play();

    }

    public void StopDroneSound()
    {

        drone.Stop();

    }


}
