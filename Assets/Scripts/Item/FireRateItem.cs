using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "FireRateItem", menuName = "Items/FireRateItem")]
public class FireRateItem : Item
{
    public override void Activate(GameObject parent)
    {
        // Increase the player's fire rate
    }

    public override string DescriptionUpdate()
    {
        itemDescription = $"Increases the fire rate by {rarityEffects[rarity]}";
        return itemDescription;
    }
}
