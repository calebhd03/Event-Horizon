using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverHeadTrigger : MonoBehaviour
{
    public bool inRadius = false;
    public bool atActivated = false;
    private WaitForSeconds delay = new WaitForSeconds(2f);
    public float Damage = 5f;
    public float damageInterval = 1f;
    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (atActivated)
        {
            timer += Time.deltaTime;
            if (timer >= damageInterval)
            {
                timer = 0f;
                DoDamage();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            {
                inRadius = true;
                StartCoroutine(ActivateAfterDelay());
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            {
                inRadius = false;
                atActivated = false;
                StopCoroutine(ActivateAfterDelay());
            }
        }
    }

    IEnumerator ActivateAfterDelay()
    {
        yield return delay;
        if (inRadius)
        {
            atActivated = true;
        }
    }

    private void DoDamage()
    {
        if (atActivated)
        {
            ApplyDamage();
        }
    }

    private void ApplyDamage()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerHealthMetric playerHealthMetric = player.GetComponent<PlayerHealthMetric>();

            if (playerHealthMetric != null)
            {
                playerHealthMetric.ModifyHealth(-Damage);
            }
        }
    }
}
