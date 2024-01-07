using UnityEngine;

[CreateAssetMenu(fileName = "Audio Config", menuName = "Guns/Audio Configuration", order = 5)]
public class AudioConfigurationSO : ScriptableObject, System.ICloneable
{
    [Range(0, 1f)]
    public float Volume = 1f;

    public AudioClip[] fireClips;
    public AudioClip emptyClip;
    public AudioClip reloadClip;
    //public AudioClip lastBulletClip;

    public void PlayShootingClip(AudioSource AudioSource) // , bool isLastBullet = false
    {
        /*
        if (isLastBullet && lastBulletClip != null)
        {
            AudioSource.PlayOneShot(lastBulletClip, Volume);
        }
        else
        {
            AudioSource.PlayOneShot(fireClips[Random.Range(0, fireClips.Length)], Volume);
        }
        */
        if (fireClips.Length > 0)
        {
            AudioSource.PlayOneShot(fireClips[Random.Range(0, fireClips.Length)], Volume);
        }
    }

    public void PlayOutOfAmmoClip(AudioSource AudioSource)
    {
        if (emptyClip != null)
        {
            AudioSource.PlayOneShot(emptyClip, Volume);
        }
    }

    public void PlayReloadClip(AudioSource AudioSource)
    {
        if (reloadClip != null)
        {
            AudioSource.PlayOneShot(reloadClip, Volume);
        }
    }


    // ICloneable interface and Clone() for using copies of scriptable objects instead
    public object Clone()
    {
        AudioConfigurationSO config = CreateInstance<AudioConfigurationSO>();
        Utilities.CopyValues(this, config); // Utilities is a custom class
        return config;
    }
}