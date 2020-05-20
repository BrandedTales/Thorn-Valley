using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BT.Core
{
    public class HealthDisplay : MonoBehaviour
    {

        Health playerHealth;

        [SerializeField] TextMeshProUGUI currentHealth;
        [SerializeField] TextMeshProUGUI maxHealth;


        // Start is called before the first frame update
        void Start()
        {
            playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
            if (playerHealth == null) Debug.LogError("Could not find player health while preparing the HUD.");

            playerHealth.PlayerHealthChange += UpdateHealthAmount;

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