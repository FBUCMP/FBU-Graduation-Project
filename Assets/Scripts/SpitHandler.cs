using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitHandler : MonoBehaviour
{   
    public FlyBossManager flyBossManager;
    
    void OnSpitEvent()
    {
        flyBossManager.SpawnSaliva();
    }
    void OnHomingSpitEvent()
    {
        flyBossManager.SpawnHomingSaliva();
    }
}
