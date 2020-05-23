using System;
using System.Collections;
using System.Collections.Generic;
using BT.Variables;
using BT.Core;
using UnityEngine;
using GameCreator.Characters;
using BT.Abilities;
using BT.Items;

namespace BT.Player
{
    public class PlayerController : MonoBehaviour, ICharacter
    {

        [SerializeField] BoolVariable resetGameData;
        
        [Header("Abilities")]
        public PlayerRunTimeData prtd;
        public Inventory inventory;

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

        private void Awake() 
        {
            myStates = FindObjectOfType<States>();
            if (resetGameData.value)
            {
                inventory.PurgeWands();
                PurgePlayerContent();
            }

        }

        private void Start() 
        {
            if (bDebug) Debug.Log("Player spawned.");
            pc = GetComponent<PlayerCharacter>();
            if (pc == null)
                Debug.LogError("No locomotion object on player.");
            defaultJump = pc.characterLocomotion.jumpForce;

            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

            health = GetComponent<Health>();

            if (prtd.passiveAbility != null)
                prtd.passiveAbility.EngageState(true);

        }

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

        private void UpdateTimers()
        {
            attackTimer += Time.deltaTime;
            defenseTimer += Time.deltaTime;
            utilityTimer += Time.deltaTime;
            regenTimer += Time.deltaTime;
        }

        private void PurgePlayerContent()
        {
            prtd.attackAbility = null;
            prtd.defenseAbility = null;
            prtd.utilityAbility = null;
            prtd.passiveAbility = null;

            myStates.ClearStates();

            prtd.activeWand = null;
        }
        
        #region Event Actions
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

        #endregion
        
        #region Activation Abilities
        private void ActivateLocation()
        {
            CharacterController cc = GetComponent<CharacterController>();
            if (cc == null) return;


            Collider[] colliders = Physics.OverlapSphere(transform.position, cc.radius);

            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<InteractionObject>() == null) continue;
                if (bDebug) Debug.Log("Activating Location: " + collider.gameObject.name + " with key " + prtd.utilityAbility.elementType);

                ElementType param;;
                if (prtd.utilityAbility==null)
                    param = ElementType.None;
                else
                    param = prtd.utilityAbility.elementType;
                    
                collider.GetComponent<InteractionObject>().ActivateObject(param);

            }

        }

        private void DefenseHandler()
        {
            if (prtd.defenseAbility == null) return;

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
            if (prtd.attackAbility == null) return;

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

        #endregion



        //Need to validate types are appropriate when using abilities/setting the ability?  I dunno.

        private void OnDrawGizmosSelected()
        {

            if ((prtd.attackAbility == null)||(prtd.defenseAbility == null)) return;

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