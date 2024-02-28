using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class RushAbility : AbilityManager
{
    private float playerSpeed;
    public float speedUpScale = 2f;
    public override void Activate(GameObject parent)
    {
        PlayerMovementNew movement = parent.GetComponent<PlayerMovementNew>();
        playerSpeed = movement.speed;

        movement.speed *= speedUpScale;

    }
    public override void BeginCooldown(GameObject parent)
    {
        PlayerMovementNew movement = parent.GetComponent<PlayerMovementNew>();
        movement.speed = playerSpeed;
    }
}
