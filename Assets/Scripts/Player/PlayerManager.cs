using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public int maxHealth = 100;
    private HealthSystem _healthSystem;
    public HealthSystem HealthSystem
    {
        get { return _healthSystem; }
        set { _healthSystem = value; }
    }

    private Camera _mainCamera;
    private AudioSource _audioSource;
    private GameObject _currentSword;
    
    private Canvas _inventoryMenuCanvas;
    private Canvas _redScreenAttack;
    private float _redScreenDuration = 0.25f;
    
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _inventoryMenuCanvas = GameObject.Find("InventoryMenu").GetComponent<Canvas>();
        _inventoryMenuCanvas.enabled = false;
        
        _redScreenAttack = GameObject.Find("RedScreenAttack").GetComponent<Canvas>();
        HideRedScreen();  // .enabled = false
        
        _currentSword = GameObject.Find("Sword");
        
        // Health
        _healthSystem = new HealthSystem(maxHealth);
        _healthSystem.OnHealthChanged += HealthSystemOnOnHealthChanged;
        _healthSystem.OnDeath += HealthSystemOnOnDeath;

        // Audio
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        OpenInventoryMenu();  // TODO: move to an Event handler
        SwitchSword();        // TODO: move to an Event handler
    }

    // -------------------------------------------------------------------
    // Manage Events (change of scene, boss), UI pops-up
    // -------------------------------------------------------------------
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
            // TODO: the Boss (Enemy) should have the intelligence to detect the Player.
            FindObjectOfType<Dungeon1BossManager>().playerIsPresent = true;  // enable the boss to "fire" at the player 
        }
        
    }
    
    private void OpenInventoryMenu()
    {
        bool buttonXstate = OVRInput.GetDown(OVRInput.Button.Three);
        if (buttonXstate)
        {
            _inventoryMenuCanvas.enabled = !_inventoryMenuCanvas.enabled;
        }
    }

    private void SwitchSword()
    {
        bool buttonAstate = OVRInput.GetDown(OVRInput.Button.Four);
        if (buttonAstate)
        {
            _currentSword.SetActive(!_currentSword.activeSelf);
        }
    }
    
    // -------------------------------------------------------------------
    // Player Health
    // -------------------------------------------------------------------
    private void HealthSystemOnOnHealthChanged(object sender, EventArgs e)
    {
        ShowRedScreen();
        CancelInvoke("HideRedScreen");  // reset timer if hit before
        Invoke("HideRedScreen", _redScreenDuration);
    }
    private void HealthSystemOnOnDeath(object sender, EventArgs e)
    {
        Debug.Log("The Player is Dead! Game Over! Try Again!");
    }

    private void ShowRedScreen()
    {
        _redScreenAttack.enabled = true;
    }

    private void HideRedScreen()
    {
        _redScreenAttack.enabled = false;
    }
    
    //
    // Player Audio
    //
    public void PlayAudioFX()
    {
        _audioSource.Play();
    }
    
}
