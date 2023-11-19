using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BossBehavior : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;

    //layerCheck
    public LayerMask playerZone;

    //See
    private bool iSeeYou;
    public float seeDistance;

    //Meteor Attack
    public GameObject meteorPrefab;
    public Transform rightMeteor;
    public Transform leftMeteor;
    public Transform middleMeteor;
    public float meteorWindUp;
    public float timeBetweenMeteorAttack;
    public float meteorSpeed = 10;
    private bool meteorAttack = false;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponentInParent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        iSeeYou = Physics.CheckSphere(transform.position, seeDistance, playerZone);
        if(iSeeYou == true && meteorAttack == false)
        {
            transform.LookAt(player);
            StartCoroutine(PerformAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        meteorAttack = true;

        yield return new WaitForSeconds(meteorWindUp);

        summonMeteor(rightMeteor.position, Quaternion.identity);
        yield return new WaitForSeconds(timeBetweenMeteorAttack);

        summonMeteor(leftMeteor.position, Quaternion.identity);
        yield return new WaitForSeconds(timeBetweenMeteorAttack);

        summonMeteor(middleMeteor.position, Quaternion.identity);

        meteorAttack = false;
        
    }

    public void summonMeteor(Vector3 position, Quaternion rotation)
    {
        Rigidbody newMeteor = Instantiate(meteorPrefab, position, rotation).GetComponent<Rigidbody>();
        Vector3 directionToPlayer = (player.position - position).normalized;
        newMeteor.velocity = directionToPlayer * meteorSpeed;
        Destroy(newMeteor.gameObject, 5f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, seeDistance);
    }
}
