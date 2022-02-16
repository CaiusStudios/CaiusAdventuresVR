using System;
using System.Collections;
using UnityEngine;
/// <summary>
///   <para> This represents the Skull (flying skulls).</para>
/// </summary>
public class SkullManager : Enemy
{
    public Material originalMaterial;
    public Transform pfHealthBar;
    public int skullMaxHealth = 100;
    public int skullStrength = 15;
    public float skullAttackRange = 2.5f;
    
    private float _redEffectDuration = 0.25f;
    public Transform DeathEffect;
    private static readonly int Color1 = Shader.PropertyToID("_Color");  // JetBrains Rider suggestion

    // Start is called before the first frame update
    void Start()
    {
        // basics of an enemy
        Strength = skullStrength;
        AttackRange = skullAttackRange;
        AttackPoint = transform;
        PlayerMask = LayerMask.GetMask("Player");
        SetupHealthSystem(pfHealthBar, skullMaxHealth);
        HealthSystem.OnHealthChanged += HealthSystemOnOnHealthChanged;
        HealthSystem.OnDeath += HealthSystemOnOnDeath;
    }



    /// <summary>
    /// Manage what happen on death (no health points left) of the skull
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HealthSystemOnOnDeath(object sender, EventArgs e)
    {
        Transform deathExplosion = Instantiate(
            DeathEffect,
            transform.position,
            transform.rotation,
            transform);

        ParticleSystem deathPS = deathExplosion.GetComponent<ParticleSystem>();
        deathPS.transform.SetParent(null);  // the particle system is not a child anymore
        deathPS.Play();
        Destroy(deathPS, deathPS.main.duration);  // particle effect dies when it ends 
        Die(gameObject);  // enemy dies/disables now
        
    }

    /// <summary>
    /// Responsible only to display a visual hit on this specific enemy when he's hit.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HealthSystemOnOnHealthChanged(object sender, EventArgs e)
    {
        // enemy flashes in red as it gets hit by the player
        GameObject skullHead = transform.Find("SkullHead").gameObject;

        skullHead.GetComponent<MeshRenderer>().material.SetColor(Color1, Color.red);
        if (gameObject.activeSelf)
        {
            StartCoroutine(ResetHeadColor(skullHead, originalMaterial.color, _redEffectDuration));
        }
    }

    /// <summary>
    /// Change the color of the head of skull back after some time (redEffectDuration).
    /// </summary>
    /// <param name="skullHead"></param>
    /// <param name="initCol"></param>
    /// <param name="redEffectDuration"></param>
    /// <returns></returns>
    IEnumerator ResetHeadColor(GameObject skullHead, Color initCol, float redEffectDuration)
    {
        yield return new WaitForSeconds(redEffectDuration);
        skullHead.GetComponent<MeshRenderer>().material.SetColor(Color1, initCol);
    }
}
