using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private LayerMask shootBlockMask;
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
        
        if (!isStopped)
        {
            GunSelector.ActiveGun.Tick(GetRequest(), gunPivot);
            
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
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GunSelector.PickupGun(GunSelector.Guns[3]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GunSelector.PickupGun(GunSelector.Guns[4]);
        }
    }
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovementNew>();
        playerMovement.OnFlipped += FlipGun;
    }
    private bool GetRequest()
    {
        return Input.GetMouseButton(0) && GunSelector.ActiveGun != null && Application.isFocused && !IsGunInWall();
    }
    private bool IsGunInWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(gunPivot.position, GunSelector.ActiveGun.model.transform.position - gunPivot.position , 2f, shootBlockMask);
        Debug.DrawRay(gunPivot.position, (GunSelector.ActiveGun.model.transform.position - gunPivot.position) * 2, Color.blue, .01f);
        return hit.collider != null;
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