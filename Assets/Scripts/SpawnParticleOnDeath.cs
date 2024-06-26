using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(IDamageable))]
// spawns particles on death event
public class SpawnParticleSystemOnDeath : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem deathParticle;
    [SerializeField]
    private AudioClip deathSound;
    public IDamageable damageable;

    private void Awake()
    {
        damageable = GetComponent<IDamageable>();
    }

    private void OnEnable()
    {
        damageable.OnDeath += Damageable_OnDeath; // subscribe to the OnDeath event set gObject inactive when invoked
    }

    private void OnDisable()
    {
        damageable.OnDeath -= Damageable_OnDeath; // unsubscribe from the OnDeath event
    }
    private void Damageable_OnDeath(Vector3 position)
    {
        Instantiate(deathParticle, position, Quaternion.identity).Play();
        
        EnemyManager.audioSource.PlayOneShot(deathSound);

        //Destroy(gameObject); 
        gameObject.SetActive(false);
    }
}