using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalivaFire : MonoBehaviour
{

    public GameObject Saliva; // Saliva objesinin prefab'i
    public GameObject HomingSaliva;
    public Transform salivaSpawnPoint; // Saliva'nýn spawnlanacaðý nokta
    public float salivaSpeed = 500f; // Saliva'nýn fýrlatma hýzý


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(salivaSpawnPoint.position, salivaSpawnPoint.position -salivaSpawnPoint.right*5);
    }



    // Fýrlatma iþlemini baþlatan fonksiyon
    public void SpawnSaliva()
    {
        
        // Saliva prefab'ýný spawnlayarak yeni bir Saliva objesi oluþtur
        GameObject newSaliva = Instantiate(Saliva, salivaSpawnPoint.position, salivaSpawnPoint.rotation);

        // Saliva'nýn rigidbody bileþenini al
        Rigidbody2D salivaRigidbody = newSaliva.GetComponent<Rigidbody2D>();

        // Eðer Saliva'nýn rigidbody bileþeni varsa
        if (salivaRigidbody != null)
        {
            // Saliva'yý belirli bir hýzda fýrlat
            //salivaRigidbody.velocity = salivaSpawnPoint.forward * salivaSpeed;
            Vector2 dir = new Vector2(-transform.localScale.x + (Random.value*2 - 1f), 0); // random.value * 2 - 1f = -1 ile 1 arasýnda random bir deðer
            salivaRigidbody.AddForce(dir * salivaSpeed);
         
        }
        else
        {
            Debug.LogError("Saliva prefab'ýnda Rigidbody bileþeni bulunamadý!");
        }
    }

    public void SpawnHomingSaliva()
    {

        // Saliva prefab'ýný spawnlayarak yeni bir Saliva objesi oluþtur
        GameObject newSaliva = Instantiate(HomingSaliva, salivaSpawnPoint.position, salivaSpawnPoint.rotation);

        Rigidbody2D salivaRigidbody = newSaliva.GetComponent<Rigidbody2D>();

        if (salivaRigidbody != null)
        {

            salivaRigidbody.AddForce(new Vector2(-transform.localScale.x, 0) * salivaSpeed);

        }
        else
        {
            Debug.LogError("Saliva prefab'ýnda Rigidbody bileþeni bulunamadý!");
        }
    }
}