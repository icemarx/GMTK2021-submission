using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PC {
    // physics
    public float walk_speed = 1;
    public float dash_speed = 2;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        Movement();
    }

    private void Movement() {
        Vector2 movement_dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movement_dir = movement_dir.normalized;

        rb.velocity = movement_dir * walk_speed;
    }

    public void Throw() {
        // TODO
    }

    public void PickUp() {
        // TODO
    }
}
