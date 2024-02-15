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
    public List<GunSO> Guns;
    [SerializeField]
    private PlayerIK playerIK;

    [Space]
    [Header("Runtime Filled")]
    public GunSO ActiveGun;
    public GunSO ActiveBaseGun;

    public delegate void GunPicked();
    public event GunPicked OnGunPicked;
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
        OnGunPicked?.Invoke();
    }
    private void SetupGun(GunSO gun)
    {
        ActiveBaseGun = gun;
        ActiveGun = gun.Clone() as GunSO;

        // if localscale.x is negative(player is looking left) add 180 degrees to z axis of ActiveGun.spawnRotation else do nothing
        // this is to make sure the gun is spawned looking where the player is looking
        ActiveGun.spawnRotation = new Vector3(ActiveGun.spawnRotation.x , ActiveGun.spawnRotation.y , ActiveGun.spawnRotation.z + (Mathf.Sign(transform.localScale.x) < 0 ? 180 : 0));
        ActiveGun.Spawn(GunParent, this);

        playerIK = GetComponent<PlayerIK>();
        playerIK.Setup(GunParent);


    }

    public void ApplyModifiers(IModifier[] Modifiers)
    {
        DespawnActiveGun();
        SetupGun(ActiveBaseGun);

        foreach (IModifier modifier in Modifiers)
        {
            modifier.Apply(ActiveGun);
        }
    }
}