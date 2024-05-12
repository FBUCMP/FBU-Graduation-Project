using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelect : MonoBehaviour
{
    // Card Template
    public GameObject cardTemplate;
    public Transform container;
    [SerializeField] private int selectedItemNumber = 0;

    // attached to the itemselect manager
    public List<Item> allItems; // all the items in the game

    // Card Objects UI
    //public List<GameObject> cardTemplates;


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
        //itemHolder.AddItem(allItems[0]); // temporary
        
        
        // DEBUG TEST 
        //ActivatePopup(0);
    }
    public List<Item> SelectRandomItems(int n) // select n different random items from allItems
    {
        List<Item> selectedItems = new List<Item>();
        for (int i = 0; i < n; i++)
        {
            int randomIndex = Random.Range(0, allItems.Count);
            //Debug.Log("Random Index: " + randomIndex);
            Item item = allItems[randomIndex];
            if (!selectedItems.Contains(item)) // if item is not already in selecteditems
            {
                selectedItems.Add(item);
            }
            else
            {
                while (selectedItems.Contains(item))
                {
                    //Debug.Log("Random Index Yeniden Seciliyor: " + randomIndex);
                    randomIndex = Random.Range(0, allItems.Count);
                    item = allItems[randomIndex];
                }
                selectedItems.Add(item);
            }

        }
        return selectedItems;
    }

    public void ActivatePopup(int roomIndex) // shoud be called when room is cleared by itemselect manager object
    {
        //Debug.Log("ActivePopup:");
        // show items in popup
        GameManagerScript.Instance.upgradeScreenUI.SetActive(true);

        // Create Cards
        CreateCards();
    }

    void CreateCards()
    {
        //Debug.Log("CreateCard activated with selectedItemNumber: " + selectedItemNumber);
        List<Item> selectedItems = SelectRandomItems(selectedItemNumber);
        
        for (int i = 0; i < selectedItemNumber; i++)
        {
            //Debug.Log("Creating card " + i);
            GameObject newCard = Instantiate(cardTemplate, container);
            
            // Display
            CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
            if (cardDisplay != null && i < selectedItems.Count)
            {
                //Debug.Log("Displayed Card: " + i);
                // Display the item's information on the card
                cardDisplay.DisplayCard(selectedItems[i]);
            }

        }
    }

}
