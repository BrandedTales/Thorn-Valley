using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuDisplay : MonoBehaviour
{

    [SerializeField] GameObject menuPanel;
    [SerializeField] bool bDebug = true;


    bool isOpen = false;

    public event Action<bool> MenuLaunch;

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isOpen = !isOpen;
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        MenuLaunch.Invoke(isOpen);
        Time.timeScale = (isOpen ? 0 : 1);
        menuPanel.SetActive(isOpen);
    }

    public void ResumeButton()
    {
        if (bDebug) Debug.Log("Resume menu button pushed.");
        isOpen = false;
        ToggleMenu();
    }

    public void SaveButton()
    {
        if (bDebug) Debug.Log("Save menu button pushed.");
    }

    public void LoadButton()
    {
        if (bDebug) Debug.Log("Load menu button pushed.");
    }

    public void OptionsButton()
    {
        if (bDebug) Debug.Log("Options menu button pushed.");
    }

    public void QuitButton()
    {
        if (bDebug) Debug.Log("Quit menu button pushed.");
        Application.Quit();
    }
}
