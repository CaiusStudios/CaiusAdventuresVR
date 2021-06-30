using System;
using UnityEngine;
using UnityEngine.UI;

public class DiscoverPads : MonoBehaviour
{
    public bool freezeMovements;
    public float speed = 8.0f;
    public float rotationSpeed = 100.0f;
    public Animator animator;
    private Vector3 _inputVector;
    
    public float sensitivity = 25.0f;
    CharacterController character;
    public GameObject cam;
    private float _moveFb, _moveLr;
    private float _rotX, _rotY;
    public bool webGLRightClickRotation = true;
    private float _gravity = -9.8f;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        character = GetComponent<CharacterController> ();
        if (Application.isEditor) {
            webGLRightClickRotation = false;
            sensitivity = sensitivity * 1.5f;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (freezeMovements)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (!freezeMovements)  // external script can block the movements. e.g. when dialogues are on display
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 15.0f;
            }
            else
            {
                speed = 8.0f;
            }
            MoveThePlayer();
        }
    }

    // Complete Player movements
    void MoveThePlayer()
    {
        _moveFb = Input.GetAxis ("Horizontal") * speed;
        _moveLr = Input.GetAxis ("Vertical") * speed;

        _rotX = Input.GetAxis ("Mouse X") * sensitivity;
        _rotY = Input.GetAxis ("Mouse Y") * sensitivity;
        
        Vector3 movement = new Vector3 (_moveFb, _gravity, _moveLr);
        
        // play animation when moving forward
        AnimateForwardPlayer(_moveLr > 0);

        // Camera
        CameraRotation (cam, _rotX, _rotY);
        if (webGLRightClickRotation) {  // extra step when we play in the WebGL instead of Editor
            if (Input.GetKey (KeyCode.Mouse0)) {
                CameraRotation (cam, _rotX, _rotY);
            }
        } else if (!webGLRightClickRotation) {
            CameraRotation (cam, _rotX, _rotY);
        }

        movement = transform.rotation * movement;
        character.Move (movement * Time.deltaTime);
    }
    
    // Camera rotation
    void CameraRotation(GameObject cam, float rotX, float rotY){
        
        transform.Rotate (0, rotX * Time.deltaTime, 0);
        cam.transform.Rotate (-rotY * Time.deltaTime, 0, 0);
    }
    
    // A simple animation to move the sword
    void AnimateForwardPlayer(bool isMovingForward)
    {
        if (isMovingForward)
        {
            animator.SetBool("SwingWalk", true);
        }
        else
        {
            animator.SetBool("SwingWalk", false);
        }
        
    }

    // Canvas' "OptionsMenu" has the ability to change mouse's sensitivity.
    public void UpdateSensitivtyFromOptionsMenu()
    {
        float sens = GameObject.Find("SliderSensitivityMouse").GetComponent<Slider>().value;
        sensitivity = sens;
    }
    
}
