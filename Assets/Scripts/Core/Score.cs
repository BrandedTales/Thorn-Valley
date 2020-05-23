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
        [SerializeField] BoolVariable resetOnlyOnNew;
        public Collection[] collections;

        public FloatVariable gugCount;
        public GameLevel gameLevel;

        // Start is called before the first frame update
        void Awake()
        {
            if (resetGameData.value==true)
            {
                for (int i = 0; i < collections.Length;i++)
                {
                    collections[i].Initialize();
                }
                gugCount.SetValue(0);
            }


        }

        private void Start() {
            if (resetOnlyOnNew)
            {
                resetGameData.SetValue(false);
            }
        }

        public void AddGugs(float gugToAdd)
        {
            gugCount.ApplyChange(gugToAdd);
        }
    }


}