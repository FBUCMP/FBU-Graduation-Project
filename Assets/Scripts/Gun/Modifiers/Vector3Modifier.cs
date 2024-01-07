using System;
using System.Reflection;
using UnityEngine;


public class Vector3Modifier : AbstractValueModifier<Vector3>
{
    public override void Apply(GunSO Gun)
    {
        try
        {
            Vector3 value = GetAttribute<Vector3>(
                Gun,
                out object targetObject,
                out FieldInfo field
            );
            value = new(
                value.x * amount.x,
                value.y * amount.y,
                value.z * amount.z
            );
            field.SetValue(targetObject, value);
        }
        catch (InvalidPathSpecifiedException) { } 
    }
}
