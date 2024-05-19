using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/JetpackAbility")]
public class JetpackAbility : AbilityManager
{
    /*
     public KeyCode key;
    public new string name;
    public float cooldownTime; 0
    public float activeTime; 0
    public bool taken = false;
    public AudioClip soundEffect;
     */
    public float capacity; // if not 0 cooldown will be 0 so always ready-active
    public float currentCapacity;
    public float rechargeRate; // speed of recharge
    public ParticleSystem particleEffect;
    public Vector2 particleOffset;
    private float gravity;
    PlayerMovementNew movement;
    [Tooltip("Enter cooldown here")] public float maxCooldownTime;
    private void OnEnable()
    {
        currentCapacity = capacity;
        cooldownTime = maxCooldownTime;
        
    }
    
    
    public override void Activate(GameObject parent)
    {
        //Debug.Log(currentCapacity);

        if (currentCapacity <= 0f)
        {
            return;
        }
        movement = parent.GetComponent<PlayerMovementNew>();
        movement.rb.velocity = new Vector2(movement.rb.velocity.x, Mathf.Min(movement.rb.velocity.y + 4, movement.speed*3)); // add upwards velocity numbers can be changed
        cooldownTime = 0;
        ParticleSystem newDashEffect = Instantiate(particleEffect, parent.transform.position + new Vector3(particleOffset.x * (movement.isFacingRight ? 1 : -1),
            particleOffset.y, 0f), Quaternion.Euler(0, 180, 0)); // particles to the back of the player
        newDashEffect.transform.SetParent(parent.transform);
        
        if (!AudioManager.Instance.IsPlaying())
        {
            AudioManager.Instance.PlaySFX(soundEffect);
        }
        currentCapacity -= Time.deltaTime;
        //Debug.Log("Current Capacity: " + currentCapacity);
        if (currentCapacity <= 0)
        {
            Debug.Log("Jetpack out of fuel");
            //cooldownTime = maxCooldownTime;
            //currentCapacity = capacity;
            currentCapacity = 0;
        }
    }
    public override void BeginCooldown(GameObject parent)
    {
        movement = parent.GetComponent<PlayerMovementNew>();
    }
    
}
