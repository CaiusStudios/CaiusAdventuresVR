using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerMain : MonoBehaviour
{
    // TODO: why transition between scenes is not managed here in this script?
    // GameManagerMain is responsible for the
    //  - load initial scene and dialogues of the game, manage Game Over situation/menu.
    //  - display of the canvas/menus

    public TextAsset prologueJsonFile;
    
    private PlayerManager _scriptPlayerDamage;
    private bool _playerIsDead;
    public bool screenBusy;  // if true, there is already a canvas (or some screen) on display
    public Canvas gameUICanvas;
    
    public Canvas dialoguesMenuCanvas;
    public Canvas pauseMenuCanvas;
    public Canvas gameoverMenuCanvas;
    public Canvas optionsMenuCanvas;
    

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;  // ensure the time is "running"
        
        // dialoguesMenuCanvas = GameObject.FindWithTag("DialoguesMenu").GetComponent<Canvas>();
        // dialoguesMenuCanvas.enabled = false;
        // pauseMenuCanvas = GameObject.FindWithTag("PauseMenu").GetComponent<Canvas>();
        // pauseMenuCanvas.enabled = false; // ensure the Pause Menu is deactivated on start
        // gameoverMenuCanvas = GameObject.FindWithTag("GameoverMenu").GetComponent<Canvas>();
        // gameoverMenuCanvas.enabled = false;
        // optionsMenuCanvas = GameObject.FindWithTag("OptionsMenu").GetComponent<Canvas>();
        // optionsMenuCanvas.enabled = false;
        
        // gameUICanvas = GameObject.FindWithTag("GameUI").GetComponent<Canvas>();
        // gameUICanvas.enabled = true;
        
        _scriptPlayerDamage = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        
        if (SceneManager.GetActiveScene().name == "Village")
        {
            Debug.Log("************Starting dialogue********");
            // DialoguePrologue(prologueJsonFile);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Check if player wants to access the Pause Menu (or exit it if already enabled)
        if (Input.GetKeyDown(KeyCode.Tab) & (!screenBusy || pauseMenuCanvas.enabled))
        {
            PauseMenuOnOff();
        }
        
        
        // Manage when player dies: show Gameover menu
        // TODO: consider that PlayerManager should send the information to the GameManagerMain? (call some method) 
        CheckPlayerStatus();
    }
    
    //
    // Start and End of the Game
    //
    public void StartGame()
    {
        // initial start of the game
        SceneManager.LoadScene("Village");
    }
    
    private void CheckPlayerStatus()
    {
        // The game ends by the display of the Game Over Menu.
        if (!_playerIsDead)
        {
            if (_scriptPlayerDamage.currentHealth <= 0)
            {
                _playerIsDead = true;
                Time.timeScale = 0;
                gameoverMenuCanvas.enabled = true;
                screenBusy = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    //
    // Canvas / Menus
    //
    private void PauseMenuOnOff()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            pauseMenuCanvas.enabled = true;
            screenBusy = true;
            Cursor.lockState = CursorLockMode.None;

        } else if (Time.timeScale == 0)
        {
            // player wants to exit the Pause menu
            Time.timeScale = 1;
            pauseMenuCanvas.enabled = false;
            screenBusy = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    
    public void ResumeGame()
    {
        // a "global purpose" function to exit any of the canvas
        Time.timeScale = 1;
        pauseMenuCanvas.enabled = false;
        optionsMenuCanvas.enabled = false;
        screenBusy = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadOptionsMenu()
    {
        // assume we were in the Pause Menu and go into the Options Menu
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        pauseMenuCanvas.enabled = false;
        optionsMenuCanvas.enabled = true;
        screenBusy = true;
    }
    
    //
    // Dialogue: start with the prologue at the beginning of the game (when Village scene is loaded)
    //
    private void DialoguePrologue(TextAsset jsonFile)
    {
        if (!dialoguesMenuCanvas.enabled)
        {
            dialoguesMenuCanvas.enabled = true;
            screenBusy = true;
            FindObjectOfType<DialogueManager>().StartDialogue(jsonFile);
        }
    }
    
    public void CloseDialogue()
    {
        if (dialoguesMenuCanvas.enabled)
        {
            dialoguesMenuCanvas.enabled = false;
            screenBusy = false;
        }
    }
}
