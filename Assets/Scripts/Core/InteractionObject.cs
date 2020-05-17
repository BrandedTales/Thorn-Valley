using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT.Abilities;

namespace BT.Core
{
    public class InteractionObject : MonoBehaviour
    {

        [SerializeField] ElementType elementType;

        PlayerAbilityManager abilityManager;
        InteractBehavior interactBehavior;

        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        

        // Start is called before the first frame update
        void Start()
        {
            abilityManager = GameObject.FindWithTag("Player").GetComponent<PlayerAbilityManager>();
            interactBehavior = GetComponent<InteractBehavior>();

            abilityManager.Interaction += ActivateObject;
        }

        public void ActivateObject()
        {
            if (PlayerIsClose())
            {
                if (elementType == ElementType.None)
                {
                    if (bDebug) Debug.Log("Interacting!  No key required.");
                    interactBehavior.Activate();
                }
                else if (elementType == abilityManager.utilityAbility.elementType)
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

        private bool PlayerIsClose()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
            var player = GameObject.FindWithTag("Player");

            foreach (Collider collider in hitColliders)
            {
                if (collider.gameObject == player)
                    return true;
            }
            if (bDebug) Debug.Log("Player not in area of " + gameObject.name);
            return false;

        }

    }
}