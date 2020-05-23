using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BT.Variables;
using BT.Items;

namespace BT.Items
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Thorn Valley/Create Inventory", order = 0)]
    public class Inventory : ScriptableObject
    {
        public List<Wand> wands;

        public void PurgeWands()
        {
            if (wands == null) return;
            
            wands.Clear();
        }

        public void AddWand(Wand newWand)
        {
            wands.Add(newWand);
        }
    }
}