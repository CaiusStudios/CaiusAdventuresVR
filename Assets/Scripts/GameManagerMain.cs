using UnityEngine;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;

public class GameManagerMain : MonoBehaviour
{
    // TODO: why transition between scenes is not managed here in this script?
    // GameManagerMain is responsible for the
    //  - load initial scene and dialogues of the game, manage Game Over situation/menu.
    //  - display of the canvas/menus

    public TextAsset prologueJsonFile;
    
    private PlayerManager _scriptPlayerDamage;
    private bool _playerIsDead;
    
    public Canvas dialoguesMenuCanvas;
    public Canvas pauseMenuCanvas;
    public Canvas optionsMenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;  // ensure the time is "running"
        
        // dialoguesMenuCanvas = GameObject.FindWithTag("DialoguesMenu").GetComponent<Canvas>();
        // dialoguesMenuCanvas.enabled = false;
        // pauseMenuCanvas = GameObject.FindWithTag("PauseMenu").GetComponent<Canvas>();
        // pauseMenuCanvas.enabled = false; // ensure the Pause Menu is deactivated on start
        // optionsMenuCanvas = GameObject.FindWithTag("OptionsMenu").GetComponent<Canvas>();
        // optionsMenuCanvas.enabled = false;

        // TODO: resolve:_scriptPlayerDamage = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        
        if (SceneManager.GetActiveScene().name == "Village")
        {
            // Debug.Log("************Starting dialogue********");
            // DialoguePrologue(prologueJsonFile);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Check if player wants to access the Pause Menu, or exit it if already enabled
        if (Input.GetKeyDown(KeyCode.Tab) & pauseMenuCanvas.enabled)
        {
            PauseMenuOnOff();
        }
        
        // Manage when player dies: show Gameover menu
        // TODO: not working with VR because player not find: CheckPlayerStatus(); // TODO: make use of Event instead!
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
            if (_scriptPlayerDamage.HealthSystem.GetHealth() <= 0)
            {
                _playerIsDead = true;
                Time.timeScale = 0;
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
        } else if (Time.timeScale == 0)
        {
            // player wants to exit the Pause menu
            Time.timeScale = 1;
            pauseMenuCanvas.enabled = false;
        }
    }
    
    public void ResumeGame()
    {
        // a "global purpose" function to exit any of the canvas
        Time.timeScale = 1;
        pauseMenuCanvas.enabled = false;
        optionsMenuCanvas.enabled = false;
    }

    public void LoadOptionsMenu()
    {
        // assume we were in the Pause Menu and go into the Options Menu
        Time.timeScale = 0;
        pauseMenuCanvas.enabled = false;
        optionsMenuCanvas.enabled = true;
    }
    
    //
    // Dialogue: start with the prologue at the beginning of the game (when Village scene is loaded)
    //
    private void DialoguePrologue(TextAsset jsonFile)
    {
        if (!dialoguesMenuCanvas.enabled)
        {
            dialoguesMenuCanvas.enabled = true;
            FindObjectOfType<DialogueManager>().StartDialogue(jsonFile);
        }
    }
    
    public void CloseDialogue()
    {
        if (dialoguesMenuCanvas.enabled)
        {
            dialoguesMenuCanvas.enabled = false;
        }
    }
}
