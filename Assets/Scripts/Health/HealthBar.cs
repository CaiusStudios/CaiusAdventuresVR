using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private HealthSystem _healthSystem;
    public void Setup(HealthSystem healthSystem)
    {
        _healthSystem = healthSystem;
        healthSystem.OnHealthChanged += HealthSystemOnOnHealthChanged;
    }
    private void HealthSystemOnOnHealthChanged(object sender, System.EventArgs e)
    {
        transform.Find("Bar").localScale = new Vector3(_healthSystem.GetHealthPercent(), 1);
    }
}
