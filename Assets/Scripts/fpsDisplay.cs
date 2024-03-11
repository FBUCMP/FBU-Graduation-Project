using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpsDisplay : MonoBehaviour
{

    private float fps;
    public TMPro.TextMeshProUGUI FPSCounterText;
    public Canvas HealthBarCanvas;

    // Start is called before the first frame update
    void Start()
    {
        HealthBarCanvas = GameObject.Find("HealthBarCanvas").GetComponent<Canvas>(); // canvas� bul
        FPSCounterText = HealthBarCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();   //texti �ek
        InvokeRepeating("getFPS", 1, 1); // saniye ba��na yenileme yap
    }


    void getFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        FPSCounterText.text = "FPS: " + fps.ToString("F0");
    }
}
