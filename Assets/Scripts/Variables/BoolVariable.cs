using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BT.Variables
{
    [CreateAssetMenu(menuName = "Variable/Bool")]
    public class BoolVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string Description = "";
#endif
        public bool value;

        public void SetValue(bool value)
        {
            this.value = value;
        }

        public void TurnOff()
        {
            value = false;
        }

        public void TurnOn()
        {
            value = true;
        }

        public void ToggleValue()
        {
            value = !value;
        }
    }
}