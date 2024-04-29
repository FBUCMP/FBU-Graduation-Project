using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockbackable
{
    public float stillThreshold { get; set; }
    void GetKnockedBack(Vector3 force, float maxMoveTime);
}
