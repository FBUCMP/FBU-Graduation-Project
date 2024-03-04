using System.Linq;
using UnityEngine;
using UnityEngine.U2D.IK;


public class PlayerIK : MonoBehaviour
{
    public LimbSolver2D rightHandSolver;// drag all in editor
    public LimbSolver2D leftHandSolver;
    public Transform leftHandTarget; 
    public Transform rightHandTarget;
    public Transform head;
    public Transform headTarget;
    public Transform gunPivot;
    private Transform leftHandOnGun;
    private Transform rightHandOnGun;

    [Range(0, 1f)]
    [SerializeField]
    private float handWeight = 1f;

    private void Awake()
    {
        SetHandWeights(handWeight);
    }
    

    private void Update()
    {
        if (leftHandOnGun != null)
        {
            leftHandTarget.position = leftHandOnGun.position;
            leftHandTarget.rotation = leftHandOnGun.rotation; // might be wrong if so try local rotation
        }
        if (rightHandOnGun != null)
        {
            rightHandTarget.position = rightHandOnGun.position;
            rightHandTarget.rotation = rightHandOnGun.rotation;
        }
        if (head != null && rightHandTarget !=  null)
        {
            
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePos - (Vector2)gunPivot.position; // direction from gun rotation point to mouse
            headTarget.position = head.position + (Vector3)direction.normalized; // head target is where the head should look, it's the head position + the direction
            if (headTarget.localPosition.x < 1 && headTarget.localPosition.x > -1) /* if the head target's x is too close to the head, keep a distance of 1 so it doesnt directly look down or up */
            {
                headTarget.localPosition = new Vector3(Mathf.Sign(headTarget.localPosition.x) * 1, headTarget.localPosition.y, headTarget.localPosition.z);
            }
        }
            
    }

    public void SetHandWeights(float weight)
    {
        handWeight = weight;
        if (leftHandSolver != null || rightHandSolver != null)
        {
            leftHandSolver.weight = handWeight;
            rightHandSolver.weight = handWeight;
        }
    }

    public void Setup(Transform GunParent)
    {
        Transform[] allChildren = GunParent.GetComponentsInChildren<Transform>();

        leftHandOnGun = allChildren.FirstOrDefault(child => child.name == "LeftHandPos");
        rightHandOnGun = allChildren.FirstOrDefault(child => child.name == "RightHandPos");

    }
}
