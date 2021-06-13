using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private float min_spawn_distance = 2;

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

    public GameObject gameOverScreen;
    public Text gameOverScoreText;

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
        Vector3 point = GetRandomPointInView();
        while(Vector3.Distance(point, player.transform.position) < min_spawn_distance) {
            point = GetRandomPointInView();
        }

        Instantiate(enemy_types[0], point, Quaternion.identity);
    }

    public Vector2 GetRandomPointInView() {
        return new Vector2(Random.Range(MIN_X, MAX_X), Random.Range(MIN_Y, MAX_Y));
    }

    public void Lose() {
        if (pauseScript != null)
            pauseScript.DoPause();
        else {
            Time.timeScale = 0f;
            this.isPaused = true;
        }
        gameOverScreen.SetActive(true);
        gameOverScoreText.text = "Your score is: " + score.ToString();
    }

    public void UpdateScore(int difference) {
        score += difference;
        // Debug.Log(score);

        score_text.SetText("Score: " + score);
    }

    public void LoadMenu() {
        if (pauseScript != null)
            pauseScript.UnPause();
        else {
            Time.timeScale = 0f;
            this.isPaused = true;
        }
        SceneManager.LoadScene(0);
    }

    public void Restart() {
        if (pauseScript != null)
            pauseScript.UnPause();
        else {
            Time.timeScale = 0f;
            this.isPaused = true;
        }
        SceneManager.LoadScene(1);
    }

}
