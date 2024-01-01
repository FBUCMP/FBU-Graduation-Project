using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class PlayerIK : MonoBehaviour
{
    public Transform LeftHandIKTarget;
    public Transform RightHandIKTarget;
    public Transform LeftElbowIKTarget;
    public Transform RightElbowIKTarget;

    [SerializeField]
    [Range(0, 1f)]
    private float HandIKAmount = 1f;
    [SerializeField]
    [Range(0, 1f)]
    private float ElbowIKAmount = 1f;

    private Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (LeftHandIKTarget != null)
        {
            Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, HandIKAmount);
            Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, HandIKAmount);
            Animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandIKTarget.position);
            Animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandIKTarget.rotation);
        }
        if (RightHandIKTarget != null)
        {
            Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, HandIKAmount);
            Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, HandIKAmount);
            Animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandIKTarget.rotation);
            Animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandIKTarget.position);
        }
        if (LeftElbowIKTarget != null)
        {
            Animator.SetIKHintPosition(AvatarIKHint.LeftElbow, LeftElbowIKTarget.position);
            Animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, ElbowIKAmount);
        }

        if (RightElbowIKTarget != null)
        {
            Animator.SetIKHintPosition(AvatarIKHint.RightElbow, RightElbowIKTarget.position);
            Animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, ElbowIKAmount);
        }
    }
}