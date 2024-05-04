using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalivaFire : MonoBehaviour
{

    public GameObject Saliva; // Saliva objesinin prefab'i
    public Transform salivaSpawnPoint; // Saliva'nýn spawnlanacaðý nokta
    public float salivaSpeed = 500f; // Saliva'nýn fýrlatma hýzý


    private void Start()
    {
        // Belirli aralýklarla SpawnSaliva fonksiyonunu çaðýr
        
    }
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
           
            salivaRigidbody.AddForce(-salivaSpawnPoint.right * salivaSpeed);
            
        }
        else
        {
            Debug.LogError("Saliva prefab'ýnda Rigidbody bileþeni bulunamadý!");
        }
    }
}