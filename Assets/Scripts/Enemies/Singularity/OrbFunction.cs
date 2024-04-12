using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbFunction : MonoBehaviour
{
    public Transform Singularity;
    public float moveSpeed = 5;
    public bool hit = false;

    public StarterAssetsInputs starterAssetsInputs;
    GameObject player;
    public Collider[] colliderArray;
    float interactRange = 2f;
    public static int orbCount = 0;

    private void Awake()
    {
        Singularity = GameObject.Find("Singularity2").transform;

        player = GameObject.FindWithTag("Player");
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(hit)
        {
            Vector3 direction = Singularity.position -  transform.position;
            direction.Normalize();
            transform.position += direction * Time.deltaTime * moveSpeed;
        }

        colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
            if (collider.tag == "Player")
            {
                if (starterAssetsInputs.interact == true)
                {
                    GetOrb();
                }
            }
    }

    void GetOrb()
    {
        gameObject.SetActive(false);
        orbCount = orbCount + 1;
        Debug.Log("Total Orbs " + orbCount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") || other.CompareTag("BHBullet") || other.CompareTag("Plasma Bullet"))
        {
            Debug.Log("Bullet hit and orb shot");
            hit = true;   
        }

        if(other.CompareTag("WeakPoint") || other.CompareTag("CriticalPoint"))
        {
            gameObject.SetActive(false);  
        }
    }
}
