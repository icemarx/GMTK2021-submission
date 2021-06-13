using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : PC
{
    private GameManager GM;
    
    void Start() {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    

    public override void Hit(float damage) {
        GM.player.CableHit(damage);
    }
}
