using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerMain : MonoBehaviour
{
    public TextAsset prologueJsonFile;
    
    private PlayerHealthDamageController _scriptPlayerDamage;
    private bool _playerIsDead;
    public bool screenBusy;  // if true, there is already a canvas (or some screen) on display
    public Canvas gameUICanvas;
    
    private Canvas _dialoguesMenuCanvas;
    private Canvas _pauseMenuCanvas;
    private Canvas _gameoverMenuCanvas;
    private Canvas _inventoryMenuCanvas;
    private Canvas _optionsMenuCanvas;
    

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;  // ensure the time is "running"
        
        // canvas
        _dialoguesMenuCanvas = GameObject.FindWithTag("DialoguesMenu").GetComponent<Canvas>();
        _dialoguesMenuCanvas.enabled = false;
        _pauseMenuCanvas = GameObject.FindWithTag("PauseMenu").GetComponent<Canvas>();
        _pauseMenuCanvas.enabled = false; // ensure the Pause Menu is deactivated on start
        _gameoverMenuCanvas = GameObject.FindWithTag("GameoverMenu").GetComponent<Canvas>();
        _gameoverMenuCanvas.enabled = false;
        _inventoryMenuCanvas = GameObject.FindWithTag("InventoryMenu").GetComponent<Canvas>();
        _inventoryMenuCanvas.enabled = false;
        _optionsMenuCanvas = GameObject.FindWithTag("OptionsMenu").GetComponent<Canvas>();
        _optionsMenuCanvas.enabled = false;
        
        // UI
        gameUICanvas = GameObject.FindWithTag("GameUI").GetComponent<Canvas>();
        gameUICanvas.enabled = true;
        
        // player
        _scriptPlayerDamage = GameObject.FindWithTag("Player").GetComponent<PlayerHealthDamageController>();
        
        // if loaded scene = village, then play the prologue
        if (SceneManager.GetActiveScene().name == "Village")
        {
            DialoguePrologue(prologueJsonFile);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Check if player wants to access the Pause Menu (or exit it if already enabled)
        if (Input.GetKeyDown(KeyCode.Tab) & (!screenBusy || _pauseMenuCanvas.enabled))
        {
            PauseMenuOnOff();
        }
        
        // Show inventory menu (or exit it if already enabled)
        if (Input.GetKeyDown(KeyCode.Alpha1) & (!screenBusy || _inventoryMenuCanvas.enabled))
        {
            InventoryMenuOnOff();
        }
        
        // Manage when player dies: show Gameover menu
        CheckPlayerStatus();
    }
    
    // Check Player Status: if dead, enable Gameover menu
    private void CheckPlayerStatus()
    {
        if (!_playerIsDead)
        {
            if (_scriptPlayerDamage.currentHealth <= 0)
            {
                _playerIsDead = true;
                Debug.Log("Game Over!");
                Time.timeScale = 0;
                _gameoverMenuCanvas.enabled = true;
                screenBusy = true;
                Cursor.lockState = CursorLockMode.None;
                FindObjectOfType<DiscoverPads>().freezeMovements = true;  // deactivate any update on keyboard/mouse movements
            }
        }
    }
    
    // Show pause menu: on/ff
    private void PauseMenuOnOff()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            _pauseMenuCanvas.enabled = true;
            screenBusy = true;
            Cursor.lockState = CursorLockMode.None;
            FindObjectOfType<DiscoverPads>().freezeMovements = true;  // deactivate any update on keyboard/mouse movements

        } else if (Time.timeScale == 0)
        {
            // player wants to exit the Pause menu
            Time.timeScale = 1;
            _pauseMenuCanvas.enabled = false;
            screenBusy = false;
            Cursor.lockState = CursorLockMode.Locked;
            FindObjectOfType<DiscoverPads>().freezeMovements = false;
        }
    }
    
    // Show Inventory Menu: on/off
    private void InventoryMenuOnOff()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            _inventoryMenuCanvas.enabled = true;
            screenBusy = true;
            Cursor.lockState = CursorLockMode.None;
            FindObjectOfType<DiscoverPads>().freezeMovements = true;  // deactivate any update on keyboard+mouse movements

        } else if (Time.timeScale == 0)
        {
            // player wants to exit the Pause menu
            Time.timeScale = 1;
            _inventoryMenuCanvas.enabled = false;
            screenBusy = false;
            Cursor.lockState = CursorLockMode.Locked;
            FindObjectOfType<DiscoverPads>().freezeMovements = false;
        }
    }
    
    // Dialogue: start with the prologue at the beginning of the game (when Village scene is loaded)
    private void DialoguePrologue(TextAsset jsonFile)
    {
        if (!_dialoguesMenuCanvas.enabled)
        {
            FindObjectOfType<DiscoverPads>().freezeMovements = true;  // deactivate any update on keyboard+mouse movements
            _dialoguesMenuCanvas.enabled = true;
            screenBusy = true;
            FindObjectOfType<DialogueManager>().StartDialogue(jsonFile);
        }
    }
    
    public void StartGame()
    {
        // initial start of the game
        SceneManager.LoadScene("Village");
    }
    
    public void ResumeGame()
    {
        // a "global purpose" function to exit any of the canvas
        Time.timeScale = 1;
        _pauseMenuCanvas.enabled = false;
        _inventoryMenuCanvas.enabled = false;
        _optionsMenuCanvas.enabled = false;
        screenBusy = false;
        Cursor.lockState = CursorLockMode.Locked;
        FindObjectOfType<DiscoverPads>().freezeMovements = false;
    }

    public void LoadOptionsMenu()
    {
        // assume we were in the Pause Menu and go into the Options Menu
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        _pauseMenuCanvas.enabled = false;
        _optionsMenuCanvas.enabled = true;
        screenBusy = true;
    }
    


    public void CloseDialogue()
    {
        if (_dialoguesMenuCanvas.enabled)
        {
            FindObjectOfType<DiscoverPads>().freezeMovements = false;  // activate any update on keyboard+mouse movements
            _dialoguesMenuCanvas.enabled = false;
            screenBusy = false;
        }
    }
}
