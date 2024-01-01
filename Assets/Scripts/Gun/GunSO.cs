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
    private ObjectPool<TrailRenderer> trailPool; // bullet trail

    public void Spawn(Transform Parent, MonoBehaviour activeMonoBehaviour)
    {
        this.activeMonoBehaviour = activeMonoBehaviour;
        lastShootTime = 0; // Time.time = time since game started. lastShootTime = time the shot was fired
        trailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        model = Instantiate(modelPrefab);
        model.transform.SetParent(Parent, false);
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


    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit2D Hit)
    {
        TrailRenderer instance = trailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = StartPoint;
        yield return null; // wait one frame to avoid position carry-over from last frame if reused

        instance.emitting = true;

        float distance = Vector3.Distance(StartPoint, EndPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                StartPoint,
                EndPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance))
            );
            remainingDistance -= trailConfig.simulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = EndPoint;

        if (Hit.collider != null)
        {
            // surface manager is not implemented
            /*
            SurfaceManager.Instance.HandleImpact(
                Hit.transform.gameObject,
                EndPoint,
                Hit.normal,
                ImpactType,
                0
            );
            */
            if(Hit.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(damageConfig.GetDamage(distance)); // distance is used to calculate damage
            }
        }

        yield return new WaitForSeconds(trailConfig.duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        trailPool.Release(instance);
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

}