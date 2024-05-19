using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardButtonImage : MonoBehaviour
{
    // attached to the card button in the item select (upgrade) screen canvas
    public Image backgroundImage; // Kart arka planý için Image bileþeni

    public Sprite whiteCardSprite; // Beyaz kart görseli
    public Sprite greenCardSprite; // Yeþil kart görseli
    public Sprite blueCardSprite; // Mavi kart görseli
    public Sprite purpleCardSprite; // Mor kart görseli
    public Sprite redCardSprite; // Kýrmýzý kart görseli
    public void SetCardColor(string color)
    {
        // Renk ismine göre arka plan görselini ayarla
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
