using System.Collections;
using UnityEngine;

public class RaycastBow : MonoBehaviour
{
    public int gunDamage = 1;
    public float fireRate = .25f;  // wait 0.25 seconds before fireing again
    public float fireSpeed = 300.0f;  // travel speed of the projectile / arrow / 
    public float weaponRange = 50f;  // how far the ray casts into the scene: 50 units of range
    public float hitForce = 100f;  // apply a force of hitForce to the gameobject hit with ray
    public Transform swordEnd;
    public Transform arrowPrefab;

    private Camera _fpsCam;
    private readonly WaitForSeconds _shotDuration = new WaitForSeconds(.07f);
    private AudioSource gunAudio;
    private LineRenderer _laserLine;
    private float _nextFire;
    private Vector3 _arrowTarget;
    private Transform _arrowInstantiated;

    // Start is called before the first frame update
    void Start()
    {
        //_arrowInstantiated = arrowPrefab;
        
        _laserLine = GetComponent<LineRenderer>();
        //gunAudio = GetComponent<AudioSource>();
        _fpsCam = Camera.main;  // GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetButtonDown("Fire1") && Time.time > _nextFire)
        if (OVRInput.GetDown(OVRInput.Button.One) && Time.time > _nextFire)
        {
            _nextFire = Time.time + fireRate;

            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = _fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            
            _laserLine.SetPosition(0, swordEnd.position);

            if (Physics.Raycast(rayOrigin, _fpsCam.transform.forward, out hit, weaponRange))
            {
                Debug.Log("[RaycastBow] something got hit... " + hit.collider.name);
                _laserLine.SetPosition(1, hit.point);
                _arrowTarget = hit.point;

                Destroyable objectDestroyable = hit.collider.GetComponentInParent<Destroyable>();
                
                _arrowInstantiated = Instantiate(arrowPrefab, swordEnd);

                if (objectDestroyable != null)
                {
                    objectDestroyable.DestroyWithFragments();
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }
            }
            else
            {
                _laserLine.SetPosition(1, rayOrigin + (_fpsCam.transform.forward * weaponRange));
            }
        } // end Fire1 button check
        
        if (_arrowInstantiated != null)
        {
            // _arrowInstantiated.position = Vector3.MoveTowards(
            //     _arrowInstantiated.position, 
            //     _arrowTarget, //_arrowInstantiated.position + new Vector3(0, 0, 10), 
            //     fireSpeed * Time.deltaTime);

            _arrowInstantiated.position = Vector3.Lerp(
                _arrowInstantiated.position,
                _arrowTarget,
                fireSpeed * Time.deltaTime);
        }
    }

    private IEnumerator ShotEffect()
    {
        //gunAudio.Play();
        _laserLine.enabled = true;
        yield return _shotDuration;
        _laserLine.enabled = false;
    }
}
