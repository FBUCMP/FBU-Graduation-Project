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
            playerHealth.Heal(playerHealth.maxHealth); // heal player to the max health 
            Debug.Log("NewMaxHealth: " + playerHealth.maxHealth);
            Debug.Log("NewHealth: " + playerHealth.currentHealth);
        }
    }

    public override string DescriptionUpdate()
    {
        itemDescription = $"Increases the health by {rarityEffects[rarity]} and restores health";
        return itemDescription;
    }
}
