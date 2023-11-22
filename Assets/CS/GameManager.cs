using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;

    public int health;

    public PlayerMovement player;

    public void NextStage()
    {
        stageIndex++;
        
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
                other.attachedRigidbody.velocity = Vector2.zero;
                other.transform.position = new Vector3(0, 0, -1);    
            }
            // Health Down
            HealthDown();
        }
    }
}
