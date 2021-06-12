using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    public int worth = 1;
    public GameObject Bullet_Style;

    protected abstract void Shoot();
}
