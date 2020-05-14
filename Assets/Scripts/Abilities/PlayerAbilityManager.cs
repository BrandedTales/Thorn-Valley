using System;
using System.Collections;
using System.Collections.Generic;
using BT.Variables;
using UnityEngine;

namespace BT.Abilities
{
    public class PlayerAbilityManager : MonoBehaviour
    {
        public Ability attackAbility;
        public Ability defenseAbility;
        public Ability utilityAbility;
        public Ability passiveAbility;

        float attackTimer = 0;
        float defenseTimer = 0;
        float utilityTimer = 0;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                AttackHandler();

            }

            if (Input.GetButtonDown("Fire2"))
            {
                DefenseHandler();
            }

            if (Input.GetButtonDown("Fire3"))
            {
                UtilityHandler();
            }

            UpdateTimers();

        }

        private void UpdateTimers()
        {
            attackTimer += Time.deltaTime;
            defenseTimer += Time.deltaTime;
            utilityTimer += Time.deltaTime;
        }

        private void UtilityHandler()
        {
            if (utilityTimer > utilityAbility.cooldown.value)
            {
                utilityTimer = 0;
                utilityAbility.Execute(this.gameObject);
            }
            else
            {
                Debug.Log("Cooldown: " + utilityTimer + "/" + utilityAbility.cooldown.value);
            }
        }

        private void DefenseHandler()
        {
            if (defenseTimer > defenseAbility.cooldown.value)
            {
                StartCoroutine(CastAbility(defenseAbility));
                defenseTimer = 0;
            }
            else
            {
                Debug.Log("Cooldown: " + defenseTimer + "/" + defenseAbility.cooldown.value);
            }
        }

        private void AttackHandler()
        {
            if (attackTimer > attackAbility.cooldown.value)
            {
                StartCoroutine(CastAbility(attackAbility));
                attackTimer = 0;
            }
            else
            {
                Debug.Log("Cooldown: " + attackTimer + "/" + attackAbility.cooldown.value);
            }
        }

        private IEnumerator CastAbility(Ability activeAbility)
        {
            yield return new WaitForSeconds(activeAbility.castTime.value);
            activeAbility.Execute(this.gameObject);
        }

        //Need to validate types are appropriate when using abilities/setting the ability?  I dunno.

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackAbility.explosionRadius.value);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, defenseAbility.explosionRadius.value);
        }

    }
}