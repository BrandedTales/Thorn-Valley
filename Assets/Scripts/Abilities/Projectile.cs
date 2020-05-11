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
        bool isPassthrough =false;

        [SerializeField] bool bDebug = true;

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }


        public void InitializeProjectile(float speed, float lifeSpan, float damage, bool isPassthrough)
        {
            this.speed = speed;
            this.lifeSpan = lifeSpan;
            this.damage = damage;
            this.isPassthrough = isPassthrough;

            StartCoroutine(DestroyMe());
        }

        IEnumerator DestroyMe()
        {
            yield return new WaitForSeconds(lifeSpan);
            Destroy(gameObject);
        }

        public void Impact()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (other.tag == "Player") return;

            if (other.tag == "Enemy")
            {
                if (bDebug) Debug.Log("Hit an enemy!"); 
                other.GetComponent<Health>().TakeDamage(damage);
                if (!isPassthrough)
                {
                    if (bDebug) Debug.Log("not a passthrough attack");
                    Impact();
                }
            }
            else 
            {
                if (bDebug) Debug.Log("Not an enemy:" + other.gameObject.name);
                Impact();
            }
        }

    }
}