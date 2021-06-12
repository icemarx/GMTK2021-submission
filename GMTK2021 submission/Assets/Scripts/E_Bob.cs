using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Bob : Enemy {
    private GameManager GM;

    private Transform player;

    private void Start() {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GM.player.transform;
    }

    private void Update() {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, player.position - transform.position) * Quaternion.Euler(0,0,90);

        if (Input.GetKeyDown(KeyCode.I)) {
            Shoot();
        }
    }

    protected override void Shoot() {
        Instantiate(Bullet_Style, transform.position + transform.right, transform.rotation * Quaternion.Euler(0, 0, 180));
    }

    private void OnDestroy() {
        GM.UpdateScore(worth);
    }
}
