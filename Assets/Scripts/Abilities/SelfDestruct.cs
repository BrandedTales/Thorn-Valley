using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT.Variables;

namespace BT.Abilities
{
    public class SelfDestruct : MonoBehaviour
    {

        [SerializeField] FloatReference timer;

        // Start is called before the first frame update
        void Start()
        {

            Destroy(gameObject, timer.value);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}