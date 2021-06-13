using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Bob : Enemy {
    private GameManager GM;

    private Transform player;

    [SerializeField]
    private float mean_shoot_time = 3;
    [SerializeField]
    private float shoot_time_diviation = 1.5f;

    [SerializeField]
    private GameObject turret;

    public AudioSource shootSFX;
    public GameObject deathFX;

    private void Start() {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GM.player.transform;

        StartCoroutine("AutoShoot");
    }

    private void Update() {
        turret.transform.rotation = Quaternion.LookRotation(Vector3.forward, player.position - transform.position) * Quaternion.Euler(0,0,90);

        /*
        if (Input.GetKeyDown(KeyCode.I)) {
            Shoot();
        }
        */
    }

    IEnumerator AutoShoot() {
        while(true) {
            float t = mean_shoot_time + Random.Range(-shoot_time_diviation, shoot_time_diviation);
            yield return new WaitForSeconds(t);

            Shoot();
        }
    }

    protected override void Shoot() {
        Instantiate(Bullet_Style, turret.transform.position + turret.transform.right, turret.transform.rotation * Quaternion.Euler(0, 0, 180));
        shootSFX.Play();
    }

    private void OnDestroy() {

        GM.UpdateScore(worth);
    }

    public void SpawnDeathFX() {
        Instantiate(deathFX, this.transform.position, Quaternion.identity);
    }
}
