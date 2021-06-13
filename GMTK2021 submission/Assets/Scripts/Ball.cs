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
    public bool isAttackMode = false;

    // sprite references
    private SpriteRenderer spriteRenderer;
    public Sprite[] ballSprites;

    // audio references
    public AudioSource bounceSFX;

    private void Start() {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        spriteRenderer.flipX = rb.velocity.x > 0.1f;
        int spriteIndex = rb.velocity.y > 0.1f ? 1 : 0;
        spriteIndex += isConnected ? 0 : 2;
        spriteRenderer.sprite = ballSprites[spriteIndex];
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Debug.Log(speed);
        speed -= on_hit_speed_loss * throw_speed;
        speed = Mathf.Max(speed, 0);

        if (!isConnected)
            bounceSFX.Play();
        // Vector2 new_dir = Vector2.Reflect(rb.velocity, collision.GetContact(0).normal).normalized * speed;
        // rb.velocity = new_dir;

        if(collision.gameObject.CompareTag("Player")) {
            // connect
            PickUp();
        } else if(isAttackMode && collision.gameObject.CompareTag("Enemy")) {
            Destroy(collision.gameObject);
        }
    }

    public void Throw() {
        if (isConnected && GM.player.stamina >= GM.player.toss_cost) {
            // Debug.Log("THROW");
            GM.player.UpdateStamina(-GM.player.toss_cost);

            // start velocity
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            Vector2 dir = (mousePos - transform.position).normalized;

            rb.velocity = dir;
            speed = throw_speed;

            isAttackMode = true;
            GM.player.DisconnectCable();
        }
    }

    public void PickUp() {
        if (!isConnected) {
            // Debug.Log("PICK UP");

            isAttackMode = false;
            GM.player.ConnectCable();
        }
    }

    public override void Hit(float damage) {
        Debug.Log("Took it like a champ");
    }
}
