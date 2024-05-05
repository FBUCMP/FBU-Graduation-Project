using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [field: SerializeField]
    public Vector3 spawnLocation
    {
        get; private set;
    }

    public float delayedDisableTime = 2f;

    public delegate void CollisionEvent(Bullet bullet, Collision2D collision);
    public event CollisionEvent OnCollsion;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Spawn(Vector3 spawnForce)
    {
        spawnLocation = transform.position;
        transform.right = spawnForce.normalized;
        rb.AddForce(spawnForce); // spawn with force
        StartCoroutine(DelayedDisable(delayedDisableTime));
    }

    private IEnumerator DelayedDisable(float time)
    {
        yield return new WaitForSeconds(time);
        OnCollisionEnter2D(null); // if doesnt collide at all
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;
        if (collision.gameObject.tag != "Player")
        {
            OnCollsion?.Invoke(this, collision);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        OnCollsion = null; // clear
    }
}