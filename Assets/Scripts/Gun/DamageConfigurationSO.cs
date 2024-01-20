using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[CreateAssetMenu(fileName = "Damage Config", menuName = "Guns/Damage Configuration", order = 1)]
public class DamageConfigurationSO : ScriptableObject, System.ICloneable
{
    public MinMaxCurve damageCurve;

    private void Reset() // when you create a new scriptable object, this function will be called
    {
        damageCurve.mode = ParticleSystemCurveMode.Curve;
    }
    
    public int GetDamage(float distance = 0)
    {
        return (int)damageCurve.Evaluate(distance); // add random next to distance if using two constants or two curves
    }

    // ICloneable interface and Clone() for using copies of scriptable objects instead
    public object Clone()
    {
        DamageConfigurationSO config = CreateInstance<DamageConfigurationSO>();
        config.damageCurve = damageCurve;
        return config;
    }
}
