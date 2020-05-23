using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BT.Events;
using BT.Items;

namespace BT.UI
{
    public class InventoryDisplay : MonoBehaviour
    {
        public HorizontalLayoutGroup wandPanel;
        public GameObject inventoryPanel;
        public WandButton wandButton;

        public Inventory inventory;

        public GameEvent pauseGame;
        public GameEvent unpauseGame;


        bool isOpen = false;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                isOpen = !isOpen;
                ToggleMenu();
            }

        }

        private void ToggleMenu()
        {
            Time.timeScale = (isOpen ? 0 : 1);
            inventoryPanel.SetActive(isOpen);
            if (isOpen)
                pauseGame.Raise();
            else
                unpauseGame.Raise();
        }
        public void RefreshWandPanel()
        {
            foreach (Transform child in wandPanel.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Wand wand in inventory.wands)
            {
                WandButton button = Instantiate(wandButton);
                button.name = wand.name;
                button.transform.parent = wandPanel.transform;
                button.GetComponent<Image>().sprite = wand.wandIcon;
                button.wand = wand;

            }
        }

    }
}