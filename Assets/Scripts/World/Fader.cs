using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.World
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;


        private void Start() {
            canvasGroup = GetComponent<CanvasGroup>();

            // StartCoroutine(FadeOutIn());
        }


        // public IEnumerator FadeOutIn()
        // {
        //     yield return FadeOut(3.0f);
        //     yield return FadeIn(1.5f);
        // }

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha != 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }


        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

    }
}
