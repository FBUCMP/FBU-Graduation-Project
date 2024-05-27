using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardButtonImage : MonoBehaviour
{
    // attached to the card button in the item select (upgrade) screen canvas
    public Image backgroundImage; // Kart arka plan� i�in Image bile�eni

    public Sprite whiteCardSprite; // Beyaz kart g�rseli
    public Sprite greenCardSprite; // Ye�il kart g�rseli
    public Sprite blueCardSprite; // Mavi kart g�rseli
    public Sprite purpleCardSprite; // Mor kart g�rseli
    public Sprite redCardSprite; // K�rm�z� kart g�rseli
    public void SetCardColor(string color)
    {
        // Renk ismine g�re arka plan g�rselini ayarla
        switch (color.ToLower())
        {
            case "white":
                backgroundImage.sprite = whiteCardSprite;
                break;
            case "green":
                backgroundImage.sprite = greenCardSprite;
                break;
            case "blue":
                backgroundImage.sprite = blueCardSprite;
                break;
            case "purple":
                backgroundImage.sprite = purpleCardSprite;
                break;
            case "red":
                backgroundImage.sprite = redCardSprite;
                break;
            default:
                break;
        }
    }
}
