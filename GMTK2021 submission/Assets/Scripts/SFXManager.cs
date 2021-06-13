using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource enemyDeath;
    public AudioClip enemyDeathclip;

    public static SFXManager Instance = null;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Multiple instances of SFXManager");
            Destroy(this);
        }
    }


    public void playEnemyDeath()
    {
        enemyDeath.PlayOneShot(enemyDeathclip, 0.4f);
    }

}
