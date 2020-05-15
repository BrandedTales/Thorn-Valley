using System;
using UnityEngine;

namespace BT.Core
{
    public enum PlayerPassive { Jump, Treasure, Light, Regen, Water, Invisible }

    public class States : MonoBehaviour
    {
        public bool[] currentStates;

        private void Start() 
        {
            currentStates = new bool[Enum.GetValues(typeof(PlayerPassive)).Length];
        }

        public bool GetState(int state)
        {
            return currentStates[state];
        }

        public void SetState(int state, bool newState)
        {
            currentStates[state] = newState;
        }

        public void ClearStates()
        {
            for (int i = 0; i <currentStates.Length;i++)
            {
                currentStates[i] = false;
            }
        }
    }
}