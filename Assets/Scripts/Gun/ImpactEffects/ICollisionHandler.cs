using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollisionHandler
{
    void HandleImpact(
            Collider2D ImpactedObject,
            Vector3 HitPosition,
            Vector3 HitNormal,
            float DistanceTravelled,
            GunSO Gun
        );
}
