using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "DamageItem", menuName = "Items/DamageItem")]
public class DamageItem : Item
{
    private PlayerGunSelector gunSelect;

    private void Awake()
    {
        if (gunSelect)
        {
            gunSelect.OnGunPicked += Apply;
        }
    }
    public override void Activate(GameObject parent)
    {
        if (parent.TryGetComponent(out PlayerGunSelector gunSelector))
        {
            gunSelect = gunSelector;
            gunSelect.OnGunPicked += Apply;
            Apply();


        }
    }

    private void Apply() // TODO: fix granade launcher's explosion effect being none after applying the damage modifier
    {
        if (gunSelect.ActiveGun.type == GunType.GranadeLauncher)
        {
            return;
        }
        gunSelect.ApplyModifiers(new IModifier[] {
                new DamageModifier() {
                    amount =  1f + (rarityEffects[rarity]/100f),
                    attributeName = "damageConfig/damageCurve"
                }
        });

        switch (gunSelect.ActiveGun.damageConfig.damageCurve.mode)
        {
            case UnityEngine.ParticleSystemCurveMode.TwoConstants:
                Debug.Log("NewDamage: " + gunSelect.ActiveGun.damageConfig.damageCurve.constantMax);
                break;
            case UnityEngine.ParticleSystemCurveMode.TwoCurves:
                Debug.Log("NewDamage: " + gunSelect.ActiveGun.damageConfig.damageCurve.curveMultiplier);
                break;
            case UnityEngine.ParticleSystemCurveMode.Curve:
                Debug.Log("NewDamage: " + gunSelect.ActiveGun.damageConfig.damageCurve.curveMultiplier);
                break;
            case UnityEngine.ParticleSystemCurveMode.Constant:
                Debug.Log("NewDamage: " + gunSelect.ActiveGun.damageConfig.damageCurve.constant);
                break;
        }
    }
    public override string DescriptionUpdate()
    {
        itemDescription = $"Increases the fire damage by %{rarityEffects[rarity]}";
        return itemDescription;
    }

}
