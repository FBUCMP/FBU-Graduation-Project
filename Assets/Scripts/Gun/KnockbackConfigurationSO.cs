using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Knockback Config", menuName = "Guns/Knockback Configuration", order = 8)]
public class KnockbackConfigurationSO : ScriptableObject, System.ICloneable
{
    public float knockbackStrength = 250f;
    public ParticleSystem.MinMaxCurve distanceFalloff;
    public float maxKnockbackTime = 1f;

    public Vector3 GetKnockbackStrength(Vector3 direction, float distance)
    {
        return knockbackStrength * distanceFalloff.Evaluate(distance) * direction.normalized;
    }
    public object Clone()
    {
        KnockbackConfigurationSO clone = CreateInstance<KnockbackConfigurationSO>();
        Utilities.CopyValues(this, clone);
        return clone;
    }
}
