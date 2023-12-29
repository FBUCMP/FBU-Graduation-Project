using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
// its all just animation stuff
public class EnemyPainResponse : MonoBehaviour
{
    [SerializeField]
    private EnemyHealth health;
    private Animator animator;
    [SerializeField]
    [Range(1, 100)]
    private int maxDamagePainThreshold = 5;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HandlePain(int Damage)
    {
        if (health.currentHealth != 0)
        {
            // you can do some cool stuff based on the
            // amount of damage taken relative to max health
            // here we're simply setting the additive layer
            // weight based on damage vs max pain threshhold
            animator.ResetTrigger("Hit");
            animator.SetLayerWeight(1, (float)Damage / maxDamagePainThreshold);
            animator.SetTrigger("Hit");
        }
    }

    public void HandleDeath()
    {
        animator.applyRootMotion = true;
        animator.SetTrigger("Die");
    }
}