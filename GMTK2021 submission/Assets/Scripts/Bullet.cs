using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 2;
    public float damage = 1;
    Rigidbody2D rb;
    
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update() {
        rb.velocity = -transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ball") || collision.gameObject.CompareTag("Cable")) {
            collision.gameObject.GetComponent<PC>().Hit(damage);
        }
        Destroy(gameObject);
    }
}
