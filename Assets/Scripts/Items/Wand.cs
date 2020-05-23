using UnityEngine;
using UnityEngine.UI;
using BT.Abilities;
using BT.Variables;
using BT.Core;

namespace BT.Items
{
    [CreateAssetMenu(fileName = "New Wand", menuName = "Thorn Valley/Create Wand", order = 0)]
    public class Wand : ScriptableObject
    {

        [Header("Visuals")]
        public string wandName;
        public GameObject wandPrefab;
        public Sprite wandIcon;

        [Header("Abilities")]
        public Ability attackAbility;
        public Ability defenseAbility;
        public Ability utilityAbility;
        public Ability passiveAbility;

    }
}