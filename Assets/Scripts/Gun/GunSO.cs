using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunSO : ScriptableObject, System.ICloneable
{
    // spawn - shoot - create trail - play trail 
    public ImpactType impactType;
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
    public KnockbackConfigurationSO knockbackConfig;

    private MonoBehaviour activeMonoBehaviour; // bullet
    [HideInInspector] public GameObject model;
    private AudioSource shootingAudioSource;

    public ICollisionHandler[] bulletImpactEffects = new ICollisionHandler[0];

    private float lastShootTime;
    private ParticleSystem shootSystem; // gun muzzle flash

    private ObjectPool<Bullet> bulletPool; // projectile bullet pool
    private ObjectPool<TrailRenderer> trailPool; // raycast bullet trail


    public delegate void ShootEvent(float power);
    public event ShootEvent OnShoot;

    public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
        this.activeMonoBehaviour = activeMonoBehaviour;
        /*
        lastShootTime = 0; // Time.time = time since game started. lastShootTime = time the shot was fired
        ammoConfig.currentClipAmmo = ammoConfig.clipSize; // set current ammo to max ammo
        */

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

        if (knockbackConfig.knockbackStrength > 0)
        {
            ICollisionHandler[] currentHandlers = bulletImpactEffects;
            bulletImpactEffects = new ICollisionHandler[currentHandlers.Length + 1];
            System.Array.Copy(currentHandlers, bulletImpactEffects, currentHandlers.Length);
            bulletImpactEffects[^1] = new Knockback();

        }
    }

    public void Despawn()
    {
       
        model.gameObject.SetActive(false);
        Destroy(model);
        trailPool.Clear();
        if (bulletPool != null)
        {
            bulletPool.Clear();
        }

        shootingAudioSource = null;
        shootSystem = null;
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

            ammoConfig.currentClipAmmo--;
            for (int i = 0; i < shootConfig.bulletsPerShot; i++)
            {
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


                if (shootConfig.isHitScan)
                {
                    HitScanShoot(shootDirection);
                }
                else
                {
                    ProjectileShoot(shootDirection);
                }

                OnShoot?.Invoke(shootConfig.bulletSpawnForce);
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
    public void Tick(bool shootRequest, Transform gunPivot)
    {
        // try to recover from recoil every frame. Shoot() recoil > recovery speed
        model.transform.localRotation = Quaternion.Lerp(
            model.transform.localRotation,
            Quaternion.Euler(spawnRotation),
            Time.deltaTime * shootConfig.recoilRecoverySpeed);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - (Vector2)gunPivot.position;
        float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
        /*
         *              |
         *             90
         *              |
         * <--180/-180--o--------0-->
         *              |
         *            -90
         *              |
         */
        // if angle is 0 to 90 meaning looking up-right, higher the gunPivot's y position
        // if angle is 0 to -90  meaning looking down-right, lower the gunPivot's y position and higher the x position slightly
        Vector3 modifiedGunPivot = gunPivot.position;
        float sinAng = Mathf.Sin(angle * Mathf.Deg2Rad); // distance from x axis
        float distX = 0.4f;
        float distY = 0.2f;
        if (direction.y > 0) // up
        {
            modifiedGunPivot = new Vector3(gunPivot.position.x + distX * Mathf.Abs(sinAng) * Mathf.Sign(direction.x), gunPivot.position.y + distY * sinAng, gunPivot.position.z);
        }
        else // down
        {
            modifiedGunPivot = new Vector3(gunPivot.position.x + distX * Mathf.Abs(sinAng) * Mathf.Sign(direction.x), gunPivot.position.y + distY * sinAng, gunPivot.position.z);
        }
        Debug.DrawLine(gunPivot.position, modifiedGunPivot, Color.yellow);
        model.transform.position = modifiedGunPivot + Quaternion.Euler(0f, 0f, angle) * new Vector3(spawnPoint.x, 0f, 0f); 
        model.transform.rotation = Quaternion.Euler(spawnRotation.x, spawnRotation.y, spawnRotation.z + angle);
        //model.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        if (shootRequest)
        {
            Debug.DrawLine(gunPivot.position, mousePos, Color.red);
            TryToShoot();            
        }
    }

    public void FlipGun()
    {
        model.transform.localScale = new Vector3(-model.transform.localScale.x, -model.transform.localScale.y, model.transform.localScale.z);
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
        if (SurfaceManager.Instance != null)
        {
            SurfaceManager.Instance.HandleImpact(
                            hitCollider.gameObject,
                            hitLocation,
                            hitNormal,
                            impactType,
                            0
                        );
        }
        
        
        
        if (hitCollider.TryGetComponent(out IDamageable damageable))
        {
            if (damageConfig.GetDamage(distanceTraveled) > 0)
            {
                damageable.TakeDamage(damageConfig.GetDamage(distanceTraveled), hitLocation, 1);
            }
        }
        foreach (ICollisionHandler handler in bulletImpactEffects)
        {
            handler.HandleImpact(hitCollider, hitLocation, hitNormal, distanceTraveled, this);
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

    // ICloneable interface and Clone() for using copies of scriptable objects instead
    public object Clone()
    {
        GunSO config = CreateInstance<GunSO>();
        
        config.impactType = impactType;
        config.type = type;
        config.Name = Name;
        config.name = name; // built-in name property
        config.modelPrefab = modelPrefab;
        config.spawnPoint = spawnPoint;
        config.spawnRotation = spawnRotation;
        config.damageConfig = damageConfig.Clone() as DamageConfigurationSO;
        config.ammoConfig = ammoConfig.Clone() as AmmoConfigurationSO;
        config.shootConfig = shootConfig.Clone() as ShootConfigurationSO;
        config.trailConfig = trailConfig.Clone() as TrailConfigurationSO;
        config.audioConfig = audioConfig.Clone() as AudioConfigurationSO;
        config.knockbackConfig = knockbackConfig.Clone() as KnockbackConfigurationSO;

        return config;
    }
}