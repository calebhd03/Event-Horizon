using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


public class TachyonTranslocator : MonoBehaviour
{
    public StarterAssetsInputs starterAssetsInputs;
    PlayerHealthMetric playerHealthMetric;
    GameObject player;
    public Collider[] colliderArray;
    float interactRange = 2f;
    public bool hasCompass = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerHealthMetric = player.GetComponent<PlayerHealthMetric>();
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();

        if (playerHealthMetric.playerData.hasCompass == true)
        {
           hasCompass = true;
        }
        else
        {
            hasCompass = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
            if (collider.tag == "Player")
            {
                if (starterAssetsInputs.interact == true)
                {
                    GetCompass();
                }
            }
    }

    void GetCompass()
    {
        hasCompass = true;
        playerHealthMetric.playerData.hasCompass = true;
        //Destroy(gameObject);
    }
}
