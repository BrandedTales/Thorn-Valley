using System;
using UnityEngine;
using BT.Core;
using BT.Variables;

namespace BT.Enemies
{
    public class AIController : MonoBehaviour
    {

        #region Editor Fields
        [Header("Aggression")]
        [SerializeField] FloatReference chaseDistance;

        [SerializeField] FloatReference suspicionTime;
        [SerializeField] FloatReference fleeThreshold;
        [SerializeField] bool isAutoshooter;

        [Header("Movement")]
        [SerializeField] FloatReference waypointTolerance;
        [SerializeField] FloatReference dwellTime;
        [SerializeField] FloatReference patrolSpeedFraction;
        [Space(15)]
        [SerializeField] PatrolPath patrolPath;
        [Space(15)]

        [SerializeField] bool hasRandomMovement;
        [Range(0,2)][SerializeField] float minWanderDistance;
        [Range(2,4)][SerializeField] float maxWanderDistance;
        #endregion 

        Fighter fighter;
        Health health;
        GameObject player;
        Mover mover;
        Vector3 nextWaypoint;

        Vector3 guardPosition;
        float timeSinceLastSeenPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceLastShot = 0;

        int currentWaypointIndex = 0;

        [SerializeField] bool bDebug = true;

        private void Start() 
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
            nextWaypoint = guardPosition;

        }

        private void Update()
        {

            if (health.IsDead()) return;

            if ((!player.GetComponent<States>().GetState((int)PlayerPassive.Invisible))&&(health.HealthPercentage() < fleeThreshold))
            {
                if (bDebug) Debug.Log("Flee Behavior" + health.HealthPercentage());
                FleeBehavior();
            }
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                timeSinceLastSeenPlayer = 0;
                AttackBehavior();
            }
            else if (timeSinceLastSeenPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                MoveBehavior();
            }

            UpdateTimers();
        }

        private void FleeBehavior()
        {
            Vector3 direction = Vector3.Normalize(transform.position - player.transform.position);
            if (bDebug) Debug.Log(direction);
            mover.StartMoveAction(direction + transform.position, 1);
        }

        private void UpdateTimers()
        {
            timeSinceLastSeenPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceLastShot += Time.deltaTime;
        }

        private void MoveBehavior()
        {

            Vector3 nextPosition = guardPosition;

            if ((patrolPath != null)||(hasRandomMovement))
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                    timeSinceArrivedAtWaypoint = 0;
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArrivedAtWaypoint > dwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
            if (isAutoshooter&&(timeSinceLastShot > fighter.attackAbility.cooldown.value))
            {
                AttackBehavior();
                timeSinceLastShot = 0;
            }

        }

        private Vector3 GetCurrentWaypoint()
        {
            if (hasRandomMovement)
                return nextWaypoint;
            else
                return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            if (hasRandomMovement)
            {
                float newX = UnityEngine.Random.Range(minWanderDistance, maxWanderDistance) * ((int)UnityEngine.Random.Range(0, 2) * 2 - 1);
                float newZ = UnityEngine.Random.Range(minWanderDistance, maxWanderDistance) * ((int)UnityEngine.Random.Range(0, 2) * 2 - 1);
                Vector3 newLocation = new Vector3(newX, 0, newZ);
                if (bDebug) Debug.Log("Wander to: " + newLocation);
                nextWaypoint = transform.position + newLocation;
            }
            else
                currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWayPoint < waypointTolerance;
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {            

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

    }
}