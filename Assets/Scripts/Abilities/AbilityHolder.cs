using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public List<AbilityManager> abilities;
    List<float> cooldownTimes;
    List<float> activeTimes;
    List<AbilityState> states;

    enum AbilityState
    {
        ready,
        active,
        cooldown
    }

    private void Start()
    {
        InitializeAbilities();
    }
    public void AddAbility(AbilityManager newAbility)
    {
        foreach (var ability in abilities)
        {
            if(ability == newAbility)
            {
                return;
            }
        }
        newAbility.taken = true;
        abilities.Add(newAbility);
        cooldownTimes.Add(0f);
        activeTimes.Add(0f);
        states.Add(AbilityState.ready);
        Debug.Log("Initialize Will Begin");
    }
    private void InitializeAbilities()
    {
        Debug.Log("Initialize Begin");
        cooldownTimes = new List<float>();
        activeTimes = new List<float>();
        states = new List<AbilityState>();

        foreach (var ability in abilities)
        {
            cooldownTimes.Add(0f);
            activeTimes.Add(0f);
            states.Add(AbilityState.ready);
        }
    }

    private void FixedUpdate()
    {
        

        for (int i = 0; i < abilities.Count; i++)
        {
            switch (states[i])
            {

                // ---------------------------------- READY ----------------------------------
                case AbilityState.ready:
                    
                    if (Input.GetKey(abilities[i].key)) // key pressed
                    {
                        
                        abilities[i].Activate(gameObject);
                        states[i] = AbilityState.active;
                        activeTimes[i] = abilities[i].activeTime;
                    }

                    else // key NOT pressed
                    {
                        if (abilities[i].GetType() == typeof(JetpackAbility)) // if type of jetpack
                        {
                            JetpackAbility jetpack = (JetpackAbility)abilities[i];
                            RechargeJetpack(jetpack, i); // this is a special case for jetpacks to recharge
                        }
                    }
                    break;

                // ---------------------------------- ACTIVE ----------------------------------
                case AbilityState.active:
                    if (activeTimes[i] > 0)
                    {
                        activeTimes[i] -= Time.deltaTime;
                    }
                    else // when active time count is over
                    {
                        if (abilities[i].cooldownTime > 0) // if ability has cooldown
                        {
                            abilities[i].BeginCooldown(gameObject);
                            states[i] = AbilityState.cooldown;
                            cooldownTimes[i] = abilities[i].cooldownTime;
                        }
                        else // if ability has NO cooldown
                        {
                            states[i] = AbilityState.ready;
                        }
                    }
                    break;
                
                // -------------------------------- COOLDOWN ----------------------------------
                case AbilityState.cooldown:
                    if (cooldownTimes[i] > 0)
                    {
                        cooldownTimes[i] -= Time.deltaTime;
                    }
                    else
                    {
                        states[i] = AbilityState.ready;
                    }
                    break;
            }
        }
    }

    void RechargeJetpack(JetpackAbility jetpack, int index) // regen capacity when in cooldown
    {
        Debug.Log(jetpack.currentCapacity);
        if (jetpack.cooldownTime <= 0 && jetpack.currentCapacity < jetpack.capacity)
        {
            jetpack.currentCapacity += Time.deltaTime* jetpack.rechargeRate;
            jetpack.currentCapacity = Mathf.Min(jetpack.currentCapacity, jetpack.capacity); // clamp the max amount
        }
            
    }
}
