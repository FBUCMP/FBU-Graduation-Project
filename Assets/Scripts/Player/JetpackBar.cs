using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class JetpackBar : MonoBehaviour
{
    [HideInInspector] public Slider slider;
    public Image fill;
    public JetpackAbility jetpackAbility;

    private void Start()
    {
        slider = GetComponent<Slider>();
        

        if (jetpackAbility == null)
        {
            Debug.Log("jetpackAbility null");
        }


    }
    private void Update()
    {
        if (jetpackAbility)
        {
            if (jetpackAbility.taken)
            {
                fill.gameObject.SetActive(true);
            }
            else
            {
                fill.gameObject.SetActive(false);
            }

            float val = Mathf.Clamp01(jetpackAbility.currentCapacity / jetpackAbility.capacity);
            slider.value = val;
            
        }
    }
}
