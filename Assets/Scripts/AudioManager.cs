using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("---------Audio Source----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------Audio Clip----------")]
    public AudioClip background;
    public AudioClip collect;
    public AudioClip death;
    public AudioClip win;
    public AudioClip spawn;
    public AudioClip break_box;
    public AudioClip tap;
    public AudioClip nokey;
    public AudioClip rhinocollision;
    public AudioClip run;
    public AudioClip slimedead;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayTapSound()
    {
        if (tap != null)
        {
            PlaySFX(tap);
        }
    }
}
