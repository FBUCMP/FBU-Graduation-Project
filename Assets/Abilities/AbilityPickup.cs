using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public DashAbility dashAbility;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AbilityHolder abilityHolder = other.GetComponent<AbilityHolder>();
            if (abilityHolder != null)
            {
                abilityHolder.AddAbility(dashAbility);
                Destroy(gameObject); // Objeyi yok et, çünkü oyuncu yeteneði aldý.
            }
        }
    }
}
