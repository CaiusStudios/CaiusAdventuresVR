using UnityEngine;

public class Backpack : MonoBehaviour
{
    public OVRGrabbable spawnObject;
    private const int MAXSword = 1;
    private int _currentSword; // initialized with value 0

    private void OnTriggerEnter(Collider theCollider)
    {
        //  consider only GameObject with tag "grabber"
        if (!theCollider.gameObject.CompareTag("grabber")) return;
        
        // the player only has a MAXSword GameObject available in his "Backpack"
        if (_currentSword > MAXSword) return;

        var transform1 = transform;
        OVRGrabbable spawned = Instantiate(spawnObject, transform1.position, transform1.rotation);
        // spawned.GrabBegin(theCollider.gameObject.GetComponent<OVRGrabber>, theCollider);
        _currentSword++;

    }
}
