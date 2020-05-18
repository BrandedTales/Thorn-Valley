using UnityEngine;
using BT.Variables;

namespace BT.Core
{
    public  class RecoverHealth : LootableBehavior
    {

        Health playerHealth;
        public FloatReference healAmount;

        private void Start() 
        {
            playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        public override void CollectLoot()
        {
            playerHealth.RecoverHealth(healAmount);
        }
    }
}