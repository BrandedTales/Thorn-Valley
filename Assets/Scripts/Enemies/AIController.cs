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

        [Header("Charmed")]
        [SerializeField] bool isCharmed = false;
        //[SerializeField] FloatReference followDistance;  Maybe later.
        [SerializeField] FloatReference charmDuration;

        Fighter fighter;
        Health health;
        GameObject target;
        Mover mover;
        Vector3 nextWaypoint;

        Vector3 guardPosition;
        float timeSinceLastSeenPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceLastShot = 0;
        float timeSinceCharmed = Mathf.Infinity;

        int currentWaypointIndex = 0;

        [SerializeField] bool bDebug = true;

        private void Start() 
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            target = GameObject.FindWithTag("Player");
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
            nextWaypoint = guardPosition;

        }

        public void BecomeCharmed()
        {
            if (bDebug) Debug.Log(gameObject.name + " has become charmed.");
            timeSinceCharmed = 0;
            isCharmed = true;
            target = null;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void Update()
        {

            if (bDebug&&(target!=null)) Debug.Log("Currently targetting: " + target.name);
            if (health.IsDead()) return;

            if (isCharmed)
            {
                if (bDebug) Debug.Log("Charmed " + gameObject.name + " finding a new target.");
                CharmedBehavior();
            }
            else if ((target.tag=="Player")&&(!target.GetComponent<States>().GetState((int)PlayerPassive.Invisible))&&(health.HealthPercentage() < fleeThreshold))
            {
                if (bDebug) Debug.Log("Flee Behavior" + health.HealthPercentage());
                FleeBehavior();
            }
            else if (InAttackRangeOfTarget(target) && fighter.CanAttack(target))
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

        private void CharmedBehavior()
        {
            if (target == null)
            {
                fighter.CharmFindTarget();
                target = fighter.GetTarget().gameObject;
            }
            if (timeSinceCharmed >= charmDuration.value)
            {
                isCharmed = false;
                target = GameObject.FindWithTag("Player");
            }
        }

        private void FleeBehavior()
        {
            Vector3 direction = Vector3.Normalize(transform.position - target.transform.position);
            if (bDebug) Debug.Log(direction);
            mover.StartMoveAction(direction + transform.position, 1);
        }

        private void UpdateTimers()
        {
            timeSinceLastSeenPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceLastShot += Time.deltaTime;
            timeSinceCharmed += Time.deltaTime;
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
            fighter.Attack(target);
        }

        private bool InAttackRangeOfTarget(GameObject target)
        {            

            float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
            return distanceToTarget < chaseDistance;
        }

        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

    }
}