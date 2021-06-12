using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // objects data
    public Player player;
    public Ball ball;
    public float cable_length = 5;
    public GameObject[] enemy_types;

    // map data
    private static readonly float MAX_X = 6;
    private static readonly float MIN_X = -6;
    private static readonly float MAX_Y = 3;
    private static readonly float MIN_Y = -3;

    // UI data
    public bool isPaused = false;
    public int score = 0;

    private void Start() {
        UpdateScore(-score);
    }

    private void Update() {
        if (!isPaused) {
            if (Input.GetKeyDown(KeyCode.O)) {
                SpawnEnemy();
            }
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
        Debug.Log(score);

        // TODO
    }
}
