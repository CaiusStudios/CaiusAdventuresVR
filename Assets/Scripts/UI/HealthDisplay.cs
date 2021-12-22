using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private int _health;
    private PlayerManager _playerHealthScript;
    public Text healthText;
    // Start is called before the first frame update
    void Start()
    {
        _playerHealthScript = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _health = _playerHealthScript.currentHealth;
        healthText.text = "HEALTH : " + _health;
    }
}
