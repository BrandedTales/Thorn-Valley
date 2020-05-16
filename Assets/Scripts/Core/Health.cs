using UnityEngine;
using BT.Variables;
using System;

namespace BT.Core
{
    public class Health : MonoBehaviour
    {

        [SerializeField] FloatReference maxHealth;
        [SerializeField] float currentHealth;

        bool isDead = false;



        private void Start() {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void RecoverHealth(float heal)
        {
            currentHealth = Mathf.Clamp(currentHealth + heal, 0, maxHealth.value);
        }

        public float HealthPercentage()
        {
            return 100 * (currentHealth / maxHealth.value);
        }

        private void Die()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            Destroy(gameObject);
        }

        public bool IsDead()
        {
            return isDead;
        }
    }
}