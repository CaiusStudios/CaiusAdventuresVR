using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(TimeToSelfDestruct()); // wait for 5 seconds
    }
    
    // a corountine that suspends execution for a given amount of seconds
    private IEnumerator TimeToSelfDestruct()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

}
