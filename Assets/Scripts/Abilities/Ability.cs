﻿using System;
using System.Collections;
using System.Collections.Generic;
using BT.Variables;
using UnityEngine;

namespace BT.Abilities
{

    [CreateAssetMenu(fileName = "New Ability", menuName = "Thorn Valley/Create Ability")]
    public class Ability : ScriptableObject
    {
        public enum AbilityType { Attack, Defense, Utility, Passive }
        public enum ElementType { Air, Water, Fire, Earth, Life }
        public enum OutputType { SpawnObject, ActivateLocation, State}

        [Header("General Traits")]
        [SerializeField] StringReference abilityName;
        [SerializeField] AbilityType abilityType;
        [SerializeField] ElementType elementType;
        [SerializeField] OutputType outputType;

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
        //[SerializeField] AIBehavior aIBehavior;  //Haven't implemented AI behaviors yet.
        [Space(15)]

        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        public void Execute()
        {
            switch (outputType)
            {
                case OutputType.SpawnObject: SpawnAbility();
                    break;
                case OutputType.ActivateLocation: ActivateLocation();
                    break;

            }

        }


        private void SpawnAbility()
        {
            GameObject player = GameObject.FindWithTag("Player");

            if (player == null) return;

            Vector3 offsetVector = (player.transform.forward * offset) + new Vector3(0,1f,0);
            AbilitySpawn abilitySpawn = Instantiate(projectilePrefab, player.transform.position+offsetVector, player.transform.rotation);
            abilitySpawn.Initialize(this);
            if (isConnectedToPlayer)
                abilitySpawn.transform.parent = player.transform;
        }

        private void ActivateLocation()
        {
            if (bDebug) Debug.Log("Using a utility!.");
        }
    }
}