using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT.Items;
using BT.World;
using BT.Variables;

namespace BT.Core
{
    public class Initializer : MonoBehaviour
    {

        [SerializeField] BoolVariable resetGameData;

        public Collection[] collections;
        public Inventory inventory;
        public FloatReference gugAmount;
        public FloatReference playerCurrentHealth;
        public FloatReference playerMaxHealth;
        public PlayerRunTimeData prtd;
        public BoolVariable[] uniqueItems;

        // Start is called before the first frame update
        void Awake()
        {
            if (!resetGameData.value) return;

            inventory.PurgeWands();
            foreach (Collection collection in collections)
            {
                collection.Initialize();
            }

            gugAmount.variable.SetValue(0);
            playerMaxHealth.variable.SetValue(5);
            playerCurrentHealth.variable.SetValue(5);
            prtd.Purge();

            for (int i = 0; i < uniqueItems.Length;i++)
            {
                uniqueItems[i].SetValue(false);
            }

            resetGameData.SetValue(false);
        }


    }
}