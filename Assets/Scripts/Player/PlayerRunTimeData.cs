using UnityEngine;
using BT.Core;
using BT.Abilities;

namespace BT.Player
{
    [CreateAssetMenu(fileName = "New PlayerRunTimeData", menuName = "Thorn Valley/Create PlayerRunTimeData", order = 0)]
    public class PlayerRunTimeData : ScriptableObject
    {
        [Header("Abilities")]
        public Ability attackAbility;
        public Ability defenseAbility;
        public Ability utilityAbility;
        public Ability passiveAbility;

    }
}