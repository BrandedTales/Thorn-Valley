using UnityEngine;
using BT.Variables;
using BT.Core;

namespace BT.Items
{
    public  class RecoverHealthItem : LootableBehavior
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