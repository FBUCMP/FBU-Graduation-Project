using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SpeedItem", menuName = "Items/SpeedItem")]
public class SpeedItem : Item
{
    public override void Activate(GameObject parent)
    {
        if (parent.TryGetComponent(out PlayerMovementNew playerMovement))
        {
            playerMovement.speed += rarityEffects[rarity]; // increase the player's speed
            Debug.Log("NewSpeed: " + playerMovement.speed);
        }
    }

    public override string DescriptionUpdate()
    {
        itemDescription = $"Increases the speed by {rarityEffects[rarity]}";
        return itemDescription;
    }
}

