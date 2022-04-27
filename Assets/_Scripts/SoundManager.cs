using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    [Header("Sound Info")]
    [SerializeField] private AudioSource audioSource;

    [Header("Sound Clips Info")]
    public AudioClip clickAudioClip;
    public AudioClip flagAudioClip;
    public AudioClip unflagAudioClip;
    public AudioClip wrongAudioClip;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    public void PlayEffect(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
