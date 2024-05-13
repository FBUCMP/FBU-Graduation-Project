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
        ActivatePopup(0);
    }
    public List<Item> SelectRandomItems(int n) // select n different random items from allItems
    {
        List<Item> selectedItems = new List<Item>();
        for (int i = 0; i < n; i++)
        {
            int randomIndex = Random.Range(0, allItems.Count);
            //Debug.Log("Random Index: " + randomIndex);
            
            float randomRarity = Random.Range(0f, 1f);
            Debug.Log(randomRarity);






            Item item = allItems[randomIndex];
            if (selectedItems.Contains(item)) // if item is not already in selecteditems
            {
                while (selectedItems.Contains(item))
                {
                    //Debug.Log("Random Index Yeniden Seciliyor: " + randomIndex);
                    randomIndex = Random.Range(0, allItems.Count);
                    item = allItems[randomIndex];
                }
            }
            if (randomRarity < 0.4286)
            {
                item.rarity = 0;
            }
            else if(randomRarity < 0.7143)
            {
                item.rarity = 1;
            }
            else if (randomRarity < 0.8929)
            {
                item.rarity = 2;
            }
            else if (randomRarity < 0.9643)
            {
                item.rarity = 3;
            }
            else
            {
                item.rarity = 4;
            }
            selectedItems.Add(item);
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
