using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongDisplay : MonoBehaviour
{
    public TMPro.TextMeshProUGUI currentSongText;
    GameController gameController;
    AudioManager audioManager;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }
    private void OnEnable()
    {
        if (audioManager.currentSong != null)
        {
            UpdateCurrentSongText();
        }
    }
    public void UpdateCurrentSongText()
    {
        currentSongText.text = audioManager.currentSong.name;
    }
}
