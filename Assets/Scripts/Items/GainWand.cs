using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT.Core;
using BT.Events;

namespace BT.Items
{
    public class GainWand : LootableBehavior
    {

        public PlayerRunTimeData prtd;
        public Wand wand;
        public GameEvent somethingCollected;
        public Inventory inventory;

        [Header("Debugging")]
        [SerializeField] bool bDebug = true;



        // Start is called before the first frame update
        void Awake()
        {
            GameObject newWand = Instantiate(wand.wandPrefab, transform.position, transform.rotation);
            newWand.transform.parent = transform;
            GetComponent<Lootable>().lootableObject = newWand;
        }



        public override void CollectLoot()
        {
            inventory.AddWand(wand);
            somethingCollected.Raise();

            if (inventory.wands.Count == 1) prtd.EquipWand(wand);
        }





    }
}