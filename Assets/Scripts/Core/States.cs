using System;
using BT.Events;
using UnityEngine;

namespace BT.Core
{
    public enum PlayerPassive { Jump, Treasure, Light, Regen, Water, Invisible, Invulnerable }

    public class States : MonoBehaviour
    {
        public bool[] currentStates;

        [SerializeField] bool bDebug = false;

        /* Notes on States
         *
         * Invisible:  Makes enemy unable to "notice" the player.  This is checked in a few places:
         *              - "CanAttack" module in the fighter module.  Invisible players always return null.
         *              - "FleeBehavior" criteria include a check for invisibility to ensure it doesn't flee from something it can't see.
         * Jump: Gives the player a higher jump ability.  This is done by:
         *              - When a state is set, it fires off the StateEngaged event.  The PAM subscribes, and has an "Update Jump" method that checks.
         * Invulnerable: Prevents enemies from being able to damage the player.
         *              - Right now the only changes are within the AbilitySpawn module.  TriggerEnter and Explode both check for Invulnerability state.
         *              - Made a note to add proper logic once Fighter is updated for melee (which is pending the animator being activated.)
         * Light: Illuminates a small area around the player.
         *              - Added a light object attached to the player.  Event state called in PAM to Update light.
         * Regen: Heals player a set amount over a set timer.
         *              - Added a check for this state in the PAM Update() function.
         */

        [SerializeField] GameEvent stateChange;

        private void Awake() 
        {
            currentStates = new bool[Enum.GetValues(typeof(PlayerPassive)).Length];
        }

        public bool GetState(int state)
        {
            return currentStates[state];
        }

        public void SetState(int state, bool newState)
        {
            if (bDebug) Debug.Log("Setting state: " + state + " out of " + currentStates.Length);
            currentStates[state] = newState;
            stateChange.Raise();
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