using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Abilities
{
    [RequireComponent(typeof(PlayerAbilityManager))]
    public class AbilityInputHandler : MonoBehaviour
    {

        PlayerAbilityManager pam = null;
        float attackTimer = 0;
        float defenseTimer = 0;
        float utilityTimer = 0;

        private void Start() {
            pam = GetComponent<PlayerAbilityManager>();
        }
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
            if (utilityTimer > pam.utilityAbility.cooldown.value)
            {
                utilityTimer = 0;
                pam.utilityAbility.Execute();
            }
            else 
            {
                Debug.Log("Cooldown: " + utilityTimer + "/" + pam.utilityAbility.cooldown.value);
            }
        }

        private void DefenseHandler()
        {
            if (defenseTimer > pam.defenseAbility.cooldown.value)
            {
                defenseTimer = 0;
                pam.defenseAbility.Execute();
            }
            else
            {
                Debug.Log("Cooldown: " + defenseTimer + "/" + pam.defenseAbility.cooldown.value);
            }
        }

        private void AttackHandler()
        {
            if (attackTimer > pam.attackAbility.cooldown.value)
            {
                attackTimer = 0;
                pam.attackAbility.Execute();
            }
            else
            {
                Debug.Log("Cooldown: " + attackTimer + "/" + pam.attackAbility.cooldown.value);
            }
        }
    }
}