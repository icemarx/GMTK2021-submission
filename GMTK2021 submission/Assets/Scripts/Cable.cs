using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : PC
{
    private GameManager GM;
    public SpriteRenderer spriteRenderer;
    public Sprite emptySprite;
    public Sprite fullSprite;
    
    void Start() {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        default_material = spriteRenderer.material;
    }
    

    public override void Hit(float damage) {
        GM.player.CableHit(damage);
    }

    IEnumerator DisplayHurtSprite() {
        spriteRenderer.material = hurt_material;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = default_material;
    }

    public void SetEmptySprite(bool isEmpty) {
        if (isEmpty) {
            spriteRenderer.sprite = emptySprite;
        }
        else {
            spriteRenderer.sprite = fullSprite;
        }
    }
}
