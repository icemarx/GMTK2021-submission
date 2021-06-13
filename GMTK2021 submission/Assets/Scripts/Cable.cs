using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : PC
{
    private GameManager GM;
    
    void Start() {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        default_material = GetComponent<SpriteRenderer>().material;
    }
    

    public override void Hit(float damage) {
        GM.player.CableHit(damage);
    }

    IEnumerator DisplayHurtSprite() {
        GetComponent<SpriteRenderer>().material = hurt_material;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().material = default_material;
    }
}
