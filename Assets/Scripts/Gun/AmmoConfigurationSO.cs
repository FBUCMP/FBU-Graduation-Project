using UnityEngine;

[CreateAssetMenu(fileName = "Ammo Config", menuName = "Guns/Ammo Configuration", order = 3)]
public class AmmoConfigurationSO : ScriptableObject
{
    public int clipSize = 30;
    public int currentClipAmmo = 30;

    
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
}