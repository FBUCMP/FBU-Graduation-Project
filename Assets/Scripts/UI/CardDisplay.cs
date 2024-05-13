using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{ 
    public Text nameText;
    public Text descriptionText;
    public Image itemSprite;

    public Image backgroundImage; // Card image

    public Sprite whiteCardSprite; // White card image
    public Sprite greenCardSprite; // Green card image
    public Sprite blueCardSprite; // Blue card image
    public Sprite purpleCardSprite; // Purple card image
    public Sprite redCardSprite; // Red card image

    public void DisplayCard(Item item)
    {
        //Debug.Log("Item Name: " + item.itemName);
        nameText.text = item.itemName;
        descriptionText.text = item.itemDescription;
        itemSprite.sprite = item.itemSprite;

        switch(item.rarity)
        {
            case 0:
                backgroundImage.sprite = whiteCardSprite;
                break;
            case 1:
                backgroundImage.sprite = greenCardSprite;
                break;
            case 2:
                backgroundImage.sprite = blueCardSprite;
                break;
            case 3: 
                backgroundImage.sprite = purpleCardSprite;
                break;
            case 4:
                backgroundImage.sprite = redCardSprite;
                break;
            default:
                break;
        }

    }


}
