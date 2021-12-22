using System.Collections.Generic;
using UnityEngine;
/// <summary>
///   <para> Takes care of the specificities of a ghost (e.g. chain).</para>
///   <para> implements EnemyManager.cs.</para>
/// </summary>
public class GhostManager : Enemy
{
    private List<GameObject> _chainBalls = new List<GameObject>();
    private float _pointsPerChainball;
    private float _destroyDelay = 3.0f;
    
    public Transform pfHealthBar;
    public int MaxHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        FindChainBalls();
        AttackPoint = transform;
        PlayerMask = LayerMask.GetMask("Player");
        SetupHealthSystem(pfHealthBar, MaxHealth);

        _pointsPerChainball = (float)HealthSystem.GetHealth() / _chainBalls.Count;
        Debug.Log("Ghost: a chain ball represent " + _pointsPerChainball + " health points.");
    }

    // Update is called once per frame
    void Update()
    {
        FindChainBalls();
        if (_chainBalls.Count == 0)
        {
            Debug.Log(gameObject.name + " is a ball-less ghost!");
            Destroy(gameObject, _destroyDelay * Time.deltaTime);
        }
    }

    // -------------------------------------------------------------------
    // Fight related methods
    // -------------------------------------------------------------------
    /// <summary>
    ///   <para> ... .</para>
    /// </summary>
    private void FindChainBalls(string searchTag = "chainBall")
    {
        _chainBalls.Clear();
        Transform parent = transform;
        GetChildObject(parent, searchTag);
    }

    /// <summary>
    ///   <para> ... .</para>
    /// </summary>
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
