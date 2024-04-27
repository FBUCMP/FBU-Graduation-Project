using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName ="Abilities/DashAbility")]
public class DashAbility : AbilityManager
{
    public float dashVelocity;
    public ParticleSystem dashEffect;
    public Vector2 particleOffset;
    private float gravity;
    public override void Activate(GameObject parent)
    {
        
        PlayerMovementNew movement = parent.GetComponent<PlayerMovementNew>();
        gravity = movement.rb.gravityScale;
        movement.isDashing = true;
        movement.rb.gravityScale = 0f;
        movement.rb.velocity = new Vector2(movement.movementInput * dashVelocity, 0f);
        AudioManager.Instance.PlaySFX(soundEffect);
        ParticleSystem newDashEffect = Instantiate(dashEffect, parent.transform.position + new Vector3(particleOffset.x * (movement.isFacingRight ? 1 : -1),
            particleOffset.y, 0f), Quaternion.Euler(0, 180, 0)); //(0, (movement.isFacingRight ? 180 : 0), 0));
        newDashEffect.transform.SetParent(parent.transform);
        // if facing left , flip the particle system x
    }
    public override void BeginCooldown(GameObject parent)
    {
        PlayerMovementNew movement = parent.GetComponent<PlayerMovementNew>();
        movement.rb.gravityScale = gravity;
        movement.isDashing = false;
    }
}
