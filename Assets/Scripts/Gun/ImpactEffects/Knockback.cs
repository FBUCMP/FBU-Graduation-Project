using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Knockback : ICollisionHandler
{
    public void HandleImpact(Collider2D ImpactedObject, Vector3 HitPosition, Vector3 HitNormal, float DistanceTravelled, GunSO Gun)
    {
        
        if (ImpactedObject.TryGetComponent(out IKnockbackable knockbackable))
        {
            Vector3 knockbackForce = Gun.knockbackConfig.GetKnockbackStrength(-HitNormal, DistanceTravelled);
            float knockbackTime = Gun.knockbackConfig.maxKnockbackTime;
            knockbackable.GetKnockedBack(knockbackForce, knockbackTime);
        }
    }

    
}
