using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    private List<GameObject> _chainBalls = new List<GameObject>();
    private float destroyDelay = 3.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        FindChainBalls();
    }

    // Update is called once per frame
    void Update()
    {
        FindChainBalls();
        if (_chainBalls.Count == 0)
        {
            Debug.Log(gameObject.name + " is a ball-less ghost!");
            Destroy(gameObject, destroyDelay * Time.deltaTime);
        }
        
    }

    //
    // helper functions
    //
    private void FindChainBalls(string searchTag = "chainBall")
    {
        _chainBalls.Clear();
        Transform parent = transform;
        GetChildObject(parent, searchTag);
    }

    private void GetChildObject(Transform parent, string searchTag)
    { 
        // source: https://answers.unity.com/questions/1197131/how-to-gameobjectfindgameobjectswithtag-within-chi.html
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.CompareTag(searchTag))
            {
                _chainBalls.Add(child.gameObject);
            }
        }
    }
}
