using UnityEngine;

[CreateAssetMenu(fileName = "Ammo Config", menuName = "Guns/Ammo Configuration", order = 3)]
public class AmmoConfigurationSO : ScriptableObject, System.ICloneable
{
    public int clipSize = 30;
    public int currentClipAmmo = 30;
    public float reloadTime = 1.5f;
    
    public void Reload()
    {
        int maxReloadAmount = clipSize; // reloads the clip from empty to full
        int availableBulletsInCurrentClip = clipSize - currentClipAmmo;
        int reloadAmount = Mathf.Min(maxReloadAmount, availableBulletsInCurrentClip);
        currentClipAmmo += reloadAmount;
        Debug.Log("Reloaded " + reloadAmount + " bullets");
    }

   
    public bool CanReload()
    {
        return currentClipAmmo < clipSize;
    }

    // ICloneable interface and Clone() for using copies of scriptable objects instead
    public object Clone()
    {
        AmmoConfigurationSO config = CreateInstance<AmmoConfigurationSO>();
        Utilities.CopyValues(this, config); // Utilities is a custom class
        return config;
    }
}