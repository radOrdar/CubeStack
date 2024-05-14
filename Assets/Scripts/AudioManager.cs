using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip placeSound;
    [SerializeField] private AudioClip perfectSound;
    [SerializeField] private AudioClip lostSound;
    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance != null)
        {
           Destroy(gameObject);
           return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.clip = backgroundMusic;
        _audioSource.Play();
    }

    public void PlayPlace()
    {
        _audioSource.PlayOneShot(placeSound);
    }
    
    public void PlayPerfect()
    {
        _audioSource.PlayOneShot(perfectSound);
    }
    
    public void PlayLost()
    {
        _audioSource.PlayOneShot(lostSound);
    }
}