using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleBullet : MonoBehaviour
{
    private Rigidbody bulletRigidbody;
    private Renderer bulletRenderer;
    public float eventHorizonRadius;
    public float effectTime;
    public float speed = 50f;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletRigidbody.velocity = transform.forward * speed;
        Object.Destroy(gameObject,2.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        bulletRigidbody.constraints = RigidbodyConstraints.FreezePosition; //Stops projectile
        StartCoroutine(ScaleOverTime(effectTime));
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, eventHorizonRadius);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.tag == "Enemy")
            {
                StartCoroutine(DestroyTarget(hitCollider));
            }
        }
    }

    IEnumerator ScaleOverTime(float time)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 destinationScale = new Vector3(eventHorizonRadius, eventHorizonRadius, eventHorizonRadius);
        float currentTime = 0.0f;

        transform.localScale = new Vector3 (eventHorizonRadius, eventHorizonRadius, eventHorizonRadius);
        
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

    IEnumerator DestroyTarget(Collider target)
    {
        float currentTime = 0.0f;
        Vector3 startPosition = target.transform.position;
        while(currentTime < (effectTime * .75))
        {
            target.transform.position = Vector3.Lerp(startPosition, transform.position, currentTime / (effectTime * .75f));
            currentTime += Time.deltaTime;
            yield return null;
        }
        Destroy(target.transform.parent.gameObject);
        Destroy(target.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, eventHorizonRadius);
    }
}
