using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("------- Audio Source ------------")]
    // Sound[] musicSounds, sfxSounds;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;


    [Header("------- Audio Clip ------------")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip dash;
    public AudioClip wallTouch;
    public AudioClip jump;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

}
