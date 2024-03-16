using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu]
public class DashAbility : AbilityManager
{
    public float dashVelocity;

    private float gravity;
    public override void Activate(GameObject parent)
    {
        
        PlayerMovementNew movement = parent.GetComponent<PlayerMovementNew>();
        gravity = movement.rb.gravityScale;
        movement.isDashing = true;
        movement.rb.gravityScale = 0f;
        movement.rb.velocity = new Vector2(movement.movementInput * dashVelocity, 0f);
        AudioManager.Instance.PlaySFX(soundEffect);
    }
    public override void BeginCooldown(GameObject parent)
    {
        PlayerMovementNew movement = parent.GetComponent<PlayerMovementNew>();
        movement.rb.gravityScale = gravity;
        movement.isDashing = false;
    }
}
