using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunModifierApplier : MonoBehaviour
{
    [SerializeField]
    private PlayerGunSelector GunSelector;
    // modifiers for buffs and debuffs
    private void Start()
    {
        DamageModifier damageModifier = new()
        {
            amount = 1.5f, // 50% more damage
            attributeName = "damageConfig/damageCurve"
        };
         
        damageModifier.Apply(GunSelector.ActiveGun);

        Vector3Modifier spreadModifier = new()
        {
            amount = Vector3.one / 2, // no spread
            attributeName = "shootConfig/spread"
        };
        
        spreadModifier.Apply(GunSelector.ActiveGun);
    }
}