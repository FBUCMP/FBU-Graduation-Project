using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HealthItem", menuName = "Items/HealthItem")]
public class HealthItem : Item
{
    public override void Activate(GameObject parent)
    {
        // Increase the player's health
    }

    public override string DescriptionUpdate()
    {
        itemDescription = $"Increases the health by {rarityEffects[rarity]}";
        return itemDescription;
    }
}
