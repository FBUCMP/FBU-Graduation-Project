using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    // proje 2d olduðu için kamera Z'yi -10dan baslattim. Yoksa goruntu alamiyorduk
    public Vector3 cameraOffset = new Vector3(0, 0, -10f);
    public Vector3 targetedPosition;
    private Vector3 velocity = Vector3.zero;

    public float smoothTime = 0.25f;
    [HideInInspector] public float maxX, maxY, minX, minY;
    private void Start()
    {
        // baþlangýçta target objecti tag üzerinden player seçmek için 
        // cameranýn takip edeceði obje haliyle player
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;

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
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target)
        {
            targetedPosition = target.position + cameraOffset; // kameranýn hedef pozisyonunu hesaplar
            if (minX != 0 && minY != 0 && maxX != 0 && maxY != 0)
            {
                targetedPosition.x = Mathf.Clamp(targetedPosition.x, minX, maxX); // x ekseninde kamera sýnýrlarýný belirler
                targetedPosition.y = Mathf.Clamp(targetedPosition.y, minY, maxY); // y ekseninde kamera sýnýrlarýný belirler
            }
            /*Debug.Log("targetedPosition: " + targetedPosition);
             soft bir kamera takibi için SmoothDamp kullandýk.
             Vector3.SmoothDamp, 
             bir vektörün diðer bir vektöre yumuþak bir þekilde (smoothTime) geçiþini saðlar.
             transform.position'dan targetedPosition'a doðru geçiþ yapýlýr
             referans vektörü velocity ve geçiþin süresi smoothTime.
            */
            transform.position = Vector3.SmoothDamp(transform.position, targetedPosition, ref velocity, smoothTime);

        }
    }
}