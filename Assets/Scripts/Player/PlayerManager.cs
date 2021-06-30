using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private Canvas _endMenuCanvas;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider cCollider)
    {
        if (cCollider.CompareTag("TriggerNextLevel"))
        {
            SceneManager.LoadScene("Dungeon1");
        }

        if (cCollider.CompareTag("EndGate"))
        {
            Debug.Log("collided against EndGate!");
            FindObjectOfType<Dungeon1Manager>().EndDungeon1Level();
        }

        if (cCollider.CompareTag("EntranceToDungeon1Boss"))
        {
            FindObjectOfType<BossSkullSpawning>().playerIsPresent = true;  // enable the boss to "fire" at the player 
        }
        
    }
}
