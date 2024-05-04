using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelect : MonoBehaviour
{
    // attached to the itemselect manager
    public List<Item> allItems; // all the items in the game

    private void Awake()
    {
        EnemyManager.OnRoomCleared += ActivatePopup;
    }

    private void OnDestroy()
    {
        EnemyManager.OnRoomCleared -= ActivatePopup;
    }

    private void Start()
    {
        ItemHolder itemHolder = GameObject.FindObjectOfType<ItemHolder>();
        itemHolder.AddItem(allItems[0]); // temporary
    }
    public List<Item> SelectRandomItems(int n) // select n different random items from allItems
    {
        List<Item> selectedItems = new List<Item>();
        int i = 0;
        while (i < n)
        {
            int randomIndex = Random.Range(0, allItems.Count);
            Item item = allItems[randomIndex];
            if (!selectedItems.Contains(item)) // if item is not already in selecteditems
            {
                selectedItems.Add(item);
                i++;
            }
        }
        return selectedItems;
    }

    public void ActivatePopup(int roomIndex) // shoud be called when room is cleared by itemselect manager object
    {
        List<Item> selectedItems = SelectRandomItems(3);

        // show items in popup
    }
}
