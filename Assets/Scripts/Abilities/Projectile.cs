using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using BT.Variables;

namespace BT.Abilities
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] float speed;
        [SerializeField] float lifeSpan;

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }


        public void InitializeProjectile(float speed, float lifeSpan, bool isPassthrough)
        {
            this.speed = speed;
            this.lifeSpan = lifeSpan;

            StartCoroutine(DestroyMe());
        }

        IEnumerator DestroyMe()
        {
            yield return new WaitForSeconds(lifeSpan);
            Destroy(gameObject);
        }

    }
}