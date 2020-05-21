using UnityEngine;
using BT.Variables;
using System;
using BT.Events;

namespace BT.Core
{
    public class Health : MonoBehaviour
    {

        [Header("Player Settings")]
        [SerializeField] FloatReference playerMaxHealth;
        [SerializeField] FloatReference playerCurrentHealth;
        public GameEvent playerHealthChange;

        [Header("Enemy Settings")]
        [SerializeField] float enemyCurrentHealth;
        [SerializeField] float enemyMaxHealth;

        [Header("Initialization Settings")]
        [SerializeField] BoolVariable resetGameData;
        [SerializeField] float maxHealthInitialization;

        [Header("Regen Settings")]
        public FloatReference regenTickAmount;
        public FloatReference regenHealthAmount;

        bool isPlayer = false;
        bool isDead = false;

        ICharacter character;



        States myStates;

        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        private void Start()
        {
            character = GetComponent<ICharacter>();

            if (gameObject.tag == "Player")
            {
                isPlayer = true;
                myStates = GetComponent<States>();
            }

            if (resetGameData.value && isPlayer)
            {
                if (bDebug) Debug.Log("Initializing player data for " + gameObject.name);
                if (bDebug) Debug.Log("Pre-initialize max: " + playerMaxHealth.value + "; current: " + playerCurrentHealth.value);
                playerMaxHealth.variable.SetValue(maxHealthInitialization);
                playerCurrentHealth.variable.SetValue(playerMaxHealth.value);

                playerHealthChange.Raise();
            }


        }

        public float RegenHealth(float timer)
        {
            if (timer >= regenTickAmount.value)
            {
                RecoverHealth(regenHealthAmount.value);
                return 0;
            }
            else
            {
                return timer;
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
                playerHealthChange.Raise();
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
                playerHealthChange.Raise();
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
            character.Die();
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

            playerHealthChange.Raise();

        }
    }
}