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

        [Header("Aggression Behaviors")]
        [Tooltip("Attack only if provoked")]
        [SerializeField] bool isAttackingOnlyWhenProvoked;
        [Tooltip("Use a whole number for percentage health")]
        [SerializeField] FloatReference fleeThreshold;
        [Tooltip("How close until we trigger the aggression behavior")]
        [SerializeField] FloatReference chaseRange;
        [Tooltip("How fast does the enemy move when engaging aggression behavior")]
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
            if (AggressionBehavior()) return;
            MoveBehavior();

        }


        #region Aggression Behavior
        private bool AggressionBehavior()
        {
            if (isAttackingOnlyWhenProvoked&&(health.HealthPercentage() == 100)) return false;

            //If enemy is close to the player, check his health.  If it's low, run.  If it's high, chase.
            GameObject player = GameObject.FindWithTag("Player");
            if ((player != null) && (Vector3.Distance(transform.position, player.transform.position) < chaseRange.value))
            {
                if (health.HealthPercentage() < fleeThreshold.value)
                {
                    if (bDebug) Debug.Log("Health:" + health.HealthPercentage());

                    Vector3 direction = Vector3.Normalize(player.transform.position - transform.position);
                    navMeshAgent.destination = (direction * -1 * chaseRange.value);
                    navMeshAgent.speed = runSpeed;

                }
                else
                {
                    navMeshAgent.destination = player.transform.position;
                    navMeshAgent.speed = runSpeed;
                }
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

        #region Move Behavior

        private void MoveBehavior()
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