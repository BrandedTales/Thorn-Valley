using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT.Core;
using BT.Items;

namespace BT.UI
{
    public class WandButton : MonoBehaviour
    {

        public Wand wand;
        public PlayerRunTimeData prtd;

        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        public void EquipWand()
        {
            if (bDebug) Debug.Log("Equip Button pressed - " + wand.name + " is being equipped.");
            prtd.EquipWand(wand);
        }

    }
}