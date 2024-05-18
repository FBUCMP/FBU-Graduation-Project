using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewItem", menuName = "Items/White")]
public class White : Item
{
    // 0,1 0,6 -> 0 -> desc bu item 5 güç vferir -> bu item 10 güç verir 
    public override string DescriptionUpdate()
    {
        itemDescription = "Bu item sizlere " + rarityEffects[rarity] + " kadar güç vermektedir!";
        return itemDescription;
    }
    public override void Activate(GameObject parent)
    {
        
    }
}
