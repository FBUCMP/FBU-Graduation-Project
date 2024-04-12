using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLimit : MonoBehaviour
{
    CameraZoom cameraZoom;
    CameraFollow cameraFollow;
    public float roomWidth;
    public float roomHeight;
    Camera cam;
    float cameraRatio = 16f / 9f;
    Vector3 roomCenter;

    private void Awake()
    {
        GateManager.OnTeleport += OnTeleport;
        cam = GetComponent<Camera>();
        cameraZoom = GetComponent<CameraZoom>();
        cameraFollow = GetComponent<CameraFollow>();
       
    }
    void Start()
    {

        cameraZoom.maxZoom = Mathf.Min(cameraZoom.maxZoom, CalculateMaxZoom());
        //Debug.Log("Max Zoom: " + cameraZoom.maxZoom);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateClamp();
        
    }
    float CalculateMaxZoom()
    {
        // calculate the max camera orthographic size based on the room size
        // max camera size cant allow the camera to see outside of the room
        float maxZoomX = roomWidth / 2;
        float maxZoomY = roomHeight / 2;
        float maxZoom = Mathf.Min(maxZoomX, maxZoomY);
        return maxZoom;
    }
    void OnTeleport(Vector3 roomCenter)
    {
        // if the player teleports to a new room, limit camera position to the new room

        this.roomCenter = roomCenter;
        UpdateClamp();
    }
    void UpdateClamp()
    {
        if (cam != null)
        {
            cameraFollow.maxX = (roomCenter.x + roomWidth / 2) - (cam.orthographicSize * cameraRatio);
            cameraFollow.minX = (roomCenter.x - roomWidth / 2) + (cam.orthographicSize * cameraRatio);
            cameraFollow.maxY = (roomCenter.y + roomHeight / 2) - cam.orthographicSize;
            cameraFollow.minY = (roomCenter.y - roomHeight / 2) + cam.orthographicSize;
        }
    }
}
