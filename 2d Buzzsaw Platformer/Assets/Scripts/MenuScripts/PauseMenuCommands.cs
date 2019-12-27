using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuCommands : MonoBehaviour
{
    public GameObject pauseMenu;

    GameController gameController;
    AudioManager audioManager;


    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void PauseGame()
    {
        PlaySound("Open Menu");
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gameController.gamePaused = true;
    }
    public void UnPauseGame()
    {
        PlaySound("Close Menu");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameController.gamePaused = false;
    }
    public void QuitToMainMenu()
    {
        gameController.QuitToMainMenu();
    }
    public void PlaySongByRelativeIndex(int indexRelation)
    {
        audioManager.PlaySongByRelativeIndex(indexRelation);
    }
    public void ToggleMusicPaused(bool pausePlayer)
    {
        if (pausePlayer)
        {
            audioManager.PauseMusicPlayer();
        }
        if (!pausePlayer)
        {
            audioManager.ResumeMusicPlayer();
        }
    }
    public void PlaySound(string sound)
    {
        if (gameController != null)
        {
            audioManager.PlaySoundEffect(sound);
        }
    }
    public void ToggleLoopMusic(OptionButton loopButton)
    {
        loopButton.ToggleButton();
        if (audioManager != null)
        {
            audioManager.loopCurrentSong = loopButton.buttonActive;
        }
    }
}
