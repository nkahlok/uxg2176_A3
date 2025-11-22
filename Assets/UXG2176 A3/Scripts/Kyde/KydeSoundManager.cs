using UnityEngine;

public class KydeSoundManager : MonoBehaviour
{
    AudioSource audioSource;


    public AudioClip telephone;
    public AudioClip drone;

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
        {
            PlayDroneSound();
        }
        // Stop drone SFX if we press any keys to get out of drone state while in drone state
        else if (Player.Instance.playerState == Player.PlayerState.DRONE && Input.GetKeyDown(KeyCode.Alpha1) || Player.Instance.playerState == Player.PlayerState.DRONE && Input.GetKeyDown(KeyCode.Alpha2))
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
        audioSource.loop = true;
        audioSource.PlayOneShot(drone);

    }

    public void StopDroneSound()
    {

        audioSource.loop = false;
        audioSource.Stop();

    }


}
