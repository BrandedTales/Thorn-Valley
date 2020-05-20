using UnityEngine;
using BT.Variables;
using System;

namespace BT.Core
{
    public class Health : MonoBehaviour
    {

        [Header("Player Settings")]
        [SerializeField] FloatReference playerMaxHealth;
        [SerializeField] FloatReference playerCurrentHealth;

        [Header("Enemy Settings")]
        [SerializeField] float enemyCurrentHealth;
        [SerializeField] float enemyMaxHealth;

        [Header("Initialization Settings")]
        [SerializeField] BoolVariable resetGameData;
        [SerializeField] float maxHealthInitialization;

        bool isPlayer = false;
        bool isDead = false;

        public event Action PlayerHealthChange;

        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        private void Start()
        {
            if (gameObject.tag == "Player") isPlayer = true;
            
            if (resetGameData.value && isPlayer)
            {
                if (bDebug) Debug.Log("Initializing player data for " + gameObject.name);
                if (bDebug) Debug.Log("Pre-initialize max: " + playerMaxHealth.value + "; current: " + playerCurrentHealth.value);
                playerMaxHealth.variable.SetValue(maxHealthInitialization);
                playerCurrentHealth.variable.SetValue(playerMaxHealth.value);

                PlayerHealthChange.Invoke();
            }

        }

        public float GetMaxHealth()
        {
            return isPlayer ? playerMaxHealth.value : enemyMaxHealth;
        }

        public float GetCurrentHealth()
        {

            return isPlayer ? playerCurrentHealth.value : enemyCurrentHealth;
        }

        public void TakeDamage(float damage)
        {
            if (isPlayer)
            {
                playerCurrentHealth.variable.ApplyChange(-1 * damage);
                PlayerHealthChange.Invoke();
            }
            else 
            {
                enemyCurrentHealth -= damage;
            }

            if (GetCurrentHealth() <= 0)
            {
                Die();
            }
        }

        public void RecoverHealth(float heal)
        {
            if (isPlayer)
            {
                playerCurrentHealth.variable.ApplyChange(heal);
                playerCurrentHealth.variable.SetValue(Mathf.Clamp(playerCurrentHealth.value, 0, playerMaxHealth.value));
                PlayerHealthChange.Invoke();
            }
            else
            {
                enemyCurrentHealth = Mathf.Clamp(enemyCurrentHealth + heal, 0, enemyMaxHealth);
            }
        }

        public float GetHealthPercentage()
        {
            return 100 * (GetCurrentHealth() / GetMaxHealth());
        }

        private void Die()
        {
            
            if (isPlayer)
            {
                Debug.Log("And... we... die...");
            }
            else
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
                GetComponent<DropLoot>().Drop(transform);
                Destroy(gameObject);
            }

        }

        public bool IsDead()
        {
            return isDead;
        }

        public void IncreaseMaxHealth(float increaseAmount, float refillPercent)
        {
            playerMaxHealth.variable.SetValue(increaseAmount);
            if (playerMaxHealth.value * refillPercent > playerCurrentHealth.value)
            {
                playerCurrentHealth.variable.SetValue(playerMaxHealth.value * refillPercent);
            }

            PlayerHealthChange.Invoke();

        }
    }
}