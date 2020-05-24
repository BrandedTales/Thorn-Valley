using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT.Core;
using BT.Events;

namespace BT.Items
{
    public class GainResources : LootableBehavior
    {

        [SerializeField] float gugAmount;
        [Header("Collectibles")]
        [SerializeField] Collection collection;
        [SerializeField] CollectibleType collectibleType;
        public GameEvent somethingCollected;
        Score score;

        [Header("Debugging")]
        [SerializeField] bool bDebug = true;



        // Start is called before the first frame update
        void Start()
        {
            score = FindObjectOfType<Score>();

            if (score == null)
                Debug.LogError("Please add a score object to the scene.");
        }

        public override void CollectLoot()
        {
            
            if (gugAmount > 0)
            {
                score.AddGugs(gugAmount);
                if (bDebug) Debug.Log(score.gugCount.value);
            }
            else if (collectibleType != null)
            {
                GetComponent<Lootable>().isUniqueAndCollected.SetValue(true);
                collection.SetCollect(score.gameLevel, collectibleType, true);
                if (bDebug)
                {
                    for (int i = 0; i < collection.collectibleTypes.Length; i++)
                    {
                        Debug.Log(collection.GetCollected(score.gameLevel, collection.collectibleTypes[i]));
                    }
                }
            }

            somethingCollected.Raise();

        }




    }
}