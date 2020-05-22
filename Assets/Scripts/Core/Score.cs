using System.Collections;
using System.Collections.Generic;
using BT.Variables;
using UnityEngine;
using BT.Events;

namespace BT.Core
{
    public enum GameLevel { Overworld, Dungeon1 }

    public class Score : MonoBehaviour
    {

        [SerializeField] BoolVariable resetGameData;
        public Collection[] collections;

        public FloatVariable gugCount;
        public GameLevel gameLevel;

        // Start is called before the first frame update
        void Start()
        {
            if (resetGameData)
            {
                for (int i = 0; i < collections.Length;i++)
                {
                    collections[i].Initialize();
                }
                gugCount.SetValue(0);
            }
        }

        public void AddGugs(float gugToAdd)
        {
            gugCount.ApplyChange(gugToAdd);
        }
    }


}