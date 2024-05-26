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
    public bool isInSwitch;
    private bool isReloading;
    private PlayerMovementNew playerMovement;
    private void Update()
    {
        if (!isStopped)
        {
            GunSelector.ActiveGun.Tick(GetRequest(), gunPivot);
            
        }
        if ((ShouldAutoReload() || ShouldManualReload()) && !isReloading)
        {
            // can play animations here in the future

            float reloadTime = GunSelector.ActiveGun.ammoConfig.reloadTime;
            float delay = 0.1f;
            isReloading = true;
            Invoke(nameof(StartGunReload), delay); // delay so it doesn't play the reload sound at the same time as the shooting sound
            Invoke(nameof(EndGunReload), reloadTime-delay);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GunSelector.PickupGun(GunSelector.Guns[0]);
            CancelInvoke(nameof(Reactivate));
            isInSwitch = true;
            Invoke(nameof(Reactivate), 1f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GunSelector.PickupGun(GunSelector.Guns[1]);
            CancelInvoke(nameof(Reactivate));
            isInSwitch = true;
            Invoke(nameof(Reactivate), 1f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GunSelector.PickupGun(GunSelector.Guns[2]);
            CancelInvoke(nameof(Reactivate));
            isInSwitch = true;
            Invoke(nameof(Reactivate), 1f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GunSelector.PickupGun(GunSelector.Guns[3]);
            CancelInvoke(nameof(Reactivate));
            isInSwitch = true;
            Invoke(nameof(Reactivate), 1f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GunSelector.PickupGun(GunSelector.Guns[4]);
            CancelInvoke(nameof(Reactivate));
            isInSwitch = true;
            Invoke(nameof(Reactivate), 1f);
        }
    }
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovementNew>();
        playerMovement.OnFlipped += FlipGun;
    }

    private void StartGunReload()
    {
        isReloading = true;
        GunSelector.ActiveGun.StartReloading();
    }
    private void EndGunReload()
    {
        GunSelector.ActiveGun.EndReloading();
        isReloading = false;
    }
    private bool GetRequest()
    {
        return Input.GetMouseButton(0) && GunSelector.ActiveGun != null && Application.isFocused && !IsGunInWall() && !isInSwitch;
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

    private void Reactivate()
    {
        isInSwitch = false;
    }
}