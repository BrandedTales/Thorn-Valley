using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BT.UI
{
    public class HUD : MonoBehaviour
    {
        //Start is called before the first frame update
        void Awake()
        {
            int objCount = FindObjectsOfType<HUD>().Length;
            Debug.Log("HUD Count: " + objCount);
            if (objCount > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}