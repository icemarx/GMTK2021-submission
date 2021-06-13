using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : PC
{
    public Player player;
    public Ball ball;
    
    void Start() {
    }
    

    public override void Hit(float damage) {
        throw new System.NotImplementedException();
    }
}
