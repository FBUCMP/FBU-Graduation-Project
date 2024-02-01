using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GunPickup : MonoBehaviour
{
    public GunSO gun;
    [SerializeField]
    private AnimationCurve curve;
    [Range(0,5)]
    public float speed = 1f;
    private Vector2 startPos;
    private void Start()
    {
        startPos = transform.position;
    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, startPos.y + curve.Evaluate((Time.time * speed % 1)), transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerGunSelector gunSelector))
        {
            gunSelector.PickupGun(gun);
            Destroy(gameObject);
        }
    }
}
