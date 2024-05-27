using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DamageItem", menuName = "Items/DamageItem")]
public class DamageItem : Item
{
    public override void Activate(GameObject parent)
    {
        // Increase the player's damage
    }

    public override string DescriptionUpdate()
    {
        itemDescription = $"Increases the fire damage by {rarityEffects[rarity]}";
        return itemDescription;
    }
}
