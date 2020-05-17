using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT.Variables;
using System;

namespace BT.Core
{
    public enum EnablerAction { TurnOn, TurnOff, Toggle}

    public class Enabler : InteractBehavior
    {
        [SerializeField] bool isPersistent;
        [SerializeField] BoolVariable isEnabled;
        [SerializeField] EnablerAction enablerAction;

        [SerializeField] GameObject objectToEnable;

        private void Start() 
        {
            if (isPersistent)
            {
                EnableObject(isEnabled.value);
            }
            else
            {
                isEnabled.SetValue(objectToEnable.activeSelf);
            }

        }


        public override void Activate()
        {

            switch (enablerAction)
            {
                case EnablerAction.TurnOn: EnableObject(true);
                    break;
                case EnablerAction.TurnOff: EnableObject(false);
                    break;
                case EnablerAction.Toggle: EnableObject(!isEnabled.value);
                    break;
            }
        }

        private void EnableObject(bool enable)
        {
            if (isEnabled.value == enable)
            {
                Debug.Log("Already in proper state.  Doing nothing.");
            }
            else
            {
                isEnabled.SetValue(enable);
                objectToEnable.SetActive(isEnabled.value);
            }
        }
    }
}