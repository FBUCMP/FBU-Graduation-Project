using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelect : MonoBehaviour
{
    public List<Item> allItems;

    private void Awake()
    {
        EnemyManager.OnRoomCleared += ActivatePopup;
    }
    public List<Item> SelectRandomItems(int n)
    {
        List<Item> selectedItems = new List<Item>();
        int i = 0;
        while (i < n)
        {
            int randomIndex = Random.Range(0, allItems.Count);
            Item item = allItems[randomIndex];
            if (!selectedItems.Contains(item))
            {
                selectedItems.Add(item);
                i++;
            }
        }
        return selectedItems;
    }

    public void ActivatePopup(int roomIndex)
    {
        List<Item> selectedItems = SelectRandomItems(3);

        // show items in popup
    }
}
