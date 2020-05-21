using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BT.Core;

namespace BT.UI
{
    public class HealthDisplay : MonoBehaviour
    {

        Health playerHealth;

        [SerializeField] TextMeshProUGUI currentHealth;
        [SerializeField] TextMeshProUGUI maxHealth;


        // Start is called before the first frame update
        void Awake()
        {
            playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
            if (playerHealth == null) Debug.LogError("Could not find player health while preparing the HUD.");

            currentHealth.SetText(playerHealth.GetCurrentHealth().ToString());
            maxHealth.SetText(playerHealth.GetMaxHealth().ToString());

        }

        private void UpdateHealthAmount()
        {
            currentHealth.SetText(playerHealth.GetCurrentHealth().ToString());
            maxHealth.SetText(playerHealth.GetMaxHealth().ToString());
        }


    }
}