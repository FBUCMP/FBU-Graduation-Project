using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private PlayerGunSelector GunSelector;
    [SerializeField]
    private bool autoReload = true;
    [SerializeField]
    private Transform gunPivot;
    public bool isStopped;
    private PlayerMovementNew playerMovement;
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
        if (!isStopped)
        {
            GunSelector.ActiveGun.Tick(Input.GetMouseButton(0) && GunSelector.ActiveGun != null && Application.isFocused, gunPivot);
            
        }
        if (ShouldAutoReload() || ShouldManualReload())
        {
            // play animations here in the future
            GunSelector.ActiveGun.StartReloading();
            GunSelector.ActiveGun.ammoConfig.Reload();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GunSelector.PickupGun(GunSelector.Guns[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GunSelector.PickupGun(GunSelector.Guns[1]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GunSelector.PickupGun(GunSelector.Guns[2]);
        }
    }
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovementNew>();
        playerMovement.OnFlipped += FlipGun;
    }
    
    private void FlipGun()
    {
        GunSelector.ActiveGun.FlipGun();
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