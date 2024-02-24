using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu]
public class DashAbility : AbilityManager
{
    public float dashVelocity;

    public override void Activate(GameObject parent)
    {
        PlayerMovementNew movement = parent.GetComponent<PlayerMovementNew>();
        Rigidbody2D rigidbody = parent.GetComponent<Rigidbody2D>();

        //rigidbody.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        //rigidbody.velocity = movement.movementInput.normalized * dashVelocity;

        
    }

}
