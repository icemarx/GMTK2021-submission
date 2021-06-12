using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : PC {
    private GameManager GM;

    // physics
    public float walk_speed = 1;
    public float dash_speed = 2;
    private Rigidbody2D rb;

    // ball
    private Ball ball;

    // stamina
    public float max_stamina = 100;
    public float stamina = 100;
    public float toss_cost = 20;
    public float dash_cost = 10;
    public float gain_stamina = 0.1f;
    public bool can_regain_stamina = true;

    // UI
    public Slider hp_slider;
    public Slider stamina_slider;

    // sprite references
    private SpriteRenderer spriteRenderer;
    public Sprite[] playerSprites;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ball = GM.ball;

        // set up the ui
        hp_slider.maxValue = max_hp;
        hp_slider.value = hp;
        stamina_slider.maxValue = max_stamina;
        stamina_slider.value = stamina;
    }

    private void Update() {
        if (!GM.isPaused) {
            if (can_regain_stamina) {
                UpdateStamina(gain_stamina);
            }

            if (Input.GetKeyDown(KeyCode.Q)) {
                Dash();
            }
        }

        spriteRenderer.flipX = rb.velocity.x > 0f;
        int spriteIndex = rb.velocity.y > 0f ? 1 : 0;
        spriteRenderer.sprite = playerSprites[spriteIndex];

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

    public void UpdateStamina(float difference) {
        stamina = Mathf.Clamp(stamina+difference, 0 , max_stamina);

        // update UI
        stamina_slider.value = stamina;
    }

    private void Dash() {
        if (stamina >= dash_cost) {
            Debug.Log("DASH");
            UpdateStamina(-dash_cost);

            // TODO
        }
    }

    public override void Hit(float damage) {
        hp = Mathf.Max(0, hp - damage);

        // update UI
        hp_slider.value = hp;

        if (hp <= 0) {
            GM.Lose();
        }
    }
}
