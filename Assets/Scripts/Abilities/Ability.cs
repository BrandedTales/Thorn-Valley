using System;
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

        [Header("General Traits")]
        [SerializeField] StringReference abilityName;
        [SerializeField] AbilityType abilityType;
        [SerializeField] ElementType elementType;
        [Space(15)]
        public FloatReference castTime;
        public FloatReference cooldown;
        [SerializeField] FloatReference duration;
        [Space(15)]
        [HideInInspector] public Transform projectileSpawn;
        [SerializeField] Projectile projectilePrefab;

        [Header("Attack")]
        [SerializeField] FloatReference damage;
        [SerializeField] FloatReference travelSpeed;
        [SerializeField] bool isPassThrough;

        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        public void Execute()
        {
            switch (abilityType)
            {
                case AbilityType.Attack:
                case AbilityType.Defense: SpawnAbility();
                    break;
                case AbilityType.Utility: ActivateUtility();
                    break;

            }

        }

        private void SpawnAbility()
        {

            Projectile projectile = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
            projectile.InitializeProjectile(travelSpeed.value, duration.value, damage.value, isPassThrough);
        }

        private void ActivateUtility()
        {
            if (bDebug) Debug.Log("Using a utility!.");
        }
    }
}