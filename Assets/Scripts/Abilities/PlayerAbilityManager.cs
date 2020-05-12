using System;
using System.Collections;
using System.Collections.Generic;
using BT.Variables;
using UnityEngine;

namespace BT.Abilities
{
    public class PlayerAbilityManager : MonoBehaviour
    {

        [SerializeField] Transform projectileSpawn;

        public Ability attackAbility;
        public Ability defenseAbility;
        public Ability utilityAbility;
        public Ability passiveAbility;

        private void Start() {
            SetAbilities();
        }

        private void SetAbilities()
        {
            if (attackAbility != null) attackAbility.projectileSpawn = projectileSpawn;
            if (defenseAbility != null) defenseAbility.projectileSpawn = projectileSpawn;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackAbility.explodeRadius.value);
        }

        //Need to validate types are appropriate when using abilities/setting the ability?  I dunno.

    }
}