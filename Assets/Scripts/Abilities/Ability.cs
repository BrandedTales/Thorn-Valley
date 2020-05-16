using System;
using System.Collections;
using System.Collections.Generic;
using BT.Variables;
using BT.Core;
using UnityEngine;

namespace BT.Abilities
{

    public enum OutputType { SpawnObject, ActivateLocation, State }

    [CreateAssetMenu(fileName = "New Ability", menuName = "Thorn Valley/Create Ability")]
    public class Ability : ScriptableObject
    {
        public enum AbilityType { Attack, Defense, Utility, Passive }
        public enum ElementType { Air, Water, Fire, Earth, Life }

        [Header("General Traits")]
        [SerializeField] StringReference abilityName;
        [SerializeField] AbilityType abilityType;
        [SerializeField] ElementType elementType;
        public OutputType outputType;

        [Header("Passive State Options")]
        public PlayerPassive passiveState;
        public FloatReference stateDuration;

        [Header("Casting Details")]
        public FloatReference castTime;
        public FloatReference cooldown;

        [Header("Spawn Details")]
        [SerializeField] AbilitySpawn projectilePrefab;
        [SerializeField] FloatReference offset;
        [SerializeField] bool isConnectedToPlayer;
        public FloatReference duration;
        public FloatReference speed;
        
        [Header("Combat Implications")]
        public FloatReference damage;
        public FloatReference triggerInterval;

        [Header("Impact Effects")]
        public GameObject objectToSpawnOnImpact;
        public FloatReference explosionRadius;
        public bool isPassThroughEnemies;
        public bool isNeverImpact;

        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        public void Execute(GameObject abilityOrigin)
        {
            switch (outputType)
            {
                case OutputType.SpawnObject: SpawnAbility(abilityOrigin);
                    break;
                case OutputType.ActivateLocation: ActivateLocation();
                    break;
            }

        }

        public void EngageState()
        {
            States playerStates = GameObject.FindWithTag("Player").GetComponent<States>();
            if (playerStates != null)
            {
                //playerStates.ClearStates();
                playerStates.SetState((int)passiveState, true);
            }
        }

        private void SpawnAbility(GameObject shooterOrigin)
        {
            if (bDebug) Debug.Log("Shot fired from" + shooterOrigin.tag);

            if (shooterOrigin == null) return;

            Vector3 offsetVector = (shooterOrigin.transform.forward * offset) + new Vector3(0,1f,0);
            AbilitySpawn abilitySpawn = Instantiate(projectilePrefab, shooterOrigin.transform.position+offsetVector, shooterOrigin.transform.rotation);
            Vector3 eulers = abilitySpawn.transform.eulerAngles;
            eulers.x = 0;
            eulers.z = 0;
            abilitySpawn.transform.eulerAngles = eulers;

            abilitySpawn.Initialize(shooterOrigin, this);
            if (isConnectedToPlayer)
                abilitySpawn.transform.parent = shooterOrigin.transform;
        }

        private void ActivateLocation()
        {
            if (bDebug) Debug.Log("Using a utility!");
        }
    }
}