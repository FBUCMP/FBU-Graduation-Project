using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private PlayerGunSelector GunSelector;
    [SerializeField]
    private bool autoReload = true;

    private void Update()
    {
        // old system for shooting
        /*
        if (Input.GetMouseButton(0) && GunSelector.ActiveGun != null)
        {
            GunSelector.ActiveGun.Shoot();
        }
        */
        // new system for recoil recovery
        GunSelector.ActiveGun.Tick(Input.GetMouseButton(0) && GunSelector.ActiveGun != null && Application.isFocused);
        if (ShouldAutoReload() || ShouldManualReload())
        {
            // play animations here in the future
            GunSelector.ActiveGun.StartReloading();
            GunSelector.ActiveGun.ammoConfig.Reload();
        }

    }

    private bool ShouldAutoReload()
    {
        return autoReload && GunSelector.ActiveGun != null && GunSelector.ActiveGun.ammoConfig.currentClipAmmo == 0 && GunSelector.ActiveGun.ammoConfig.CanReload();
    }
    private bool ShouldManualReload()
    {
        return Input.GetKeyDown(KeyCode.R) && GunSelector.ActiveGun != null && GunSelector.ActiveGun.ammoConfig.CanReload();
    }
}