using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BT.Variables
{
    [CreateAssetMenu(menuName = "Variable/String")]
    public class StringVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string Description = "";
#endif
        public string value;

        public void SetValue(string value)
        {
            this.value = value;
        }

        public void SetValue(StringVariable newValue)
        {
            value = newValue.value;
        }
    }
}