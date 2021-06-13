using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // objects data
    public Player player;
    public Ball ball;
    public float cable_length = 5;
    public GameObject[] enemy_types;
    [SerializeField]
    private float mean_spawn_time = 3;
    private float spawn_time_diviation = 1.5f;
    [SerializeField]
    private int max_enemy_num = 10;

    // map data
    private static readonly float MAX_X = 6;
    private static readonly float MIN_X = -6;
    private static readonly float MAX_Y = 3;
    private static readonly float MIN_Y = -3;

    // UI data
    public bool isPaused = false;
    public int score = 0;
    public TextMeshProUGUI score_text;

    // Menu reference
    private Pause pauseScript;

    private void Start() {
        UpdateScore(-score);
        StartCoroutine("AutoSpawnEnemy");
        pauseScript = FindObjectOfType<Pause>();
        if (pauseScript != null)
            pauseScript.GM = this;

    }

    private void Update() {
        if (!isPaused) {
            /*
            if (Input.GetKeyDown(KeyCode.O)) {
                SpawnEnemy();
            }
            */
        }
    }

    IEnumerator AutoSpawnEnemy() {
        while (true) {
            float t = mean_spawn_time + Random.Range(-spawn_time_diviation, spawn_time_diviation);
            yield return new WaitForSeconds(t);

            // count turrets
            if(GameObject.FindGameObjectsWithTag("Enemy").Length < max_enemy_num) SpawnEnemy();
        }
    }

    public void SpawnEnemy() {
        Instantiate(enemy_types[0], GetRandomPointInView(), Quaternion.identity);
    }

    public Vector2 GetRandomPointInView() {
        return new Vector2(Random.Range(MIN_X, MAX_X), Random.Range(MIN_Y, MAX_Y));
    }

    public void Win() {

    }

    public void Lose() {
        Debug.Log("You have dishonored your family!");
    }

    public void UpdateScore(int difference) {
        score += difference;
        // Debug.Log(score);

        score_text.SetText("Score: " + score);
    }
}
