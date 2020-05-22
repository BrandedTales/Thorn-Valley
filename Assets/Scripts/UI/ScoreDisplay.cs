using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT.Events;
using TMPro;
using BT.Core;
using UnityEngine.UI;

namespace BT.UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        Score score;

        public TextMeshProUGUI gugCountDisplay;
        public HorizontalLayoutGroup collectionPanel;
        public GameObject collectionImage;


        // Start is called before the first frame update
        void Start()
        {
            score = FindObjectOfType<Score>();
            UpdateScore();
        }

        public void UpdateScore()
        {
            gugCountDisplay.text = score.gugCount.value.ToString();

            foreach (Transform child in collectionPanel.transform)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < score.collections[0].collectibleTypes.Length; i++)
            {
                var image = Instantiate(collectionImage);
                image.transform.parent = collectionPanel.transform;
                if (score.collections[0].GetCollected(score.gameLevel, score.collections[0].collectibleTypes[i]))
                {
                    image.GetComponent<Image>().sprite = score.collections[0].collectibleTypes[i].collectedImage;
                }
                else
                {
                    image.GetComponent<Image>().sprite = score.collections[0].collectibleTypes[i].uncollectedImage;
                }
            }
        }
    }
}