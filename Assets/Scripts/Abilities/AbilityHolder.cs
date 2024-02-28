using System.Collections;
using System.Collections.Generic;
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

    private void InitializeAbilities()
    {
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

    private void Update()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            switch (states[i])
            {
                case AbilityState.ready:
                    if (Input.GetKeyDown(abilities[i].key))
                    {
                        abilities[i].Activate(gameObject);
                        states[i] = AbilityState.active;
                        activeTimes[i] = abilities[i].activeTime;
                    }
                    break;
                case AbilityState.active:
                    if (activeTimes[i] > 0)
                    {
                        activeTimes[i] -= Time.deltaTime;
                    }
                    else
                    {
                        abilities[i].BeginCooldown(gameObject);
                        states[i] = AbilityState.cooldown;
                        cooldownTimes[i] = abilities[i].cooldownTime;
                    }
                    break;
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
}
