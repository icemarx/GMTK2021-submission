using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PC : MonoBehaviour
{
    protected float hp = 10;

    public abstract void Hit(float damage);
}
