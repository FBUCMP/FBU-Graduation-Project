using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField]
    private GunType Gun;
    [SerializeField]
    private Transform GunParent;
    [SerializeField]
    private List<GunSO> Guns;
    [SerializeField]
    private PlayerIK playerIK;

    [Space]
    [Header("Runtime Filled")]
    public GunSO ActiveGun;

    private void Awake()
    {
        // spawn gun
        GunSO gun = Guns.Find(gun => gun.type == Gun);

        if (gun == null)
        {
            Debug.LogError($"No GunSO found for GunType: {gun}");
            return;
        }

        SetupGun(gun);
        
    }
    public void DespawnActiveGun()
    {
        ActiveGun.Despawn();
        Destroy(ActiveGun);
    }
    public void PickupGun(GunSO gun)
    {
        DespawnActiveGun();
        SetupGun(gun);
    }
    private void SetupGun(GunSO gun)
    {
        ActiveGun = gun.Clone() as GunSO;
        ActiveGun.Spawn(GunParent, this);

        playerIK = GetComponent<PlayerIK>();
        playerIK.Setup(GunParent);


    }
}