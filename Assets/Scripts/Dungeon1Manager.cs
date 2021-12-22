using UnityEngine;

public class Dungeon1Manager : MonoBehaviour
{
    private Canvas _endMenuCanvas;
    public GameObject dungeon1Boss;
    public GameObject endGate;
    public GameObject positionForEndGate;

    private int _nbEndGate = 0;
    private Dungeon1BossManager _dungeon1BossScriptSpawning;
    // Start is called before the first frame update
    void Start()
    {
        dungeon1Boss = GameObject.Find("Dungeon1Boss");
        Debug.Log(dungeon1Boss.name);
        _endMenuCanvas = GameObject.FindWithTag("EndMenu").GetComponent<Canvas>();
        _endMenuCanvas.enabled = false; // ensure the Pause Menu is deactivated on start

        _dungeon1BossScriptSpawning = dungeon1Boss.GetComponent<Dungeon1BossManager>();
    }

    // Update is called once per frame
    void Update()
    {
        DungeonCompleted();  // check if the boss is dead, and proceed to the "end of the level"
    }

    //
    // Load the EndMenu once DungeonCompleted
    //
    private void DungeonCompleted()
    {
        // once the boss is defeated (strengthLeft=0), it means no more pillars are left and the level is completed
        if (_dungeon1BossScriptSpawning.strengthLeft <= 0 & _nbEndGate == 0)
        {
            Vector3 endGateUpdatedPos = positionForEndGate.transform.position;
            endGateUpdatedPos.y = 43.6f;  // to be just visible and accessible to player.
            Instantiate(endGate, endGateUpdatedPos, positionForEndGate.transform.rotation);
            endGate.tag = "EndGate";
            _nbEndGate++;
        }
    }
    
    public void EndDungeon1Level()
    {
        Time.timeScale = 0;
        FindObjectOfType<GameManagerMain>().screenBusy = true;
        FindObjectOfType<GameManagerMain>().gameUICanvas.enabled = false;
        _endMenuCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        //FindObjectOfType<DiscoverPads>().freezeMovements = true;
    }
}
