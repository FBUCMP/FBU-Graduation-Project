using UnityEngine;


[CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/Shoot Configuration", order = 2)]
public class ShootConfigurationSO : ScriptableObject, System.ICloneable
{
    public bool isHitScan = true;
    public Bullet bulletPrefab;
    public float bulletSpawnForce = 100f;
    public LayerMask hitMask;
    public Vector3 spread = new Vector3(0.1f, 0.1f, 0f);
    public float fireRate = 0.25f;
    public int bulletsPerShot = 1;
    public float recoilRecoverySpeed = 1f;

    // ICloneable interface and Clone() for using copies of scriptable objects instead
    public object Clone()
    {
        ShootConfigurationSO config = CreateInstance<ShootConfigurationSO>();
        Utilities.CopyValues(this, config); // Utilities is a custom class
        return config;
    }
}
