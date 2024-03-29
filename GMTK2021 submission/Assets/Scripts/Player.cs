using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : PC {
    private GameManager GM;

    // physics
    public float walk_speed = 1;
    public float dash_speed = 2;
    private bool isDashing = false;
    [SerializeField]
    private float dash_timer_max = 1;
    private float dash_timer = 0;
    [SerializeField]
    private float dash_force = 2;
    private Rigidbody2D rb;

    // ball
    private Ball ball;

    // cable
    [SerializeField]
    private Cable[] cable_parts;
    [SerializeField]
    private GameObject cable_all;
    public float cable_hp_max = 10;
    public float cable_hp = 0;

    // stamina
    public float max_stamina = 100;
    public float stamina = 100;
    public float toss_cost = 20;
    public float dash_cost = 10;
    public float gain_stamina = 0.25f;
    public bool can_regain_stamina = true;

    // UI
    public Slider hp_slider;
    public Slider stamina_slider;

    // sprite references
    private SpriteRenderer spriteRenderer;
    public Sprite[] playerSprites;


    // Audio reference
    public AudioSource dashSFX;
    public AudioSource hurtSFX;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ball = GM.ball;
        default_material = GetComponent<SpriteRenderer>().material;

        // set up the ui
        hp_slider.maxValue = max_hp;
        hp_slider.value = hp;
        stamina_slider.maxValue = max_stamina;
        stamina_slider.value = stamina;

        ConnectCable();
    }

    private void Update() {
        if (!GM.isPaused) {
            if (Input.GetMouseButtonDown(1)) {
                Dash();
            }
        }

        spriteRenderer.flipX = rb.velocity.x > 0.1f;
        int spriteIndex = rb.velocity.y > 0.1f ? 1 : 0;
        spriteRenderer.sprite = playerSprites[spriteIndex];

        if(!GM.isPaused && isDashing) {
            dash_timer -= Time.deltaTime;
            if(dash_timer <= 0) {
                StopDashing();
            }
        }

    }

    void FixedUpdate() {
        if (!GM.isPaused) {
            Movement();

            if (can_regain_stamina) {
                UpdateStamina(gain_stamina);
            }
        }
    }

    private void Movement() {
        Vector2 movement_dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movement_dir = movement_dir.normalized;

        if (!isDashing)
            rb.velocity = movement_dir * walk_speed;
        // else
        //     rb.velocity = movement_dir * dash_speed;
            
    }

    public void UpdateStamina(float difference) {
        stamina = Mathf.Clamp(stamina+difference, 0, max_stamina);

        // update UI
        stamina_slider.value = stamina;
    }

    private void Dash() {
        if (stamina >= dash_cost) {
            UpdateStamina(-dash_cost);

            isDashing = true;
            dash_timer = dash_timer_max;
            dashSFX.Play();
            
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            Vector3 dir = (mousePos2D - new Vector2(transform.position.x, transform.position.y)).normalized;

            rb.velocity = Vector2.zero;
            rb.AddForce(dir * dash_force);

            // affect sprite
            Color sprite_c = GetComponent<SpriteRenderer>().color;
            sprite_c = new Color(sprite_c.r, sprite_c.g, sprite_c.b, .5f);
            GetComponent<SpriteRenderer>().color = sprite_c;

            // change collisions
            gameObject.layer = 8;
        }
    }

    private void StopDashing() {
        dash_timer = 0;
        isDashing = false;

        // affect sprite
        Color sprite_c = GetComponent<SpriteRenderer>().color;
        sprite_c = new Color(sprite_c.r, sprite_c.g, sprite_c.b, 1);
        GetComponent<SpriteRenderer>().color = sprite_c;

        // change collisions
        gameObject.layer = 7;
    }

    public override void Hit(float damage) {
        hp = Mathf.Max(0, hp - damage);

        // update UI
        hp_slider.value = hp;

        // sprite effect
        StartCoroutine("DisplayHurtSprite");
        hurtSFX.Play();

        if (hp <= 0) {
            GM.Lose();
        }
    }

    public void CableHit(float damage) {
        cable_hp = Mathf.Max(0, cable_hp - damage);
        // Debug.Log(cable_hp);

        // update UI?

        if(cable_hp <= 0) {
            DisconnectCable();
        } else {
            int counter = 0;
            foreach(Cable c in cable_parts) {
                c.SetEmptySprite(cable_hp <= counter);
                c.StartCoroutine("DisplayHurtSprite");
                counter++;
            }
        }
    }

    public void ConnectCable() {
        ball.isConnected = true;
        can_regain_stamina = true;
        cable_all.SetActive(true);
        cable_hp = cable_hp_max;
        foreach(Cable c in cable_parts) {
            c.SetEmptySprite(false);
        }
    }

    public void DisconnectCable() {
        ball.isConnected = false;
        can_regain_stamina = false;
        cable_all.SetActive(false);
        cable_hp = 0;
        foreach(Cable c in cable_parts) {
            c.SetEmptySprite(true);
        }
    }

    IEnumerator DisplayHurtSprite() {
        GetComponent<SpriteRenderer>().material = hurt_material;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().material = default_material;
    }
}
