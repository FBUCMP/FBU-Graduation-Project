using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewItem", menuName = "Items/White")]
public class White : Item
{
    // 0,1 0,6 -> 0 -> desc bu item 5 g�� vferir -> bu item 10 g�� verir 
    public override string DescriptionUpdate()
    {
        itemDescription = "Bu item sizlere " + rarityEffects[rarity] + " kadar g�� vermektedir!";
        return itemDescription;
    }
    public override void Activate(GameObject parent)
    {
        
    }
}
