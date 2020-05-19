using UnityEngine;
using BT.Variables;

namespace BT.Core
{
    public  class RecoverHealth : LootableBehavior
    {

        Health playerHealth;
        public FloatReference healAmount;
        public bool isMaxHealth = false;
        public FloatReference refillPercent;

        private void Start() 
        {
            playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        public override void CollectLoot()
        {
            if (isMaxHealth)
                playerHealth.IncreaseMaxHealth(healAmount, refillPercent.value);
            else
                playerHealth.RecoverHealth(healAmount);
        }
    }
}