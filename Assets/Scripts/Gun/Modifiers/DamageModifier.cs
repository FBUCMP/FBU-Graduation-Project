using System.Reflection;
using static UnityEngine.ParticleSystem;


public class DamageModifier : AbstractValueModifier<float>
{
    public override void Apply(GunSO Gun)
    {
        try
        {
            MinMaxCurve damageCurve = GetAttribute<MinMaxCurve>(
                Gun,
                out object targetObject,
                out FieldInfo field
            );

            switch (damageCurve.mode)
            {
                case UnityEngine.ParticleSystemCurveMode.TwoConstants:
                    damageCurve.constantMin *= amount;
                    damageCurve.constantMax *= amount;
                    break;
                case UnityEngine.ParticleSystemCurveMode.TwoCurves:
                    damageCurve.curveMultiplier *= amount;
                    break;
                case UnityEngine.ParticleSystemCurveMode.Curve:
                    damageCurve.curveMultiplier *= amount;
                    break;
                case UnityEngine.ParticleSystemCurveMode.Constant:
                    damageCurve.constant *= amount;
                    break;
            }

            field.SetValue(targetObject, damageCurve);
        }
        catch (InvalidPathSpecifiedException) { } 
    }
}