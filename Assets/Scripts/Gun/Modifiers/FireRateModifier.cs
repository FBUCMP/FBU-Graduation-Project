using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class FireRateModifier : AbstractValueModifier<float>
{
    public override void Apply(GunSO Gun)
    {
        try
        {
            float fireRate = GetAttribute<float>(
                Gun,
                out object targetObject,
                out FieldInfo field
            );

            fireRate /= amount; // fire rate going down means the player can shoot faster

            field.SetValue(targetObject, fireRate);
        }
        catch (InvalidPathSpecifiedException) { }
    }
}
