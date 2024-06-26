using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    // proje 2d oldu�u i�in kamera Z'yi -10dan baslattim. Yoksa goruntu alamiyorduk
    public Vector3 cameraOffset = new Vector3(0, 0, -10f);
    public Vector3 targetedPosition;
    private Vector3 velocity = Vector3.zero;
    private Camera thisCam;
    public float smoothTime = 0.25f;
    [HideInInspector] public float maxX, maxY, minX, minY;
    private void Start()
    {
        thisCam = GetComponent<Camera>();
        // ba�lang��ta target objecti tag �zerinden player se�mek i�in 
        // cameran�n takip edece�i obje haliyle player
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
        Late Update, Update fonksiyonundan sonra �a�r�l�r ve kamera i�lemleri i�in kullan�lmaya uygundur.
    */
    private void LateUpdate()
    {
        if (!target)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            else
            {
                return; // if player is not found, return from the update function !!
            }
        }
        if (target)
        {
            Vector3 screenVec = thisCam.ScreenToWorldPoint(Input.mousePosition);
            targetedPosition = (screenVec - target.position)/4f + target.position  + cameraOffset; // kameran�n hedef pozisyonunu hesaplar
            if (minX != 0 && minY != 0 && maxX != 0 && maxY != 0)
            {
                targetedPosition.x = Mathf.Clamp(targetedPosition.x, minX, maxX); // x ekseninde kamera s�n�rlar�n� belirler
                targetedPosition.y = Mathf.Clamp(targetedPosition.y, minY, maxY); // y ekseninde kamera s�n�rlar�n� belirler
            }
            /*
             Debug.Log("targetedPosition: " + targetedPosition);
             soft bir kamera takibi i�in SmoothDamp kulland�k.
             Vector3.SmoothDamp, 
             bir vekt�r�n di�er bir vekt�re yumu�ak bir �ekilde (smoothTime) ge�i�ini sa�lar.
             transform.position'dan targetedPosition'a do�ru ge�i� yap�l�r
             referans vekt�r� velocity ve ge�i�in s�resi smoothTime.
            */
            transform.position = Vector3.SmoothDamp(transform.position, targetedPosition, ref velocity, smoothTime);

        }
    }

    public void CameraShake(float duration, float magnitude, Vector2 dir)
    {
        StopAllCoroutines();
        StartCoroutine(Shake(duration, magnitude, dir));

    }

    IEnumerator Shake(float duration, float magnitude, Vector2 dir)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            Vector2 force = dir * magnitude;
            transform.position += new Vector3(force.x, + force.y);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    
}