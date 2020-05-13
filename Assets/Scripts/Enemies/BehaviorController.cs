using System;
using BT.Variables;
using UnityEngine;
using UnityEngine.AI;
using BT.Core;

namespace BT.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BehaviorController : MonoBehaviour
    {

        Health health;
        NavMeshAgent navMeshAgent;

        [Header("Patrol Behaviors")]
        [SerializeField] Transform[] pathOfInterest;
        [SerializeField] FloatReference waitDuringPatrols;
        [SerializeField] FloatReference distanceTolerance;
        [SerializeField] FloatReference walkSpeed;
        [SerializeField] bool oneWay = false;
        int wayPointIndex = 0;
        float patrolWaitTime;

        [Header("Chase Behaviors")]
        [SerializeField] FloatReference chaseRange;
        [SerializeField] FloatReference runSpeed;


        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        private void Start() 
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();

            if (pathOfInterest.Length > 0)
            {
                if (bDebug) Debug.Log("Initializing first waypoint.");
                navMeshAgent.speed = walkSpeed.value;
                navMeshAgent.destination = pathOfInterest[wayPointIndex].position;
            }
            patrolWaitTime = waitDuringPatrols.value;
        }

        private void Update() 
        {
            if  (ChaseBehavior()) return;
            PatrolBehavior();

        }

        #region Chase Behavior
        private bool ChaseBehavior()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if ((player!=null)&&(Vector3.Distance(transform.position, player.transform.position) < chaseRange.value))
            {
                navMeshAgent.destination = player.transform.position;
                navMeshAgent.speed = runSpeed;
                return true;
            }
            else 
            {
                navMeshAgent.speed = walkSpeed;
                navMeshAgent.destination = pathOfInterest[wayPointIndex].position;
                return false;
            }


        }

        #endregion

        #region Patrol Behavior

        private void PatrolBehavior()
        {
            navMeshAgent.speed = walkSpeed;
            float distanceToWP = Vector3.Distance(transform.position, pathOfInterest[wayPointIndex].position);
            if (bDebug) Debug.Log("Distance to WP: " + distanceToWP + " / " + distanceTolerance.value);

            if (distanceToWP < distanceTolerance.value)
            {
                if ((patrolWaitTime > 0) && (patrolWaitTime <= waitDuringPatrols.value))
                {
                    patrolWaitTime -= Time.deltaTime;
                }
                else if (patrolWaitTime <= 0)
                {
                    wayPointIndex = GetNextWayPoint();
                    if (bDebug) Debug.Log(pathOfInterest[wayPointIndex].gameObject.name);
                    navMeshAgent.destination = pathOfInterest[wayPointIndex].position;
                    patrolWaitTime = waitDuringPatrols.value;
                }
            }
        }

        private int GetNextWayPoint()
        {

            if (wayPointIndex + 1 == pathOfInterest.Length)
                return 0;
            else
                return wayPointIndex + 1;
        }

        #endregion
        
        private void OnDrawGizmos() {

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRange.value);
        }

    }
}