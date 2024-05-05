using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : ICollisionHandler
{
    public float radius = 3f;
    public AnimationCurve damageFalloff;
    public int baseDamage = 10;
    public int maxEnemiesAffected= 10;

    private Collider2D[] hitObjects;
    

    public Explode(float radius, AnimationCurve damageFalloff, int baseDamage, int maxEnemiesAffected)
    {
        this.radius = radius;
        this.damageFalloff = damageFalloff;
        this.baseDamage = baseDamage;
        this.maxEnemiesAffected = maxEnemiesAffected;
        hitObjects = new Collider2D[maxEnemiesAffected];
    }
    public void HandleImpact(Collider2D ImpactedObject, Vector3 HitPosition, Vector3 HitNormal, float DistanceTravelled,GunSO Gun)
    {
        int hits = Physics2D.OverlapCircleNonAlloc(HitPosition, radius, hitObjects, Gun.shootConfig.hitMask);
        for (int i = 0; i < hits; i++)
        {
            if (hitObjects[i].TryGetComponent(out IDamageable damagable))
            {
                float distance = Vector3.Distance(hitObjects[i].ClosestPoint(HitPosition), HitPosition); /* hitObjects[i].transform.position might cause an error because it gets center
                                                                                                   *try hitObjects[i].ClosestPoint(HitPosition). if that causes an error its because
                                                                                                   the collider is not primitive */
                //Debug.Log($"hitpos: {HitPosition}");
                //Debug.Log($"hitobj.pos: {hitObjects[i].transform.position}");
                //Debug.Log($"closestpoint: {hitObjects[i].ClosestPoint(HitPosition)}");
                //Debug.Log("exploded: " + hitObjects[i].name);

                damagable.TakeDamage(Mathf.CeilToInt(baseDamage * damageFalloff.Evaluate(distance / radius)), HitPosition, radius); // damage according to distance
            }
            if (hitObjects[i].TryGetComponent(out IKnockbackable knockbackable))
            {
                Vector3 knockbackForce = Gun.knockbackConfig.GetKnockbackStrength((hitObjects[i].transform.position - HitPosition), Vector2.Distance(HitPosition, hitObjects[i].transform.position));
                float knockbackTime = Gun.knockbackConfig.maxKnockbackTime;
                knockbackable.GetKnockedBack(knockbackForce, knockbackTime);
            }
            

        }
    }

    
}
