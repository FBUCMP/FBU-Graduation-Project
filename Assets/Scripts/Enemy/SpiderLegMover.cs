using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLegMover : MonoBehaviour 
{
    // attached to each bodyTarget which is where the target teleport to when the distance is far enough
    public Transform center; // center of the spider
    public Transform target; // target that ik actually follows
    public SpiderLegMover oppositeLeg; // opposite leg
    public SpiderLegMover oppositeLeg1; // opposite leg

    public LayerMask walkableLayer;
    public float moveDistance;
    public float groundCheckDistance;
    public float liftDistance;

    public Transform fixedPoint; // fixed point of where this object starts

    public float legSpeed;
    public bool grounded;
    
    private Vector2 targetPoint;
    private int posIndex;
    private Vector2 halfWayPoint;

    private Vector2 oldPos;
    void Start()
    {
        oldPos = target.position;
    }


    void Update()
    {
        if (!CheckGround())
        {
            target.localPosition = fixedPoint.localPosition;
        }

        // index 0 = set halfway and targetPoint , index 1 = move to halfway point, index 2 = move to targetPoint
        
        if (Vector2.Distance(target.position, transform.position) > 1f)
        {
            target.position = transform.position;
            oldPos = transform.position;
            posIndex = 0;
        }
        
        if (Vector2.Distance(transform.position, target.position) > moveDistance && posIndex == 0 && oppositeLeg.grounded && oppositeLeg1.grounded) // if the target is too far away and index is 0
        {
            //Debug.Log("0");
            oldPos = target.position; // save the old position of the target the leg follows
            targetPoint = transform.position; // set the target point to the position of bodyTarget
            halfWayPoint = (targetPoint + (Vector2)target.position) / 2; // set the halfway point to the middle of the target and bodyTarget
            halfWayPoint.y += liftDistance; // halfWayPoint is lifted up so leg looks realistic
            posIndex = 1; // set the index to 1
        }
        
        else if (posIndex == 1)
        {
            //Debug.Log("1");
            target.position = Vector3.Lerp(target.position, halfWayPoint, legSpeed * Time.deltaTime); // slowly move the target to the halfway point



            if (Vector2.Distance(target.position, halfWayPoint) <= 0.2f) // if the target is close enough to the halfway point
            {
                posIndex = 2;
            }
        }

        else if (posIndex == 2)
        {
            //Debug.Log("2");
            target.position = Vector3.Lerp(target.position, targetPoint, legSpeed * Time.deltaTime); // slowly move the target to the bodyTarget



            if (Vector2.Distance(target.position, targetPoint) < 0.2f) // if the target is close enough to the bodyTarget
            {
                posIndex = 0;
            }
        }

        if (posIndex == 0)
        {
            grounded = true;
            target.position = oldPos;
        }
        else
        {
            grounded = false;
            oldPos = target.position;
        }

    }

    public bool CheckGround()
    {
        Vector2 from = new Vector2(fixedPoint.position.x, center.position.y);
        RaycastHit2D hit = Physics2D.Raycast(from, Vector2.down, groundCheckDistance, walkableLayer);
        if (hit.collider != null)
        {
            //Debug.DrawRay(from, Vector2.down * groundCheckDistance, Color.blue);
            transform.position = hit.point;// + new Vector2(0, 0.00f);
            return true;
        }
        else
        {
            //Debug.DrawRay(from, Vector2.down * groundCheckDistance, Color.red);
            return false;
        }
    }
}
