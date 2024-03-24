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
        HealthBarCanvas = GameObject.Find("HealthBarCanvas").GetComponent<Canvas>(); // canvasý bul
        FPSCounterText = HealthBarCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();   // texti çek
        InvokeRepeating("getFPS", 1, 1); // saniye baþýna yenileme yap
    }

    void getFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        FPSCounterText.text = "FPS: " + fps.ToString("F0");
    }
}
