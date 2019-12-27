using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionButton : MonoBehaviour
{
    public string buttonName;
    public GameObject buttonTextActive;
    public GameObject buttonTextInactive;
    [HideInInspector] public bool buttonActive; 

    GameController gameController;
    AudioManager audioManager;
    
    bool isInitialized;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void OnEnable()
    {
        InitializeButton();
    }

    public void InitializeButton()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            buttonActive = gameController.GrabOptionsBool(buttonName);
            UpdateButtonImage();
        }
    }
    public void ToggleButton()
    {
        buttonActive = !buttonActive;
        gameController.StoreOptionsBool(buttonName, buttonActive);
        UpdateButtonImage();
    }
    public void UpdateButtonImage()
    {
        buttonTextActive.SetActive(buttonActive);
        buttonTextInactive.SetActive(!buttonActive);
    }
}
