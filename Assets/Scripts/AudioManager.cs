using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton örneði

    [Header("------- Audio Source ------------")]
    [SerializeField] AudioSource musicSource; // Müzik Kaynaðý 
    [SerializeField] AudioSource sfxSource; // SFX Kaynaðý

    [Header("------- Audio Clip ------------")]
    public AudioClip background; // Müzik Dosyalarýný atamak için gereken deðiþken. 
    public AudioClip death;
    public AudioClip wallTouch;
    public AudioClip jump;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        musicSource.clip = background; // Oyun baþlar baþlamaz background müziði oynatýlsýn.
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip); // SFX kaynaðýný çalýþtýrmamýza yarayan kod parçacýðý. 
    }
}
