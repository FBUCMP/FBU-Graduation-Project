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

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            targetObject = GameObject.FindGameObjectWithTag("Player");

        }
        else
        {
            Debug.Log("Player not found");
        }
    }
    private void LateUpdate()
    {
        targetObject = GameObject.FindGameObjectWithTag("Player");
        if (targetObject)
        {
            targetedPosition = targetObject.transform.position + cameraOffset;
            //Debug.Log("targetedPosition: " + targetedPosition);
            transform.position = Vector3.SmoothDamp(transform.position, targetedPosition, ref velocity, smoothTime);

        }
    }
}