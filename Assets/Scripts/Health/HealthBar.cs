using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private HealthSystem _healthSystem;
    private Camera _mainCamera;


    private void Start()
    {
        _mainCamera = Camera.main;  // cache camera because it is slow
    }

    private void Update()
    {
        AlignHealthBar();
    }
    
    public void Setup(HealthSystem healthSystem)
    {
        _healthSystem = healthSystem;
        healthSystem.OnHealthChanged += HealthSystemOnOnHealthChanged;
        healthSystem.OnShowBar += HealthSystemOnOnShowBar;
    }
    private void HealthSystemOnOnHealthChanged(object sender, System.EventArgs e)
    {
        transform.Find("Bar").localScale = new Vector3(_healthSystem.GetHealthPercent(), 1);
    }

    private void HealthSystemOnOnShowBar(object sender, System.EventArgs e)
    {
        gameObject.SetActive(!gameObject.activeSelf); 
    }

    private void AlignHealthBar()
    {
        if (_mainCamera != null)
        {
            var camXform = _mainCamera.transform;
            var forward = transform.position - camXform.position;
            forward.Normalize();
            var up = Vector3.Cross(forward, camXform.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }
}
