using UnityEngine;

public class LaserPuzzleAudioManager : MonoBehaviour
{
    public static LaserPuzzleAudioManager Instance { get; private set; }

    [Header("Mirror Sounds")]
    [SerializeField] private AudioClip mirrorRotateSound;
    [SerializeField] private float mirrorRotateVolume = 0.7f;

    [Header("Laser Sounds")]
    [SerializeField] private AudioClip laserHumSound;
    [SerializeField] private float laserHumVolume = 0.5f;
    [SerializeField] private AudioClip laserReflectSound;
    [SerializeField] private float laserReflectVolume = 0.4f;

    [Header("Receiver Sounds")]
    [SerializeField] private AudioClip receiverHitSound;
    [SerializeField] private float receiverHitVolume = 0.6f;
    [SerializeField] private AudioClip receiverChargeSound;
    [SerializeField] private float receiverChargeVolume = 0.5f;
    [SerializeField] private AudioClip doorOpenSound;
    [SerializeField] private float doorOpenVolume = 0.8f;

    [Header("UI Sounds")]
    [SerializeField] private AudioClip successSound;
    [SerializeField] private float successVolume = 0.7f;
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private float errorVolume = 0.5f;

    [Header("Background Music")]
    [SerializeField] private AudioClip levelBGM;
    [SerializeField] private float bgmVolume = 0.3f;
    [SerializeField] private bool playBGMOnStart = true;
    [SerializeField] private float bgmFadeInDuration = 2f;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource laserLoopAudioSource;
    [SerializeField] private AudioSource receiverLoopAudioSource;
    [SerializeField] private AudioSource bgmAudioSource;

    private float bgmFadeTimer = 0f;
    private bool isFadingIn = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SetupAudioSources();
        
        if (playBGMOnStart && levelBGM != null)
        {
            PlayBGM();
        }
    }

    private void Update()
    {
        if (isFadingIn && bgmFadeTimer < bgmFadeInDuration)
        {
            bgmFadeTimer += Time.deltaTime;
            float fadeProgress = bgmFadeTimer / bgmFadeInDuration;
            bgmAudioSource.volume = Mathf.Lerp(0f, bgmVolume, fadeProgress);
            
            if (bgmFadeTimer >= bgmFadeInDuration)
            {
                bgmAudioSource.volume = bgmVolume;
                isFadingIn = false;
            }
        }
    }

    private void SetupAudioSources()
    {
        if (sfxAudioSource == null)
        {
            sfxAudioSource = gameObject.AddComponent<AudioSource>();
        }
        sfxAudioSource.playOnAwake = false;
        sfxAudioSource.spatialBlend = 0f; // 2D sound

        if (laserLoopAudioSource == null)
        {
            laserLoopAudioSource = gameObject.AddComponent<AudioSource>();
        }
        laserLoopAudioSource.loop = true;
        laserLoopAudioSource.playOnAwake = false;
        laserLoopAudioSource.spatialBlend = 0f;

        if (receiverLoopAudioSource == null)
        {
            receiverLoopAudioSource = gameObject.AddComponent<AudioSource>();
        }
        receiverLoopAudioSource.loop = true;
        receiverLoopAudioSource.playOnAwake = false;
        receiverLoopAudioSource.spatialBlend = 0f;

        if (bgmAudioSource == null)
        {
            bgmAudioSource = gameObject.AddComponent<AudioSource>();
        }
        bgmAudioSource.loop = true;
        bgmAudioSource.playOnAwake = false;
        bgmAudioSource.spatialBlend = 0f;
        bgmAudioSource.priority = 128;
    }

    public void PlayMirrorRotate()
    {
        if (mirrorRotateSound != null)
        {
            sfxAudioSource.PlayOneShot(mirrorRotateSound, mirrorRotateVolume);
        }
    }

    public void PlayLaserHum()
    {
        if (laserHumSound != null && !laserLoopAudioSource.isPlaying)
        {
            laserLoopAudioSource.clip = laserHumSound;
            laserLoopAudioSource.volume = laserHumVolume;
            laserLoopAudioSource.Play();
        }
    }

    public void StopLaserHum()
    {
        if (laserLoopAudioSource.isPlaying)
        {
            laserLoopAudioSource.Stop();
        }
    }
    public void PlayLaserReflect()
    {
        if (laserReflectSound != null)
        {
            sfxAudioSource.PlayOneShot(laserReflectSound, laserReflectVolume);
        }
    }
    public void PlayReceiverHit()
    {
        if (receiverHitSound != null)
        {
            sfxAudioSource.PlayOneShot(receiverHitSound, receiverHitVolume);
        }
    }
    public void PlayReceiverCharging()
    {
        if (receiverChargeSound != null && !receiverLoopAudioSource.isPlaying)
        {
            receiverLoopAudioSource.clip = receiverChargeSound;
            receiverLoopAudioSource.volume = receiverChargeVolume;
            receiverLoopAudioSource.Play();
        }
    }

    public void StopReceiverCharging()
    {
        if (receiverLoopAudioSource.isPlaying)
        {
            receiverLoopAudioSource.Stop();
        }
    }

    public void PlayDoorOpen()
    {
        if (doorOpenSound != null)
        {
            sfxAudioSource.PlayOneShot(doorOpenSound, doorOpenVolume);
        }
    }
    public void PlaySuccess()
    {
        if (successSound != null)
        {
            sfxAudioSource.PlayOneShot(successSound, successVolume);
        }
    }

    public void PlayError()
    {
        if (errorSound != null)
        {
            sfxAudioSource.PlayOneShot(errorSound, errorVolume);
        }
    }
    public void PlayClip(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            sfxAudioSource.PlayOneShot(clip, volume);
        }
    }

    public void PlayBGM()
    {
        if (levelBGM != null && bgmAudioSource != null)
        {
            bgmAudioSource.clip = levelBGM;
            bgmAudioSource.volume = 0f;
            bgmAudioSource.Play();
            
            // Start fade in
            bgmFadeTimer = 0f;
            isFadingIn = true;
        }
    }

    public void PlayBGM(AudioClip clip, float volume)
    {
        if (clip != null && bgmAudioSource != null)
        {
            StopBGM();
            
            levelBGM = clip;
            bgmVolume = volume;
            bgmAudioSource.clip = clip;
            bgmAudioSource.volume = 0f;
            bgmAudioSource.Play();
            
            // Start fade in
            bgmFadeTimer = 0f;
            isFadingIn = true;
        }
    }
    public void StopBGM()
    {
        if (bgmAudioSource != null && bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop();
            isFadingIn = false;
        }
    }

    public void FadeOutBGM(float duration = 2f)
    {
        if (bgmAudioSource != null && bgmAudioSource.isPlaying)
        {
            StartCoroutine(FadeOutCoroutine(duration));
        }
    }

    private System.Collections.IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = bgmAudioSource.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            bgmAudioSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        bgmAudioSource.volume = 0f;
        bgmAudioSource.Stop();
    }

    public void PauseBGM()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Pause();
        }
    }
    public void ResumeBGM()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.UnPause();
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (bgmAudioSource != null && !isFadingIn)
        {
            bgmAudioSource.volume = bgmVolume;
        }
    }

    public float GetBGMVolume()
    {
        return bgmVolume;
    }

    public bool IsBGMPlaying()
    {
        return bgmAudioSource != null && bgmAudioSource.isPlaying;
    }
}