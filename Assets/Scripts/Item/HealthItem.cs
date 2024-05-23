using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HealthItem", menuName = "Items/HealthItem")]
public class HealthItem : Item
{
    public override void Activate(GameObject parent)
    {
        if (parent.TryGetComponent(out HPlayer playerHealth))
        {
            playerHealth.IncreaseMaxHealth(rarityEffects[rarity]); // increase the player's max health
            playerHealth.Heal(rarityEffects[rarity]); // heal player the same amount as the max health increase
        }
    }

    public override string DescriptionUpdate()
    {
        itemDescription = $"Increases the health by {rarityEffects[rarity]}";
        return itemDescription;
    }
}
