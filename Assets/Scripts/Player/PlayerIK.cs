using System.Linq;
using UnityEngine;
using UnityEngine.U2D.IK;


public class PlayerIK : MonoBehaviour
{
    public LimbSolver2D rightHandSolver;// drag all in editor
    public LimbSolver2D leftHandSolver;
    public Transform leftHandTarget; 
    public Transform rightHandTarget;

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
