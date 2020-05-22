using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT.World;

namespace BT.Abilities
{
    public class InteractionObject : MonoBehaviour
    {

        [SerializeField] ElementType elementType;
        
        InteractBehavior interactBehavior;


        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        

        // Start is called before the first frame update
        void Start()
        {
            interactBehavior = GetComponent<InteractBehavior>();
        }

        public void ActivateObject(ElementType playerElementType)
        {

            if (elementType == ElementType.None)
            {
                if (bDebug) Debug.Log("Interacting!  No key required.");
                interactBehavior.Activate();
            }
            else if (elementType == playerElementType)
            {
                if (bDebug) Debug.Log("Activating InteractionObject with proper key: " + elementType);
                interactBehavior.Activate();
            }
            else
            {
                if (bDebug) Debug.Log("Trying to activate interaction object, but key is missing.");
            }
            

        }

    }
}