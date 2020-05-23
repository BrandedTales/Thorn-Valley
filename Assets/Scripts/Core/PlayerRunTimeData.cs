using UnityEngine;
using BT.Core;
using BT.Abilities;
using BT.Items;


namespace BT.Core
{
    [CreateAssetMenu(fileName = "New PlayerRunTimeData", menuName = "Thorn Valley/Create PlayerRunTimeData", order = 0)]
    public class PlayerRunTimeData : ScriptableObject
    {

        [Header("Wand")]
        public Wand activeWand;

        [Header("Abilities")]
        public Ability attackAbility;
        public Ability defenseAbility;
        public Ability utilityAbility;
        public Ability passiveAbility;

        public void EquipWand(Wand wand)
        {
            activeWand = wand;

            attackAbility = activeWand.attackAbility;
            defenseAbility = activeWand.defenseAbility;
            utilityAbility = activeWand.utilityAbility;
            passiveAbility = activeWand.passiveAbility;

            passiveAbility.EngageState(true);
        }

    }
}