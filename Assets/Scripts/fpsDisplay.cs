using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fpsDisplay : MonoBehaviour
{
    private float fps;
    public TMPro.TextMeshProUGUI FPSCounterText;
    public Canvas HealthBarCanvas;

    private void Start()
    {
        HealthBarCanvas = GameObject.Find("HealthBarCanvas").GetComponent<Canvas>(); // canvas� bul
        FPSCounterText = HealthBarCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();   // texti �ek
        InvokeRepeating("getFPS", 1, 1); // saniye ba��na yenileme yap
    }

    void getFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        FPSCounterText.text = "FPS: " + fps.ToString("F0");
    }
}
