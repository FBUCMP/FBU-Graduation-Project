using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private PlayerGunSelector GunSelector;

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
    }
}