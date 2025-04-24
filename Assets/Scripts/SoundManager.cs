using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; private set;}

    public AudioSource backgroundMusic;
    public AudioClip backgroundMusicClip;

    [Header("Coin Sounds")]
    [SerializeField] private AudioSource coinAudioSource;
    [SerializeField] private AudioClip defaultCoinSound;
    [SerializeField] private AudioClip[] comboCoinSounds;
    [SerializeField] private float comboThreshold = .1f;
    private int comboIndex = 0;
    private float lastCoinPickupTime = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayBackgroundMusic(backgroundMusicClip);
    }

    public void PlayCoinSound()
    {
        float currentTime = Time.time;

        if(currentTime - lastCoinPickupTime <= comboThreshold)
        {
            if(comboIndex < comboCoinSounds.Length)
            {
                coinAudioSource.PlayOneShot(comboCoinSounds[comboIndex]);
                comboIndex++;
            }
            else
            {
                coinAudioSource.PlayOneShot(comboCoinSounds[comboCoinSounds.Length - 1]);
                comboIndex = 0;
            }
        }
        else
        {
            coinAudioSource.PlayOneShot(defaultCoinSound);
            comboIndex = 0;
        }

        lastCoinPickupTime = currentTime;
    }

public void PlayBackgroundMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("No AudioClip assigned for background music!");
            return;
        }

        // Loop the background music
        backgroundMusic.clip = clip;
        backgroundMusic.loop = true;
        backgroundMusic.Play();
    }
}



