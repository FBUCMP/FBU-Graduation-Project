using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetObject;

    // proje 2d oldu�u i�in kamera Z'yi -10dan baslattim. Yoksa goruntu alamiyorduk
    public Vector3 cameraOffset = new Vector3(0, 0, -10f);
    public Vector3 targetedPosition;
    private Vector3 velocity = Vector3.zero;

    public float smoothTime = 0.25f;

    private void Start()
    {
        // ba�lang��ta target objecti tag �zerinden player se�mek i�in 
        // cameran�n takip edece�i obje haliyle player
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            targetObject = GameObject.FindGameObjectWithTag("Player");

        }
        else
        {
            Debug.Log("Player not found");
        }
    }


    /*
        Late Update, Update fonksiyonundan sonra �a�r�l�r ve kamera i�lemleri i�in kullan�lmaya uygundur.
    */
    private void LateUpdate()
    {
        targetObject = GameObject.FindGameObjectWithTag("Player");
        if (targetObject)
        {
            targetedPosition = targetObject.transform.position + cameraOffset; // kameran�n hedef pozisyonunu hesaplar
            //Debug.Log("targetedPosition: " + targetedPosition);
            // soft bir kamera takibi i�in SmoothDamp kulland�k.
            // Vector3.SmoothDamp, 
            // bir vekt�r�n di�er bir vekt�re yumu�ak bir �ekilde (smoothTime) ge�i�ini sa�lar.
            // transform.position'dan targetedPosition'a do�ru ge�i� yap�l�r
            // referans vekt�r� velocity ve ge�i�in s�resi smoothTime.
            transform.position = Vector3.SmoothDamp(transform.position, targetedPosition, ref velocity, smoothTime);

        }
    }
}