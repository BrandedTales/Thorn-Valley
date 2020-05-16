using System;
using System.Collections;
using System.Collections.Generic;
using BT.Variables;
using BT.Core;
using UnityEngine;
using UnityEngine.AI;
using GameCreator.Characters;

namespace BT.Abilities
{
    public class PlayerAbilityManager : MonoBehaviour
    {
        [Header("Abilities")]
        public Ability attackAbility;
        public Ability defenseAbility;
        public Ability utilityAbility;
        public Ability passiveAbility;

        [Header("State Details")]
        public FloatReference superJump;
        public GameObject lightSource;
        [Space(15)]
        public FloatReference regenTickAmount;
        public FloatReference regenHealthAmount;

        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        States myStates;
        Health health;
        PlayerCharacter pc;
        float defaultJump;

        float attackTimer = 0;
        float defenseTimer = 0;
        float utilityTimer = 0;
        float regenTimer = 0;

        private void Start() 
        {
            pc = GetComponent<PlayerCharacter>();
            if (pc == null)
                Debug.LogError("No locomotion object on player.");
            defaultJump = pc.characterLocomotion.jumpForce;
            
            myStates = GetComponent<States>();

            myStates.StateEngaged += UpdateJump;
            myStates.StateEngaged += UpdateLight;

            health = GetComponent<Health>();
            //Test Code until Wands have been created.
            EquipPassive();
            

        }

        #region Methods from Event Invokes
        private void UpdateJump()
        {
            if (bDebug) Debug.Log("Jump event called.");
            if (myStates.GetState((int)PlayerPassive.Jump))
            {
                pc.characterLocomotion.jumpForce = superJump.value;
            }
            else
            {
                pc.characterLocomotion.jumpForce = defaultJump;
            }
        }

        private void UpdateLight()
        {
            if (bDebug) Debug.Log("Light event called.");
            lightSource.SetActive(myStates.GetState((int)PlayerPassive.Light));


        }

        #endregion

        #region Test Code Methods to validate functionality.
        private void EquipPassive()
        {
            if (passiveAbility == null) return;
            passiveAbility.EngageState();
        }

        #endregion

        // Update is called once per frame
        void Update()
        {

            if ((myStates.GetState((int)PlayerPassive.Regen))&&(regenTimer >= regenTickAmount.value))
            {
                regenTimer = 0;
                health.RecoverHealth(regenHealthAmount.value);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                AttackHandler();
                if (GetComponent<States>().GetState((int)PlayerPassive.Invisible))
                    Debug.Log("Invisible!");

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
            regenTimer += Time.deltaTime;
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
                if (bDebug) Debug.Log("Cooldown: " + utilityTimer + "/" + utilityAbility.cooldown.value);
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
                if (bDebug) Debug.Log("Cooldown: " + defenseTimer + "/" + defenseAbility.cooldown.value);
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
                if (bDebug) Debug.Log("Cooldown: " + attackTimer + "/" + attackAbility.cooldown.value);
            }
        }

        private IEnumerator CastAbility(Ability activeAbility)
        {
            yield return new WaitForSeconds(activeAbility.castTime.value);
            if (activeAbility.outputType==OutputType.SpawnObject)
                activeAbility.Execute(this.gameObject);
            else if (activeAbility.outputType == OutputType.State)
            {
                if (bDebug) Debug.Log("Triggering " + activeAbility.passiveState);
                myStates.SetState((int)activeAbility.passiveState, true);
                yield return new WaitForSeconds(activeAbility.stateDuration.value);
                if (bDebug) Debug.Log("Turning off " + activeAbility.passiveState);
                myStates.SetState((int)activeAbility.passiveState, false);

            }
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