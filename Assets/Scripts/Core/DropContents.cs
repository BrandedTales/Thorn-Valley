using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT.Variables;

namespace BT.Core
{
    [System.Serializable]
    public struct DropDetails
    {
        [Tooltip("Item to be looted.")]
        public Lootable item;
        [Tooltip("Likelihood of this dropping.  Calculated independent of other items in loot tables.")]
        public float dropRate;
    }

    [CreateAssetMenu(fileName = "New Loot Table", menuName = "Thorn Valley/Create Loot Table")]
    public class DropContents : ScriptableObject
    {
        [Tooltip("If checked, once an item is dropped, stopping cycling.  Order matters!")]
        public bool isMaxDropOne = true;
        public DropDetails[] dropDetails;

    }
}