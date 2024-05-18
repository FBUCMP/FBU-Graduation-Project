using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "JumpItem", menuName = "Items/JumpItem")]
public class JumpItem : Item
{
    public override void Activate(GameObject parent)
    {
        // Increase the player's jump power
    }

    public override string DescriptionUpdate()
    {
        itemDescription = $"Increases the jump force by {rarityEffects[rarity]}";
        return itemDescription;
    }
}
