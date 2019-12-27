using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class VolumeSlider : MonoBehaviour
{
    public string mixerGroup;
    public Slider slider;
    public TextMeshProUGUI volumeValueText;

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
        InitializeSlider();
    }
    public void SetVolume(float volume)
    {
        if (isActiveAndEnabled)
        {
            audioManager.ChangeVolume(volume, mixerGroup);
            volumeValueText.text = volume.ToString();
        }
    }
    void InitializeSlider()
    {
        if (!isInitialized || gameController.initiateAudio)
        {
            isInitialized = true;
            float currentVolume = gameController.GrabOptionsValue(mixerGroup);
            slider.value = currentVolume;
            volumeValueText.text = currentVolume.ToString();
        }
    }
}
