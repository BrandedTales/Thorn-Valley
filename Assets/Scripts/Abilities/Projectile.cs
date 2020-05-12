using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT.Variables;
using BT.Core;

namespace BT.Abilities
{
    public class Projectile : MonoBehaviour
    {

        float speed;
        float lifeSpan;
        float damage;
        float explosionRadius;
        bool isPassthrough =false;
        GameObject objectToSpawnOnImpact = null;

        [SerializeField] bool bDebug = true;

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }


        public void InitializeProjectile(float speed, float lifeSpan, float damage, float explosionRadius, bool isPassthrough, GameObject objectToSpawnOnImpact)
        {
            this.speed = speed;
            this.lifeSpan = lifeSpan;
            this.damage = damage;
            this.explosionRadius = explosionRadius;
            this.isPassthrough = isPassthrough;

            if (objectToSpawnOnImpact != null)
                this.objectToSpawnOnImpact = objectToSpawnOnImpact;


            StartCoroutine(DestroyMe());
        }

        IEnumerator DestroyMe()
        {
            yield return new WaitForSeconds(lifeSpan);
            //Blow it up if it's supposed to.
            if (explosionRadius > 0)
            {
                Explode(null);
            }
            Destroy(gameObject);
        }

        public void Impact()
        {
            //TODO: Add some VFX to represent the hit.

            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (other.tag == "Player") return;

            if ((isPassthrough)&&(other.tag=="Enemy"))
            {
                other.GetComponent<Health>().TakeDamage(damage);
                //Potential bug here:  If it passes through, enemy takes damage, and then detonates next to enemy, he takes damage twice.
                //It's a moot point right now (since no abilities should have this problem), but something to be aware of.
            }
            else
            {

                //I really feel like there's a better way to refactor all this as I have duplicative if statements and duplicative code.
                if (other.tag == "Enemy")
                {
                    other.GetComponent<Health>().TakeDamage(damage);
                    if (objectToSpawnOnImpact != null)
                        Instantiate(objectToSpawnOnImpact, other.transform.position, Quaternion.identity);
                }
                if (explosionRadius>0)
                {
                    Explode(other);
                }

                Impact();
            }

        }

        private void Explode(Collider other)
        {

            //TODO: Spawn some VFX here.

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
            if (bDebug) Debug.Log("Blowing up on: " + hitColliders.Length + "  Radius = " + explosionRadius + " dmg: " + damage);

            foreach (Collider collider in hitColliders)
            {
                if (bDebug) Debug.Log("Exploding on: " + collider.gameObject.name);
                
                if (collider == other) continue;

                //Checking the "other" parameter so we don't do double-damage.
                if (collider.tag == "Enemy")
                {
                    collider.GetComponent<Health>().TakeDamage(damage);
                    if (objectToSpawnOnImpact != null)
                        Instantiate(objectToSpawnOnImpact, collider.transform.position, Quaternion.identity);
                }
            }

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }

    }
}