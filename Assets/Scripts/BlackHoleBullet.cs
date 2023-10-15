using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleBullet : MonoBehaviour
{
    private Rigidbody bulletRigidbody;
    private Renderer bulletRenderer;
    public int eventHorizonRadius;
    public float growthTime;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 50f;
        bulletRigidbody.velocity = transform.forward * speed;
        Object.Destroy(gameObject,2.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        bulletRigidbody.constraints = RigidbodyConstraints.FreezePosition;
        StartCoroutine(ScaleOverTime(growthTime));
        transform.localScale = new Vector3 (eventHorizonRadius, eventHorizonRadius, eventHorizonRadius);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, eventHorizonRadius);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.tag == "Enemy")
            {
                Debug.Log("Woah an enemy!");
            }
        }
    }

    IEnumerator ScaleOverTime(float time)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 destinationScale = new Vector3(eventHorizonRadius, eventHorizonRadius, eventHorizonRadius);
        float currentTime = 0.0f;

        while(currentTime <= time)
        {
            Color objectColor = GetComponent<Renderer>().material.color;
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, 1 / (currentTime / time));
            GetComponent<Renderer>().material.color = objectColor;
            transform.localScale = Vector3.Lerp(destinationScale, originalScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, eventHorizonRadius);
    }
}
