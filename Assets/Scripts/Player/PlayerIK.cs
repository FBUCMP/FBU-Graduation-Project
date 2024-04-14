using System.Linq;
using UnityEngine;
using UnityEngine.U2D.IK;


public class PlayerIK : MonoBehaviour
{
    public float grabWallSpeed = 20f;
    public float grabGunSpeed = 50f;
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
    private PlayerMovementNew playerMovement;
    private void Awake()
    {
        SetHandWeights(handWeight);
        playerMovement = GetComponent<PlayerMovementNew>();
    }
    

    private void Update()
    {
        if (playerMovement != null)
        {
            
            if (playerMovement.isWalledLeft()) // reaching behind
            {
                
                float offset = Mathf.Sign( playerMovement.wallCheckL.position.x - playerMovement.transform.position.x );
                //rightHandTarget.position = playerMovement.wallCheckL.position  + new Vector3(offset * 0.6f, 1.35f);
                rightHandTarget.position = new Vector3(Mathf.Lerp(rightHandTarget.position.x, (playerMovement.wallCheckL.position + new Vector3(offset * 0.6f, 1.35f)).x, Time.deltaTime * grabWallSpeed),
                                                    Mathf.Lerp(rightHandTarget.position.y, (playerMovement.wallCheckL.position + new Vector3(offset * 0.6f, 1.35f)).y, Time.deltaTime * grabWallSpeed));
                rightHandSolver.flip = false;
            }
            else if (rightHandOnGun != null)
            {
                rightHandSolver.flip = true;
                //rightHandTarget.position = rightHandOnGun.position;
                rightHandTarget.position = new Vector3(Mathf.Lerp(rightHandTarget.position.x, rightHandOnGun.position.x, Time.deltaTime * grabGunSpeed),
                                                        Mathf.Lerp(rightHandTarget.position.y, rightHandOnGun.position.y, Time.deltaTime * grabGunSpeed));
                rightHandTarget.rotation = rightHandOnGun.rotation;
            }
            if (playerMovement.isWalledRight()) // reaching front
            {
                float offset = Mathf.Sign(playerMovement.wallCheckR.position.x - playerMovement.transform.position.x);
                //leftHandTarget.position = playerMovement.wallCheckR.position + new Vector3(offset * 0.5f, 1f);
                leftHandTarget.position = new Vector3(Mathf.Lerp(leftHandTarget.position.x, (playerMovement.wallCheckR.position + new Vector3(offset * 0.5f, 1f)).x, Time.deltaTime * grabWallSpeed),
                                                    Mathf.Lerp(leftHandTarget.position.y, (playerMovement.wallCheckR.position + new Vector3(offset * 0.5f, 1f)).y, Time.deltaTime * grabWallSpeed));

                //leftHandSolver.flip = false;
            }
            else if (leftHandOnGun != null)
            {
                leftHandSolver.flip = true;
                //leftHandTarget.position = leftHandOnGun.position;
                leftHandTarget.position = new Vector3(Mathf.Lerp(leftHandTarget.position.x, leftHandOnGun.position.x, Time.deltaTime * grabGunSpeed),
                                                        Mathf.Lerp(leftHandTarget.position.y, leftHandOnGun.position.y, Time.deltaTime * grabGunSpeed));
                leftHandTarget.rotation = leftHandOnGun.rotation; // might be wrong if so try local rotation
            }

            //leftHandTarget.rotation = leftHandOnGun.rotation; // might be wrong if so try local rotation

            
            
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
