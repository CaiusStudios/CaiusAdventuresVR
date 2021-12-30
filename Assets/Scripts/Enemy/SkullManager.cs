using System;
using System.Collections;
using UnityEngine;
/// <summary>
///   <para> This represents the Skull (flying skulls).</para>
/// </summary>
public class SkullManager : Enemy
{
    public Transform pfHealthBar;
    public Camera CenterEyeCamera;
    public int maxHealth = 100;
    private float _redEffectDuration = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        // basics of an enemy
        pfHealthBar.rotation = CenterEyeCamera.GetComponent<Camera>().transform.rotation; 
        AttackPoint = transform;
        PlayerMask = LayerMask.GetMask("Player");
        SetupHealthSystem(pfHealthBar, maxHealth);
        HealthSystem.OnHealthChanged += HealthSystemOnOnHealthChanged;
        HealthSystem.Damage(10);
    }

    private void Update()
    {
        // TODO: à fixer/ trouver une autre solution pour que la bar soit face au player.
        pfHealthBar.rotation = CenterEyeCamera.GetComponent<Camera>().transform.rotation;
    }

    /// <summary>
    /// Responsible only to display a visual hit on this specific enemy when he's hit.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HealthSystemOnOnHealthChanged(object sender, EventArgs e)
    {
        // color the enemy in red? make shake? break?
        GameObject skullHead = transform.Find("SkullHead").gameObject;
        Color skullHeadColor = skullHead.GetComponent<MeshRenderer>().material.color;
        Color skullHeadColorOriginal = skullHeadColor;
        
        skullHead.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        StartCoroutine(ResetHeadColor(skullHead, skullHeadColorOriginal, _redEffectDuration));
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
        skullHead.GetComponent<MeshRenderer>().material.SetColor("_Color", initCol);
    }
}
