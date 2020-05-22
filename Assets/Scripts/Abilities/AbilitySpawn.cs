using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT.Variables;
using BT.Core;
using System;

namespace BT.Abilities
{
    public class AbilitySpawn : MonoBehaviour
    {

        Ability sourceAbility;
        GameObject sourceAttacker;

        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        // Update is called once per frame
        void Update()
        {
            if (sourceAbility == null) return;

            if (sourceAbility.speed > 0) Move();


        }

        public void Initialize(GameObject attacker, Ability ability)
        {
            sourceAttacker = attacker;
            sourceAbility = ability;


            StartCoroutine(DestroyMe());
        }
        
        private void Move()
        {
            transform.Translate(Vector3.forward * sourceAbility.speed * Time.deltaTime);
        }

        IEnumerator DestroyMe()
        {
            yield return new WaitForSeconds(sourceAbility.duration);

            //Blow it up if it's supposed to.
            if (sourceAbility.explosionRadius > 0)
            {
                Explode(null);
            }
            Destroy(gameObject);
        }

        public void Impact()
        {
            if (sourceAbility.isNeverImpact) return;
            //TODO: Add some VFX to represent the hit.
            
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other) 
        {

            //if (bDebug) Debug.Log("Came from: " + sourceAttacker.name + " and the hit: " + other.gameObject.name);
            //First make sure we didn't hit ourselves.
            if (other.gameObject == sourceAttacker.gameObject) return;
            //Check if we are hitting the player, and if the player is invulnerable.
            if ((other.tag == "Player")&&(other.GetComponent<States>().GetState((int)PlayerPassive.Invulnerable)))
            {
                Impact();
                return;
            }
            if ((sourceAbility.isPassThroughEnemies)&&(other.GetComponent<Health>() != null))
            {
                other.GetComponent<Health>().TakeDamage(sourceAbility.damage);
                //Potential bug here:  If it passes through, enemy takes damage, and then detonates next to enemy, he takes damage twice.
                //It's a moot point right now (since no abilities should have this problem), but something to be aware of.
            }
            else
            {

                //I really feel like there's a better way to refactor all this as I have duplicative if statements and duplicative code.
                if ((other.gameObject != sourceAttacker.gameObject) && (other.GetComponent<Health>() != null))
                {
                    other.GetComponent<Health>().TakeDamage(sourceAbility.damage);
                    if (sourceAbility.objectToSpawnOnImpact != null)
                        Instantiate(sourceAbility.objectToSpawnOnImpact, other.transform.position, Quaternion.identity);
                }
                if (sourceAbility.explosionRadius>0)
                {
                    Explode(other);
                }

                Impact();
            }

        }

        private void Explode(Collider other)
        {

            //TODO: Spawn some VFX here.

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, sourceAbility.explosionRadius);
            if (bDebug) Debug.Log("Blowing up on: " + hitColliders.Length + "  Radius = " + sourceAbility.explosionRadius + " dmg: " + sourceAbility.damage);

            foreach (Collider collider in hitColliders)
            {
                if (bDebug) Debug.Log("Exploding on: " + collider.gameObject.name);

                //Checking the "other" parameter so we don't do double-damage.
                if ((collider == other)||(collider.GetComponent<States>().GetState((int)PlayerPassive.Invulnerable))) continue;


                if ((other.gameObject != sourceAttacker.gameObject)&&(other.GetComponent<Health>() != null))
                {
                    collider.GetComponent<Health>().TakeDamage(sourceAbility.damage);
                    if (sourceAbility.objectToSpawnOnImpact != null)
                        Instantiate(sourceAbility.objectToSpawnOnImpact, collider.transform.position, Quaternion.identity);
                }
            }

        }

    }
}