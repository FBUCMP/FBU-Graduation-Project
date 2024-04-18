using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepHandler : MonoBehaviour
{
    public ImpactType impactType;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPlayerStep()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2.2f);

        if (SurfaceManager.Instance != null && hit.collider != null)
        {
            //Debug.DrawLine(transform.position, (Vector3)Vector2.down*2 + transform.position, Color.red, 1f);
            SurfaceManager.Instance.HandleImpact(
                            hit.collider.gameObject,
                            hit.point,
                            hit.normal,
                            impactType,
                            0
                        );
        }
    }
}
