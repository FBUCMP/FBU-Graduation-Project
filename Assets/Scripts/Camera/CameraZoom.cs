using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    private float zoom;
    private float zoomMultiplier = 6f;
    private float minZoom = 4f;
    public float maxZoom = 32f;
    private float velocity = 0;
    private float smoothTime = .25f;


    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        // cam bir camerad�r wow.
        cam = gameObject.GetComponent<Camera>();
        zoom = 8f; // Default Size 5
        //zoom = cam.orthographicSize; //Default Size
    }

    // Update is called once per frame
    void Update()
    {
        // fare tekerle�i arac�l���yla scroll * zoom multiplier kadar yak�nla��yor veya uzakla��yoruz
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * zoomMultiplier;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocity, smoothTime);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            zoom = minZoom;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            zoom = maxZoom;
        }
        // Mathf.SmoothDamp, Unity'nin Mathf s�n�f�nda bulunan ve de�erleri yumu�ak bir �ekilde ge�i� yapmak i�in 
        // kullan�lan bir fonksiyondur. Bu fonksiyon genellikle bir de�eri
        // (genellikle konum, h�z veya d�n��) di�er bir de�ere (hedef) yumu�ak bir �ekilde yakla�t�rmak i�in kullan�l�r.


    }
}