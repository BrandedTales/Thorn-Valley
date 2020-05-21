using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace BT.Core
{
    [CreateAssetMenu(fileName = "New Collection", menuName = "Thorn Valley/New Collection", order = 0)]
    public class Collection : ScriptableObject
    {
        public CollectibleType[] collectibleTypes;

        public Dictionary<CollectibleType, bool> itemsInLevel = new Dictionary<CollectibleType, bool>();

        public Dictionary<GameLevels, Dictionary<CollectibleType, bool>> collection = new Dictionary<GameLevels, Dictionary<CollectibleType, bool>>();

        public void Initialize()
        {
            collection.Clear();
            //Cycle through all game levels that have been added.
            foreach (GameLevels gameLevel in Enum.GetValues(typeof(GameLevels)))
            {

                Dictionary<CollectibleType, bool> iterationLevel = new Dictionary<CollectibleType, bool>();

                for (int i = 0; i < collectibleTypes.Length;i++)
                {
                    iterationLevel.Add(collectibleTypes[i], false);
                }
                collection.Add(gameLevel, iterationLevel);
            }
        }

        public bool GetCollected(GameLevels gameLevel, CollectibleType collectibleType)
        {
            Dictionary<CollectibleType, bool> currentLevel;
            
            if(collection.TryGetValue(gameLevel, out currentLevel))
            {
                bool retVal;
                if (currentLevel.TryGetValue(collectibleType, out retVal))
                {
                    return retVal;
                }
                else
                {
                    Debug.LogError("Score Collection: Found level, but could not find specific collectible type " + collectibleType.name);
                    return false;
                }
            }
            else
            {
                Debug.LogError("Could not find score collection for level: " + gameLevel);
                return false; 
            }
        }

        public void SetCollect(GameLevels gameLevel, CollectibleType collectibleType, bool newValue)
        {
            collection[gameLevel][collectibleType] = newValue;
        }

    }
}