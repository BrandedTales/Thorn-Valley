using System;
using System.Collections;
using System.Collections.Generic;
using BT.Variables;
using BT.Core;
using UnityEngine;
using GameCreator.Characters;
using BT.Abilities;

namespace BT.Player
{
    public class PlayerController : MonoBehaviour, ICharacter
    {

        [SerializeField] BoolVariable resetGameData;
        
        [Header("Abilities")]
        public PlayerRunTimeData prtd;

        [Header("State Details")]
        public FloatReference superJump;
        public GameObject lightSource;

        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        PlayerCharacter pc;
        float defaultJump;

        float attackTimer = 0;
        float defenseTimer = 0;
        float utilityTimer = 0;
        float regenTimer = 0;

        States myStates;
        Health health;


        bool isGamePaused;

        private void Start() 
        {
            pc = GetComponent<PlayerCharacter>();
            if (pc == null)
                Debug.LogError("No locomotion object on player.");
            defaultJump = pc.characterLocomotion.jumpForce;

            myStates = FindObjectOfType<States>();
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

            health = GetComponent<Health>();

            //Test Code until Wands have been created.
            EquipPassive();
        }

        #region Methods from Event Calls
        public void UpdateJump()
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

        public void UpdateLight()
        {
            if (bDebug) Debug.Log("Light event called.");
            lightSource.SetActive(myStates.GetState((int)PlayerPassive.Light));


        }

        public void lockPlayerInput()
        {
            isGamePaused = true;
            pc.characterLocomotion.SetIsControllable(false);
        }

        public void unlockPlayerInput()
        {
            isGamePaused = false;
            pc.characterLocomotion.SetIsControllable(true);
        }

        #endregion

        #region Test Code Methods to validate functionality.
        private void EquipPassive()
        {
            if (prtd.passiveAbility == null) return;
            prtd.passiveAbility.EngageState();
        }

        #endregion

        // Update is called once per frame
        void Update()
        {

            if (isGamePaused) return;

            if (myStates.GetState((int)PlayerPassive.Regen)) regenTimer = health.RegenHealth(regenTimer);

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
                ActivateLocation();
            }

            UpdateTimers();

        }

        private void ActivateLocation()
        {
            CharacterController cc = GetComponent<CharacterController>();
            if (cc == null) return;


            Collider[] colliders = Physics.OverlapSphere(transform.position, cc.radius);

            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<InteractionObject>() == null) continue;
                if (bDebug) Debug.Log("Activating Location: " + collider.gameObject.name + " with key " + prtd.utilityAbility.elementType);
                collider.GetComponent<InteractionObject>().ActivateObject(prtd.utilityAbility.elementType);

            }

        }

        private void UpdateTimers()
        {
            attackTimer += Time.deltaTime;
            defenseTimer += Time.deltaTime;
            utilityTimer += Time.deltaTime;
            regenTimer += Time.deltaTime;
        }

        private void DefenseHandler()
        {
            if (defenseTimer > prtd.defenseAbility.cooldown.value)
            {
                StartCoroutine(CastAbility(prtd.defenseAbility));
                defenseTimer = 0;
            }
            else
            {
                if (bDebug) Debug.Log("Cooldown: " + defenseTimer + "/" + prtd.defenseAbility.cooldown.value);
            }
        }

        private void AttackHandler()
        {
            if (attackTimer > prtd.attackAbility.cooldown.value)
            {
                StartCoroutine(CastAbility(prtd.attackAbility));
                attackTimer = 0;
            }
            else
            {
                if (bDebug) Debug.Log("Cooldown: " + attackTimer + "/" + prtd.attackAbility.cooldown.value);
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
            Gizmos.DrawWireSphere(transform.position, prtd.attackAbility.explosionRadius.value);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, prtd.defenseAbility.explosionRadius.value);
        }

        public void Die()
        {
            //Die!
        }
    }
}