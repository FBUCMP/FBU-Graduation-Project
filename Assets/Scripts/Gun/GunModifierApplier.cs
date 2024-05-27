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
        AssignModifiers();
        GunSelector.OnGunPicked += AssignModifiers;
        
    }
    void AssignModifiers()
    {
        if (GunSelector == null) { return; }
        if (ImpactTypeOverride != null)
        {
            new ImpactTypeModifier
            {
                amount = ImpactTypeOverride
            }.Apply(GunSelector.ActiveGun);

        }
        if (GunSelector.ActiveGun.type == GunType.GranadeLauncher) // TODO: make automatic instead of manually assing the impact effect for the weopon 
        {
            GunSelector.ActiveGun.bulletImpactEffects = new ICollisionHandler[]
                    {
                        new Explode(
                            5f, // radius
                            new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(1, 0.5f) }), // damage falloff curve 1 to .85 over the radius
                            60, // base damage
                            10 // number of objects to apply damage to
                            )
                    };
        }

    }
}