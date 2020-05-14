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

        private void Start() 
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            maxSpeed = navMeshAgent.speed;
            health = GetComponent<Health>();
        }
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();

            UpdateAnimator();
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
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.destination = destination;
        }


    }

}