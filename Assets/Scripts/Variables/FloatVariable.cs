using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BT.Variables
{
    [CreateAssetMenu(menuName = "Variable/Float")]
    public class FloatVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string Description = "";
#endif
        public float value;

        public void SetValue(float value)
        {
            this.value = value;
        }

        public void ApplyChange(float amount)
        {
            value += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            value += amount.value;
        }
    }
}