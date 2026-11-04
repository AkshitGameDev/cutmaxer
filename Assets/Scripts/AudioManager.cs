using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public AudioClip bgm;
    public AudioClip knifeThrow;
    public AudioClip knifeHit;
     public AudioClip knifeMiss;
    public AudioClip gameOver;

    

    static AudioManager instance;
    AudioManager()
    {
        instance = this;
        
    }

    void playSound(AudioClip clip)
    {
        
        audioSource.PlayOneShot(clip);
    }

    void playBGM()
    {
        audioSource.clip = bgm;
        audioSource.loop = true;
        audioSource.Play();
    }

    void muteBGM()
    {
        audioSource.Stop();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playBGM();
    }
}
