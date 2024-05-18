using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SpeedItem", menuName = "Items/SpeedItem")]
public class SpeedItem : Item
{
    public override void Activate(GameObject parent)
    {
        // Increase the player's speed
    }

    public override string DescriptionUpdate()
    {
        itemDescription = $"Increases the speed by {rarityEffects[rarity]}";
        return itemDescription;
    }
}

