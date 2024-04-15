using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Anomaly : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] movePoints;
    private int destinationPoints = 0;

    [SerializeField] private EnemiesScanScript enemyScan;

    public bool check = false;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyScan = GetComponent<EnemiesScanScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyScan.Scanned)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.1f)
            {
                Move();
            }
        }
    }

    public void Move()
    {
        agent.destination = movePoints[destinationPoints].position;

        if (!agent.pathPending && agent.remainingDistance < 0.1f && destinationPoints == 0)
        {
            StartCoroutine(TurnOff());
            agent.isStopped = true; 
            return; 
        }

        destinationPoints = (destinationPoints + 1) % movePoints.Length;
    }

    private IEnumerator TurnOff()
    {
        check = true;
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }


}
