using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BossBehavior : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;

    public Transform[] movePoints;
    private int destinationPoints = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponentInParent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {

            pointMovement();
            
        }
    }

    public void pointMovement()
    {
        //resets movement
        if (movePoints.Length == 0)
            return;


        agent.destination = movePoints[destinationPoints].position;

        destinationPoints = (destinationPoints + 1) % movePoints.Length;
        Debug.Log("moving to " + agent.destination);
    }
}
