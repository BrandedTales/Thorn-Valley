using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BT.Core;


namespace BT.Enemies
{

    public class Mover : MonoBehaviour, IEnemyAction
    {

        NavMeshAgent navMeshAgent;
        Health health;

        float maxSpeed;

        bool isEntangled;

        [SerializeField] bool bDebug = true;

        private void Start() 
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            maxSpeed = navMeshAgent.speed;
            health = GetComponent<Health>();
        }
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            if (IsEntangled()) Cancel();

            UpdateAnimator();
        }

        private bool IsEntangled()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<CapsuleCollider>().radius);

            foreach (Collider collider in hitColliders)
            {
                if (collider.GetComponent<StopMovement>() != null) 
                    return true;
            }
            
            return false;

        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            GetComponent<Fighter>().Cancel();

            MoveTo(destination, speedFraction);
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            //float speed = localVelocity.z;
            //GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public void Cancel() 
        {
            navMeshAgent.isStopped = true;
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            if (bDebug) Debug.Log("Enemy is Entangled: " + IsEntangled());
            if (IsEntangled()) 
                Cancel();
            else
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
                navMeshAgent.destination = destination;
            }
        }

    }

}