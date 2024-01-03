using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunSO : ScriptableObject
{
    // spawn - shoot - create trail - play trail 
    
    public GunType type;
    public string Name;
    public GameObject modelPrefab;// gun model prefab
    public Vector3 spawnPoint;
    public Vector3 spawnRotation;

    public DamageConfigurationSO damageConfig;
    public AmmoConfigurationSO ammoConfig;
    public ShootConfigurationSO shootConfig;
    public TrailConfigurationSO trailConfig;

    public AudioConfigurationSO audioConfig;

    private MonoBehaviour activeMonoBehaviour; // bullet
    private GameObject model;
    private AudioSource shootingAudioSource;

    private float lastShootTime;
    private ParticleSystem shootSystem; // gun muzzle flash

    private ObjectPool<Bullet> bulletPool; // projectile bullet pool
    private ObjectPool<TrailRenderer> trailPool; // raycast bullet trail

    public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
        this.activeMonoBehaviour = activeMonoBehaviour;
        lastShootTime = 0; // Time.time = time since game started. lastShootTime = time the shot was fired
        trailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        if (!shootConfig.isHitScan)
        {
            bulletPool = new ObjectPool<Bullet>(CreateBullet);
        }

        model = Instantiate(modelPrefab);
        model.transform.SetParent(parent, false);
        model.transform.localPosition = spawnPoint;
        model.transform.localRotation = Quaternion.Euler(spawnRotation);

        shootSystem = model.GetComponentInChildren<ParticleSystem>();
        shootingAudioSource = model.GetComponent<AudioSource>();
    }

    public void TryToShoot()
    {
        if (Time.time > shootConfig.fireRate + lastShootTime)
        {
            lastShootTime = Time.time;

            if (ammoConfig.currentClipAmmo == 0)
            {
                // handled the no ammo sound here because of shoot timer
                audioConfig.PlayOutOfAmmoClip(shootingAudioSource);
                return;
            }


            shootSystem.Play();
            audioConfig.PlayShootingClip(shootingAudioSource); // , ammoConfig.currentClipAmmo == 1

            // controlled random amount of bullet spread
            Vector3 spreadAmount = new Vector3(
                    Random.Range(
                        -shootConfig.spread.x,
                        shootConfig.spread.x
                    ),
                    Random.Range(
                        -shootConfig.spread.y,
                        shootConfig.spread.y
                    ),
                    Random.Range(
                        -shootConfig.spread.z,
                        shootConfig.spread.z
                    )
                );
            model.transform.up += model.transform.TransformDirection(spreadAmount);
            Vector3 shootDirection = shootSystem.transform.forward + spreadAmount;                 
            shootDirection.Normalize();

            ammoConfig.currentClipAmmo--;

            if (shootConfig.isHitScan)
            {
                HitScanShoot(shootDirection);
            }
            else
            {
                ProjectileShoot(shootDirection);
            }
        }
    }
    private void HitScanShoot(Vector3 shootDirection) // raycast shoot 
    {

        RaycastHit2D hit = Physics2D.Raycast(
                shootSystem.transform.position,
                shootDirection,
                float.MaxValue,
                shootConfig.hitMask
            );
        if (hit.collider != null)
        {
            activeMonoBehaviour.StartCoroutine(
                PlayTrail(
                    shootSystem.transform.position,
                    hit.point,
                    hit
                )
            );
        }
        else
        {
            activeMonoBehaviour.StartCoroutine(
                PlayTrail(
                    shootSystem.transform.position,
                    shootSystem.transform.position + (shootDirection * trailConfig.missDistance),
                    new RaycastHit2D()
                )
            );
        }
    }
    private void ProjectileShoot(Vector3 shootDirection)
    {
        Bullet bullet = bulletPool.Get();
        bullet.gameObject.SetActive(true);
        bullet.OnCollsion += HandleBulletCollision;
        bullet.transform.position = shootSystem.transform.position;
        bullet.Spawn(shootDirection * shootConfig.bulletSpawnForce);
        TrailRenderer trail = trailPool.Get();
        if (trail != null)
        {
            trail.transform.SetParent(bullet.transform, false); // set trail as child of bullet and set its local position to 0 to make it same as the bullet
            trail.transform.localPosition = Vector3.zero;
            trail.emitting = true;
            trail.gameObject.SetActive(true);
        }
    }
    public void Tick(bool shootRequest)
    {
        // try to recover from recoil every frame. Shoot() recoil > recovery speed
        model.transform.localRotation = Quaternion.Lerp(
            model.transform.localRotation,
            Quaternion.Euler(spawnRotation),
            Time.deltaTime * shootConfig.recoilRecoverySpeed);
        if (shootRequest)
        {
            TryToShoot();            
        }
    }

    public void StartReloading()
    {
        audioConfig.PlayReloadClip(shootingAudioSource);
    }

  
    public void EndReload()
    {
        ammoConfig.Reload(); // actual ammo reload
    }


    private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit2D hit) // only hitscan uses this
    {
        TrailRenderer instance = trailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = startPoint;
        yield return null; // wait one frame to avoid position carry-over from last frame if reused

        instance.emitting = true;

        float distance = Vector3.Distance(startPoint, endPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                startPoint,
                endPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance))
            );
            remainingDistance -= trailConfig.simulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = endPoint;

        if (hit.collider != null)
        {
            HandleBulletImpact(distance, endPoint, hit.normal, hit.collider);
        }

        yield return new WaitForSeconds(trailConfig.duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        trailPool.Release(instance);
    }
    private void HandleBulletCollision(Bullet bullet, Collision2D collision) // gets subscribed to projectile bullet's collision event
    {
        
        TrailRenderer trail = bullet.GetComponentInChildren<TrailRenderer>();
        if (trail != null)
        {
            // bullet is collided no need for trail anymore
            trail.transform.SetParent(null, true);
            activeMonoBehaviour.StartCoroutine(DelayedDisableTrail(trail));
        }
        bullet.gameObject.SetActive(false);
        bulletPool.Release(bullet);
        if (collision != null)
        {
            ContactPoint2D contactPoint = collision.GetContact(0);
            HandleBulletImpact(
                Vector3.Distance(contactPoint.point, bullet.spawnLocation),
                contactPoint.point,
                contactPoint.normal,
                contactPoint.collider);
        }
    }
    private void HandleBulletImpact(
        float distanceTraveled,
        Vector3 hitLocation,
        Vector3 hitNormal,
        Collider2D hitCollider)
    {
        /*
        SurfaceManager.Instance.HandleImpact(
                hitCollider.gameObject,
                hitLocation,
                hitNormal,
                ImpactType,
                0
            );
        */
        
        if (hitCollider.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log("Damageable got hit");
            damageable.TakeDamage(damageConfig.GetDamage(distanceTraveled));
        }
    }
    private IEnumerator DelayedDisableTrail(TrailRenderer trail)
    {
        yield return new WaitForSeconds(trailConfig.duration);
        yield return null; // wait for one more frame
        trail.emitting = false;
        trail.gameObject.SetActive(false);
        trailPool.Release(trail);
    }
    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = trailConfig.color;
        trail.material = trailConfig.material;
        trail.widthCurve = trailConfig.widthCurve;
        trail.time = trailConfig.duration;
        trail.minVertexDistance = trailConfig.minVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }
    private Bullet CreateBullet()
    {
        return Instantiate(shootConfig.bulletPrefab);
    }
}