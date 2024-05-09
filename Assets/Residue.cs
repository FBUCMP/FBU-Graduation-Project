using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Residue : MonoBehaviour
{
    private void Awake()
    {
        Invoke("DestroySelf",5f);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }



}
