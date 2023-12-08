using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetObject;

    // proje 2d olduðu için kamera Z'yi -10dan baslattim. Yoksa goruntu alamiyorduk
    public Vector3 cameraOffset = new Vector3(0, 0, -10f);
    public Vector3 targetedPosition;
    private Vector3 velocity = Vector3.zero;

    public float smoothTime = 0.25f;

    private void Start()
    {
        // baþlangýçta target objecti tag üzerinden player seçmek için 
        // cameranýn takip edeceði obje haliyle player
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
        Late Update, Update fonksiyonundan sonra çaðrýlýr ve kamera iþlemleri için kullanýlmaya uygundur.
    */
    private void LateUpdate()
    {
        targetObject = GameObject.FindGameObjectWithTag("Player");
        if (targetObject)
        {
            targetedPosition = targetObject.transform.position + cameraOffset; // kameranýn hedef pozisyonunu hesaplar
            //Debug.Log("targetedPosition: " + targetedPosition);
            // soft bir kamera takibi için SmoothDamp kullandýk.
            // Vector3.SmoothDamp, 
            // bir vektörün diðer bir vektöre yumuþak bir þekilde (smoothTime) geçiþini saðlar.
            // transform.position'dan targetedPosition'a doðru geçiþ yapýlýr
            // referans vektörü velocity ve geçiþin süresi smoothTime.
            transform.position = Vector3.SmoothDamp(transform.position, targetedPosition, ref velocity, smoothTime);

        }
    }
}