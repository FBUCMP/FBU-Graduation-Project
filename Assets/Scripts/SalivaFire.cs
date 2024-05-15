using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalivaFire : MonoBehaviour
{

    public GameObject Saliva; // Saliva objesinin prefab'i
    public GameObject HomingSaliva;
    public Transform salivaSpawnPoint; // Saliva'n�n spawnlanaca�� nokta
    public float salivaSpeed = 500f; // Saliva'n�n f�rlatma h�z�


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(salivaSpawnPoint.position, salivaSpawnPoint.position -salivaSpawnPoint.right*5);
    }



    // F�rlatma i�lemini ba�latan fonksiyon
    public void SpawnSaliva()
    {
        
        // Saliva prefab'�n� spawnlayarak yeni bir Saliva objesi olu�tur
        GameObject newSaliva = Instantiate(Saliva, salivaSpawnPoint.position, salivaSpawnPoint.rotation);

        // Saliva'n�n rigidbody bile�enini al
        Rigidbody2D salivaRigidbody = newSaliva.GetComponent<Rigidbody2D>();

        // E�er Saliva'n�n rigidbody bile�eni varsa
        if (salivaRigidbody != null)
        {
            // Saliva'y� belirli bir h�zda f�rlat
            //salivaRigidbody.velocity = salivaSpawnPoint.forward * salivaSpeed;
            Vector2 dir = new Vector2(-transform.localScale.x + (Random.value*2 - 1f), 0); // random.value * 2 - 1f = -1 ile 1 aras�nda random bir de�er
            salivaRigidbody.AddForce(dir * salivaSpeed);
         
        }
        else
        {
            Debug.LogError("Saliva prefab'�nda Rigidbody bile�eni bulunamad�!");
        }
    }

    public void SpawnHomingSaliva()
    {

        // Saliva prefab'�n� spawnlayarak yeni bir Saliva objesi olu�tur
        GameObject newSaliva = Instantiate(HomingSaliva, salivaSpawnPoint.position, salivaSpawnPoint.rotation);

        Rigidbody2D salivaRigidbody = newSaliva.GetComponent<Rigidbody2D>();

        if (salivaRigidbody != null)
        {

            salivaRigidbody.AddForce(new Vector2(-transform.localScale.x, 0) * salivaSpeed);

        }
        else
        {
            Debug.LogError("Saliva prefab'�nda Rigidbody bile�eni bulunamad�!");
        }
    }
}