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
    private Vector3 lastPosition;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletRigidbody.velocity = transform.forward * speed;
        Object.Destroy(gameObject,2.0f);
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        int layerMask =~ LayerMask.GetMask("Bullets", "Player");
        if(Physics.Linecast(transform.position, lastPosition, out RaycastHit hitInfo, layerMask))
        {
            OnTriggerEnter(hitInfo.collider);
            Debug.Log("Raycast triggered");
        }
        lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("hit " + other);
        bulletRigidbody.constraints = RigidbodyConstraints.FreezePosition; //Stops projectile
        transform.position = lastPosition;
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

        while(currentTime <= time)
        {
            while(currentTime <= time * .1)
            {
                transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time * 10);
                currentTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = Vector3.Lerp(destinationScale, Vector3.zero, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator DestroyTarget(Collider target)
    {
        if(target.tag == "Enemy")
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, eventHorizonRadius);
    }
}
