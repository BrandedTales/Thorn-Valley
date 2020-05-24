using System.Collections;
using System.Collections.Generic;
using BT.Variables;
using UnityEngine;
using BT.Events;

namespace BT.Core
{
    public enum GameLevel { House, Overworld, Dungeon1 }

    public class Score : MonoBehaviour
    {

        public Collection[] collections;

        public FloatVariable gugCount;
        public GameLevel gameLevel;

        // Start is called before the first frame update

        public void AddGugs(float gugToAdd)
        {
            gugCount.ApplyChange(gugToAdd);
        }
    }


}