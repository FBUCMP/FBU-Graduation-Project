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
        targetObject = GameObject.FindGameObjectWithTag("Player"); // baslangiçtaki sahnede bulunan playeri otomatik secmesi icin.
        cameraOffset.x = transform.localPosition.x;
        cameraOffset.y = transform.localPosition.y;
        cameraOffset.z = -10f;
    }
    private void LateUpdate()
    {
        targetedPosition = targetObject.transform.position + cameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetedPosition, ref velocity, smoothTime);
    }
}
