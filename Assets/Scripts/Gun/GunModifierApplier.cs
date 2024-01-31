using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunModifierApplier : MonoBehaviour
{
    [SerializeField]
    private ImpactType ImpactTypeOverride;
    [SerializeField]
    private PlayerGunSelector GunSelector;
    // modifiers for buffs and debuffs
    private void Start()
    {
        if (GunSelector == null) { return; }
        if (ImpactTypeOverride != null)
        {
            new ImpactTypeModifier
            {
                amount = ImpactTypeOverride
            }.Apply(GunSelector.ActiveGun);

        }

        GunSelector.ActiveGun.bulletImpactEffects = new ICollisionHandler[]
        {
            new Explode(
                1.5f, // radius
                new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(1, 0.25f) }), // damage falloff curve 1 to .25 over the radius
                10, // base damage
                10 // number of objects to apply damage to
                )
        };

        /*
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
        */
    }
}