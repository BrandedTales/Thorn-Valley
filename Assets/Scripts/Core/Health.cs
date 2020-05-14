using UnityEngine;
using BT.Variables;
using System;

namespace BT.Core
{
    public class Health : MonoBehaviour
    {

        [SerializeField] FloatReference maxHealth;
        [SerializeField] float currentHealth;

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

        public float HealthPercentage()
        {
            return 100 * (currentHealth / maxHealth.value);
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}