using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : PC {
    private GameManager GM;


    [Min(0)]
    public float speed = 1;
    [Min(0)]
    public float on_hit_speed_loss = 0.15f;
    [SerializeField]
    [Min(0)]
    private float throw_speed = 3;
    private Rigidbody2D rb;
    public bool isConnected = true;

    private void Start() {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Throw();
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            PickUp();
        }
        
        if (!isConnected)
            rb.velocity = rb.velocity.normalized * speed;
        else if (Vector2.Distance(transform.position, GM.player.transform.position) > GM.cable_length) {
            Vector2 dir = (GM.player.transform.position - transform.position).normalized;
            rb.velocity = dir * GM.player.walk_speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        speed -= on_hit_speed_loss;
        speed = Mathf.Max(speed, 0);

        if(collision.gameObject.CompareTag("Player")) {
            // connect
            PickUp();
        }
    }

    public void Throw() {
        if (isConnected && GM.player.stamina >= GM.player.toss_cost) {
            Debug.Log("THROW");
            GM.player.UpdateStamina(-GM.player.toss_cost);

            // start velocity
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            Vector2 dir = (mousePos - transform.position).normalized;

            rb.velocity = dir;
            speed = throw_speed;
            isConnected = false;
            GM.player.can_regain_stamina = false;
        }
    }

    public void PickUp() {
        if (!isConnected) {
            Debug.Log("PICK UP");

            isConnected = true;
            GM.player.can_regain_stamina = true;

            // TODO
        }
    }
}
