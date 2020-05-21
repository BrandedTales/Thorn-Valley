using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT.Core;

namespace BT.Items
{
    public class GainResources : LootableBehavior
    {

        [SerializeField] float gugAmount;
        [Header("Collectibles")]
        [SerializeField] Collection collection;
        [SerializeField] CollectibleType collectibleType;
        [SerializeField] GameLevels gameLevel;

        Score score;

        [SerializeField] bool bDebug = true;

        public override void CollectLoot()
        {
            
            if (gugAmount > 0)
            {
                score.AddGugs(gugAmount);
                if (bDebug) Debug.Log(score.gugCount.value);
            }
            else if (collectibleType != null)
            {
                collection.SetCollect(gameLevel, collectibleType, true);
                if (bDebug)
                {
                    for (int i = 0; i < collection.collectibleTypes.Length; i++)
                    {
                        Debug.Log(collection.GetCollected(gameLevel, collection.collectibleTypes[i]));
                    }
                }
            }

        }



        // Start is called before the first frame update
        void Start()
        {
            score = FindObjectOfType<Score>();

            if (score == null)
                Debug.LogError("Please add a score object to the scene.");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}