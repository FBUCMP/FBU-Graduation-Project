using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour // Monobehaviour arastir. Gerekli olup olmadigini, static class olabilir
{   // Player'a atanacak
    // Toplanabilir esyalar icin sayaclar olustur. Temasla artsin, kullandiginda azalsin
    // 32x32 görsel oluþtur
    // Unity item manager arastir


    private int bombCounter;
    private int coinCounter = 0;
    private int keyCounter;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetBombCounter(int amount)
    {

    }


    public void SetCoinCounter(int amount)
    {
        coinCounter = amount;

    }

    public void AddCoin()
    {

        coinCounter++;

        Debug.Log("coincounter" + coinCounter);
    }

    public void SetKeyCounter(int amount)
    {

    }

}