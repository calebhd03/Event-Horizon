using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class QuantumStabilizerPickup : MonoBehaviour
{
    [SerializeField] float hideAfter;
    [SerializeField] GameObject model;
    [SerializeField] AudioSource audioSource;

    float interactRange = 2f;
    bool pickedUp = false;

    Collider[] colliderArray;
    StarterAssetsInputs starterAssetsInputs;
    MiniCore miniCore;
    LogSystem LogSystem;

    void Awake()
    {
        miniCore = FindObjectOfType<MiniCore>();
        LogSystem = miniCore.GetComponentInChildren<LogSystem>();
        starterAssetsInputs = miniCore.GetComponentInChildren<StarterAssetsInputs>();
    }

    void Update()
    {
        colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
            if (collider.tag == "Player")
            {
                if (starterAssetsInputs.interact)
                {
                    //  if(starterAssetsInputs.interact == true)
                    // {
                    //starterAssetsInputs.interact = false;
                    // }
                    Pickup();
                }
            }
    }
    void Pickup()
    {
        if (pickedUp) return;
        pickedUp = true;

        LogSystem.number = 20;
        LogSystem.UpdateSkillsLog();

        audioSource.Play();

        model.SetActive(false);
        Invoke("HideGameObject", hideAfter);
    }
    void HideGameObject()
    {
        gameObject.SetActive(false);
    }
}
