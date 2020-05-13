using System;
using BT.Variables;
using UnityEngine;
using UnityEngine.AI;

namespace BT.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BehaviorController : MonoBehaviour
    {

        NavMeshAgent navMeshAgent;

        [Header("Patrol Behaviors")]
        [SerializeField] Transform[] pathOfInterest;
        [SerializeField] FloatReference waitDuringPatrols;
        [SerializeField] FloatReference distanceTolerance;
        [SerializeField] bool oneWay = false;
        int wayPointIndex = 0;

        [Header("Chase Behaviors")]
        [SerializeField] FloatReference chaseRange;


        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        private void Start() 
        {
            navMeshAgent = GetComponent<NavMeshAgent>();

            if (pathOfInterest.Length > 0)
            {
                if (bDebug) Debug.Log("Initializing first waypoint.");
                navMeshAgent.destination = pathOfInterest[wayPointIndex].position;
            }
        }

        private void Update() 
        {
            if (IsPlayerSpotted()) ChaseBehavior();
            PatrolBehavior();
        }

        #region Chase Behavior
        private void ChaseBehavior()
        {
            
        }

        private bool IsPlayerSpotted()
        {
            return false;
        }
        #endregion

        #region Patrol Behavior

        private void PatrolBehavior()
        {
            float distanceToWP = Vector3.Distance(transform.position, pathOfInterest[wayPointIndex].position);
            Debug.Log("Distance to WP: " + distanceToWP + " / " + distanceTolerance.value);

            if (distanceToWP < distanceTolerance.value)
            {
                wayPointIndex = GetNextWayPoint();
                Debug.Log(pathOfInterest[wayPointIndex].gameObject.name);
                navMeshAgent.destination = pathOfInterest[wayPointIndex].position;
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
        

    }
}