using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "JumpItem", menuName = "Items/JumpItem")]
public class JumpItem : Item
{
    public override void Activate(GameObject parent)
    {
        if (parent.TryGetComponent(out PlayerMovementNew playerMovement))
        {
            playerMovement.jumpHeight += rarityEffects[rarity]; // increase jump height
            Debug.Log("NewJumpHeight: " + playerMovement.jumpHeight);
        }
    }

    public override string DescriptionUpdate()
    {
        itemDescription = $"Increases the jump height by {rarityEffects[rarity]}";
        return itemDescription;
    }
}
