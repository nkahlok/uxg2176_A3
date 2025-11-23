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

        // Play drone SFX but mute volume
        drone.volume = 0.0f;
        drone.Play();
    }

    void Update()
    {
        // unmute when in drone state
        if(Player.Instance.playerState == Player.PlayerState.DRONE)
        {
            drone.volume = 1f;
        }
        // mute when not in drone state
        else
        {
            drone.volume = 0f;
        }
    }

    public void PlayDialSound()
    {
        audioSource.PlayOneShot(telephone); // Plays the sound effect once
    }
}
