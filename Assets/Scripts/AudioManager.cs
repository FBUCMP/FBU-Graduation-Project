using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton �rne�i

    [Header("------- Audio Source ------------")]
    [SerializeField] AudioSource musicSource; // M�zik Kayna�� 
    [SerializeField] AudioSource sfxSource; // SFX Kayna��

    [Header("------- Audio Clip ------------")]
    public AudioClip background; // M�zik Dosyalar�n� atamak i�in gereken de�i�ken. 
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
        musicSource.clip = background; // Oyun ba�lar ba�lamaz background m�zi�i oynat�ls�n.
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip); // SFX kayna��n� �al��t�rmam�za yarayan kod par�ac���. 
    }
}
