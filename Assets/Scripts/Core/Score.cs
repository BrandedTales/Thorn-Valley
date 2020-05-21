using System.Collections;
using System.Collections.Generic;
using BT.Variables;
using UnityEngine;

namespace BT.Core
{
    public enum GameLevels { Overworld, Dungeon1 }

    public class Score : MonoBehaviour
    {

        [SerializeField] BoolVariable resetGameData;
        [SerializeField] Collection[] collections;

        public FloatVariable gugCount;

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