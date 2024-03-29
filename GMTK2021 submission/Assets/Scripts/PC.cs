using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PC : MonoBehaviour
{
    [SerializeField]
    protected float max_hp = 10;
    protected float hp = 10;
    [SerializeField]
    protected Material hurt_material;
    protected Material default_material;

    public abstract void Hit(float damage);
}
