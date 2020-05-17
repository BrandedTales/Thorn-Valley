using UnityEngine;
using BT.Variables;
using System;

namespace BT.Core
{
    public enum PortalKey { A, B, C, D, E, F}

    public class Teleporter : InteractBehavior 
    {

        public PortalKey portalKey;
        public int sceneIndex = -1;
        public Transform spawnPoint;
        public GameObject objectToTeleport;

        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        public override void Activate()
        {
            if (sceneIndex < 0)
            {
                Teleport();
            }
            else
            {
                ChangeScene();
                Teleport();
            }
        }

        private void Teleport()
        {
            GameObject[] portals = GameObject.FindGameObjectsWithTag("Interactable");

            foreach (GameObject portal in portals)
            {
                if (portal.GetComponent<Teleporter>() == null) continue;

                if (((this.portalKey) == portal.GetComponent<Teleporter>().portalKey)&&(gameObject != portal))
                {
                    Debug.Log("Teleporting from " + gameObject.name + " to " + portal.name);
                    Vector3 current = objectToTeleport.transform.position;
                    Vector3 future = portal.GetComponent<Teleporter>().spawnPoint.transform.position;

                    Debug.Log("Current Location: " + current + " and Future location:" + future);
                    objectToTeleport.transform.position = future;
                    return;
                }
            }
            Debug.Log("Tried to teleport and couldn't find a matching portal.");
        }

        private void ChangeScene()
        {
            
        }
    }
}