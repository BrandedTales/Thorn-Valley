using System;
using UnityEngine;
using BT.Core;
using BT.Variables;

namespace BT.Enemies
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] FloatReference chaseDistance;
        [SerializeField] FloatReference suspicionTime;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] FloatReference waypointTolerance;
        [SerializeField] FloatReference dwellTime;

        [SerializeField] FloatReference patrolSpeedFraction;
        [SerializeField] FloatReference fleeThreshold;

        [SerializeField] bool isAutoshooter;

        Fighter fighter;
        Health health;
        GameObject player;
        Mover mover;

        Vector3 guardPosition;
        float timeSinceLastSeenPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceLastShot = 0;

        int currentWaypointIndex = 0;


        private void Start() 
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
        }

        private void Update()
        {

            if (health.IsDead()) return;

            if (health.HealthPercentage() < fleeThreshold)
            {
                Debug.Log("Flee Behavior" + health.HealthPercentage());
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
                PatrolBehavior();
            }

            UpdateTimers();
        }

        private void FleeBehavior()
        {
            Vector3 direction = Vector3.Normalize(transform.position - player.transform.position);
            Debug.Log(direction);
            mover.StartMoveAction(direction + transform.position, 1);
        }

        private void UpdateTimers()
        {
            timeSinceLastSeenPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceLastShot += Time.deltaTime;
        }

        private void PatrolBehavior()
        {

            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
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
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
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