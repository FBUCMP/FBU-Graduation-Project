using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public int itemID;
    public int itemValue;
    public bool isActivated;

    public virtual void Activate(GameObject parent)
    {
        if (isActivated)
        {
            Debug.Log("Item Already Activated");
            return;
        }
        isActivated = true;

        Debug.Log("Item Activated");
    }
}
