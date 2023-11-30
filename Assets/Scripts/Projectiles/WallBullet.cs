using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBullet : MonoBehaviour
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
        Debug.Log("WallBullet");
    }

    private void FixedUpdate()
    {
        int layerMask = ~(LayerMask.GetMask("Bullets", "CheckPoints", "Player", "GunLayer","WallBullet"));
        if (Physics.Linecast(transform.position, lastPosition, out RaycastHit hitInfo, layerMask))
        {
            transform.position = lastPosition;
            OnTriggerEnter(hitInfo.collider);
            Debug.Log("Raycast triggered");
        }
        lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {

         int layerMask = other.gameObject.layer;
        // Check if the collider is on any of the specified layers
        if (layerMask == LayerMask.NameToLayer("Bullets") ||
            layerMask == LayerMask.NameToLayer("CheckPoints") ||
            layerMask == LayerMask.NameToLayer("Player") ||
            layerMask == LayerMask.NameToLayer("GunLayer")||
            layerMask == LayerMask.NameToLayer("WallBullet"))
        {
            // Do nothing if the collider is on the specified layers
            return;
        }
             
        GameObject otherObject = other.gameObject;
         Debug.LogWarning("hit " + other);

        if (otherObject.CompareTag("Barrier"))
        {
            BarrierController barrierController = otherObject.GetComponent<BarrierController>();
            if (barrierController != null)
            {
                barrierController.DestroyBarrier();
            }
        }
        else if (otherObject.CompareTag("Enemy") || otherObject.CompareTag("WeakPoint"))
        {
           
            bulletRigidbody.constraints = RigidbodyConstraints.FreezePosition; // Stops projectile
            transform.position = lastPosition;
            StartCoroutine(ScaleOverTime(effectTime));

            // Only perform the overlap sphere logic if the tag is Enemy or WeakPoint
            if (otherObject.CompareTag("Enemy") || otherObject.CompareTag("WeakPoint"))
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, eventHorizonRadius);
                foreach (var hitCollider in hitColliders)
                {
                   
                }
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

    IEnumerator MoveTarget(Collider target)
    {
        Debug.Log("MoveTarget coroutine started");
        if(target.CompareTag("Enemy"))
        {
            Debug.Log("MoveTarget: Hit Enemy");
            float currentTime = 0.0f;
            Vector3 startPosition = target.transform.position;
            while(currentTime < (effectTime * .75))
            {
                target.transform.position = Vector3.Lerp(startPosition, transform.position, currentTime / (effectTime * .75f));
                currentTime += Time.deltaTime;
                yield return null;
            }
            Debug.Log("MoveTarget coroutine finished");
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, eventHorizonRadius);
    }
}