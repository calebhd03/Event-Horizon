using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystalArmor : MonoBehaviour
{
    [SerializeField] int armorMaxHealth;
    private int shotOnArmor = 0;
    [SerializeField] crystalEnemy crystalEnemy;
    [SerializeField] AudioSource breakSound;
    [SerializeField] ParticleSystem breakParticle;
    [SerializeField] AudioSource hitSound;
    [SerializeField] GameObject model;
    [SerializeField] AudioClip[] crystalHitSounds;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            shotOnArmor += 1; 
            UpdateArmorHealth();
            Debug.Log("Shots on armor" + shotOnArmor);
        }
    }

    private void UpdateArmorHealth()
    {
        if (shotOnArmor == armorMaxHealth)
        {
            model.SetActive(false);
            GetComponent<Collider>().enabled = false;

            if (breakSound != null)  breakSound?.Play();
            if (breakParticle != null) breakParticle?.Play();

            
            crystalEnemy.ArmorBroke();
        }
        else
        {
            if(hitSound != null) hitSound.PlayOneShot(crystalHitSounds[Random.Range(0, crystalHitSounds.Length-1)]);
        }
    }
}
