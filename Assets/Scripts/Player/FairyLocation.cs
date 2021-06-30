using UnityEngine;

public class FairyLocation : MonoBehaviour
{
    public GameObject player;
    private Vector3 randomShift;
    private float valueShiftX;
    private float valueShiftY;
    private float valueShiftZ;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(68.0f, 1.0f, 440.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // valueShiftX = Random.Range(-1.0f, 1.0f);
        // valueShiftY = Random.Range(-1.0f, 1.0f);
        // valueShiftZ = Random.Range(-1.0f, 1.0f);
        // randomShift = new Vector3(valueShiftX, 0, 0);
        // transform.position = new Vector3(68.0f, 1.0f, 440.0f) + randomShift * Time.deltaTime;
    }
}
