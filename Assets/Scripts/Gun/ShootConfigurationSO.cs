using UnityEngine;


[CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/Shoot Configuration", order = 2)]
public class ShootConfigurationSO : ScriptableObject
{
    public bool isHitScan = true;
    public Bullet bulletPrefab;
    public float bulletSpawnForce = 100f;
    public LayerMask hitMask;
    public Vector3 spread = new Vector3(0.1f, 0.1f, 0f);
    public float fireRate = 0.25f;
    public float recoilRecoverySpeed = 1f;

}
