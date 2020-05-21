using UnityEngine;
using BT.Variables;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

namespace BT.World
{
    public enum PortalKey { A, B, C, D, E, F}

    public class Teleporter : InteractBehavior 
    {

        public PortalKey portalKey;
        public int sceneIndex = -1;
        public Transform spawnPoint;
        public GameObject objectToTeleport;

        [Header("Fade Settings")]
        [SerializeField] FloatReference fadeOutTime;
        [SerializeField] FloatReference fadeWaitTime;
        [SerializeField] FloatReference fadeInTime;

        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        public override void Activate()
        {
            if (sceneIndex < 0)
            {
                Teleport(false);
            }
            else
            {
                StartCoroutine(TeleportNewScene());

            }
        }

        private void Teleport(bool sceneChange)
        {
            if (sceneChange) objectToTeleport = GameObject.FindGameObjectWithTag("Player");

            foreach (Teleporter portal in FindObjectsOfType<Teleporter>())
            {
                if ((portal == null) || (portal == this)) continue;

                if ((this.portalKey) == portal.portalKey)
                {
                    Debug.Log("Teleporting from " + gameObject.name + " to " + portal.gameObject.name);
                    Vector3 current = objectToTeleport.transform.position;
                    Vector3 future = portal.spawnPoint.transform.position;

                    Debug.Log("Current Location: " + current + " and Future location:" + future);
                    objectToTeleport.transform.position = future;
                    return;
                }
            }
            Debug.Log("Tried to teleport and couldn't find a matching portal.");
        }

        private IEnumerator TeleportNewScene()
        {
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(sceneIndex);

            Teleport(true);

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            if (bDebug) Debug.Log("Faded into new Scene.  Getting ready to destroy.");
            Destroy(gameObject);
        }
    }
}