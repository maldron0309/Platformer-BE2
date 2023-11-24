using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;

    public int health;

    public PlayerMovement player;
    public GameObject[] stages;

    public Image[] UIhealth;
    public TextMeshPro UIPoint;
    public TextMeshPro UIStage;

    public void NextStage()
    {
        // Change Stage
        if (stageIndex < stages.Length - 1)
        {
            stages[stageIndex].SetActive(false);
            stageIndex++;
            stages[stageIndex].SetActive(true);
            PlayerReposition();
        }
        else // Game Clear
        {
            Time.timeScale = 0;
            // Result UI
            Debug.Log("클리어");
        }

        // Calulate Point
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        if (health > 0)
        {
            health--;
        }
        else
        {
            // Player Die Effect
            player.OnDie();
            // Result UI
            Debug.Log("죽음");
            // Retry Button UI
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // PLayer Resposition
            if (health > 1)
            {
                PlayerReposition();
            }

            // Health Down
            HealthDown();
        }
    }

    public void PlayerReposition()
    {
        player.VelocityZero();
    }
}