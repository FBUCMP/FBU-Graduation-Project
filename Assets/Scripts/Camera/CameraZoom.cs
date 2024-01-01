using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    private float zoom;
    private float zoomMultiplier = 4f;
    private float minZoom = 4f;
    private float maxZoom = 16f;
    private float velocity = 0;
    private float smoothTime = 0.25f;


    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        // cam bir cameradýr wow.
        cam = gameObject.GetComponent<Camera>();
        zoom = 8f; // Default Size 5
        //zoom = cam.orthographicSize; //Default Size
    }

    // Update is called once per frame
    void Update()
    {
        // fare tekerleði aracýlýðýyla scroll * zoom multiplier kadar yakýnlaþýyor veya uzaklaþýyoruz
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * zoomMultiplier;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocity, smoothTime);
        // Mathf.SmoothDamp, Unity'nin Mathf sýnýfýnda bulunan ve deðerleri yumuþak bir þekilde geçiþ yapmak için 
        // kullanýlan bir fonksiyondur. Bu fonksiyon genellikle bir deðeri
        // (genellikle konum, hýz veya dönüþ) diðer bir deðere (hedef) yumuþak bir þekilde yaklaþtýrmak için kullanýlýr.


    }
}