using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkullSpawning : MonoBehaviour
{
    public GameObject babyEnemy;
    public int strengthLeft;
    public bool playerIsPresent;
    public List<GameObject> magicPillars;

    private bool _bossCoroutineStarted;

    private List<GameObject> _spawnedBabySkulls = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        strengthLeft = magicPillars.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsPresent & !_bossCoroutineStarted)
        {
            StartCoroutine(launchBabySkull());
            _bossCoroutineStarted = true;
        }
        updateCurrentStrength();  // update the 
        if (strengthLeft <= 0)
        {
            gameObject.SetActive(false);  // the Boss is dead!
            foreach (GameObject skull in _spawnedBabySkulls)
            {
                skull.SetActive(false);
            }
        }
    }

    // will instantiate new Skull that will attack the player
    // (they should attack the player irrespective of distance to player)
    IEnumerator launchBabySkull()
    {
        while ( strengthLeft > 0 )
        {
            //GameObject newBaby =  Instantiate(babyEnemy, randomSpawn(), transform.rotation);
            _spawnedBabySkulls.Add(Instantiate(babyEnemy, randomSpawn(), transform.rotation));

            strengthLeft--;  // remove a strength-point at each spawn of a baby-enemy
            // Debug.Log("Boss remaining strength: " + strengthLeft);
            yield return new WaitForSeconds(Random.Range(15, 10));  // wait 1-8 sec before new baby-enemy launch
        }
    }

    // update the number of active magical pillar => which represent the current strength of the boss
    private void updateCurrentStrength()
    {
        int nb = 0;
        foreach (GameObject pillar in magicPillars)
        {
            if (pillar.activeSelf)
            {
                nb++;
            }
        }

        strengthLeft = nb;  // update current strength w.r.t. remaining magical pillars
    }
    
    // "randomize" spawn position around the Boss
    private Vector3 randomSpawn()
    {
        Vector3 transPos = transform.position;
        float randomX = Random.Range(transPos.x * 0.85f, transPos.x * 1.15f);
        float randomY = Random.Range(transPos.y * 0.85f, transPos.y * 1.15f);
        float randomZ = Random.Range(transPos.z * 0.85f, transPos.z * 1.15f);
        transPos.x = randomX;
        transPos.y = randomY;
        transPos.z = randomZ;
        return transPos;
    }
    
    
}
