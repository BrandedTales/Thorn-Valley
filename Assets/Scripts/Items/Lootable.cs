using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT.Variables;
using System;

namespace BT.Items
{
    public class Lootable : MonoBehaviour
    {

        LootableBehavior lootableBehavior;

        [Header("General Details")]
        [SerializeField] string objectName;
        [SerializeField] GameObject lootableObject;

        [Header("Item Bob")]
        [SerializeField] bool isBobbing = false;
        [SerializeField] float bobRange = 0.2f;
        float bobDirection = 0.002f;
        float bobCurrent = 0;

        [Header("Item Spin")]
        [Tooltip("Currently does nothing!")]
        [SerializeField] bool isSpinning = false;
        [SerializeField] FloatReference rotationFactor;

        [Header("Destroy Options")]
        [SerializeField] FloatReference destroyTimer;

        [Header("Debugging")]
        [SerializeField] bool bDebug = true;

        // Start is called before the first frame update
        void Start()
        {
            lootableBehavior = GetComponent<LootableBehavior>();

            if (destroyTimer.value > 0)
                StartCoroutine(DestroyMe());
        }

        // Update is called once per frame
        void Update()
        {
            if (isBobbing) moveBob();
            if (isSpinning) spinObject();
        }

        private void moveBob()
        {

            bobCurrent += bobDirection;
            Vector3 bobVector = new Vector3(0, bobDirection, 0);

            if ((bobCurrent <= 0) || (bobCurrent >= bobRange))
            {
                bobDirection *= -1;
            }
            lootableObject.transform.Translate(bobVector);

        }


        private void spinObject()
        {
            lootableObject.transform.Rotate(0, rotationFactor.value * Time.deltaTime, 0, Space.Self);
        }

        IEnumerator DestroyMe()
        {
            yield return new WaitForSeconds(destroyTimer.value);

            //Add some flicker effects, maybe chain together coroutines to make this work.
 
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.tag=="Player")
            {
                CollectItem();
            }
        }

        private void CollectItem()
        {
            if (bDebug) Debug.Log("Item has been collected.");
            lootableBehavior.CollectLoot();
            Destroy(gameObject);
        }

    }
}