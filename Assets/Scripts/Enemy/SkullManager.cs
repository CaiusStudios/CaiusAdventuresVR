using UnityEngine;
/// <summary>
///   <para> This represents the Skull (flying skulls).</para>
/// </summary>
public class SkullManager : Enemy
{
    public Transform pfHealthBar;
    public int MaxHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        // basics of an enemy
        AttackPoint = transform;
        PlayerMask = LayerMask.GetMask("Player");
        SetupHealthSystem(pfHealthBar, MaxHealth);
    }


}
