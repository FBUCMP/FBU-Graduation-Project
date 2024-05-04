using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    // attached to the player

    public List<Item> items; // items that the player has

    public void AddItem(Item newItem) // add item to the player
    {
        foreach (var item in items)
        {
            if (item == newItem)
            {
                return;
            }
        }
        items.Add(newItem);
        Debug.Log("Item Added" + newItem.itemName);
        newItem.Activate(gameObject);
    }

    
}
