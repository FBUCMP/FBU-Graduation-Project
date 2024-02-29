using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    private GameObject player;
    private bool hasLineOfSight = false;

    private Collider2D enemyCollider;
    void Start()
    {
        enemyCollider = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (hasLineOfSight)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = player.transform.position - transform.position;
        RaycastHit2D[] results = new RaycastHit2D[1];
        int hitAmount = enemyCollider.Raycast(direction, results);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
        RaycastHit2D hit = results[0];
        if (hit.collider != null)
        {
            hasLineOfSight = hit.collider.CompareTag(player.tag);
            if (hasLineOfSight)
            {
                Debug.DrawRay(transform.position, direction, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, direction, Color.red);
            }
        }
    }
}