using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "FireRateItem", menuName = "Items/FireRateItem")]
public class FireRateItem : Item
{
    private PlayerGunSelector gunSelect;
    public override void Activate(GameObject parent)
    {
        if (parent.TryGetComponent(out PlayerGunSelector gunSelector))
        {
            gunSelect = gunSelector;
            gunSelect.OnGunPicked += Apply;
            Apply();


        }
    }

    private void Apply()
    {
        gunSelect.ApplyModifiers(new IModifier[]
        {
                new FireRateModifier()
                {
                    amount =  1f + (rarityEffects[rarity]/100f),
                    attributeName = "shootConfig/fireRate"
                }
        });

        Debug.Log("NewFireRate: " + gunSelect.ActiveGun.shootConfig.fireRate);
    }

    public override string DescriptionUpdate()
    {
        itemDescription = $"Increases the fire rate by %{rarityEffects[rarity]}";
        return itemDescription;
    }
}
