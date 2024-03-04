using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : ScriptableObject
{
    public KeyCode key;
    public new string name;
    public float cooldownTime;
    public float activeTime;
    public bool taken = false;
    public virtual void Activate(GameObject parent) { }
    public virtual void BeginCooldown(GameObject parent) { }
}
