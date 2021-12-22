using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private Canvas _redScreenAttack;
    private float _redScreenDuration = 0.5f;
    
    private AudioSource _audioSource;
    
    private Canvas _inventoryMenuCanvas;
    private GameObject _currentSword;
    
    // Start is called before the first frame update
    void Start()
    {
        _inventoryMenuCanvas = GameObject.FindWithTag("InventoryMenu").GetComponent<Canvas>();
        _inventoryMenuCanvas.enabled = false;
        _currentSword = GameObject.Find("Sword");
        
        // Health
        _redScreenAttack = GameObject.Find("RedScreenAttack").GetComponent<Canvas>();
        HideRedScreen();
        currentHealth = maxHealth;
        
        // Combat
        
        // Audio
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        OpenInventoryMenu();
        SwitchSword();

        // if (OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger))
        // {
        //     Debug.LogWarning(".....................Near index trigger ");
        // }
        //
        // float tempPressure = OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger);
        // Debug.Log("...............pressed trigger at : " + tempPressure);



    }

    //
    // Manage Events (change of scene, boss), UI pops-up
    //
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
        bool buttonAstate = OVRInput.GetDown(OVRInput.Button.One);
        if (buttonAstate)
        {
            _currentSword.SetActive(!_currentSword.activeSelf);
        }
    }
    
    //
    // Player Health
    //
    public void TakeDamage(int damage)
    {
        ShowRedScreen();
        CancelInvoke("HideRedScreen");  // reset timer if hit before
        Invoke("HideRedScreen", _redScreenDuration);
        currentHealth -= damage;
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
