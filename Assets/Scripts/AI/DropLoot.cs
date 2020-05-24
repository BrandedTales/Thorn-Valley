using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT.Items;

namespace BT.AI
{
    public class DropLoot : MonoBehaviour
    {

        [SerializeField] DropContents dropContents;
        [SerializeField] float spawnLootHeightOffset = 1.0f;

        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        public void Drop(Transform spawnLocation)
        {
            for (int i = 0; i < dropContents.dropDetails.Length;i++)
            {
                int r = UnityEngine.Random.Range(1, 100);
                if (r < dropContents.dropDetails[i].dropRate)
                {
                    SpawnLoot(dropContents.dropDetails[i].item, spawnLocation);
                    if (dropContents.isMaxDropOne) return;
                }
            }
        }

        private void SpawnLoot(Lootable item, Transform spawnLocation)
        {
            Instantiate(item, spawnLocation.position + new Vector3(0,spawnLootHeightOffset, 0), spawnLocation.rotation);
            if (bDebug) Debug.Log("Spawned loot at " + (spawnLocation.position + new Vector3(0, spawnLootHeightOffset, 0)));
        }
    }
}