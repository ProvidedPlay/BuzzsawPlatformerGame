using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuCommands : MonoBehaviour
{
    GameController gameController;
    AudioManager audioManager;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void StartNewRun()
    {
        gameController.showRunTime = true;
        gameController.ToggleGameCanvas(true);
        gameController.runComplete = true;
        gameController.LoadLevelByIndex(1);
    }
    public void LoadLevel(int buildIndex)
    {
        if (buildIndex == 1)
        {
            StartNewRun();
        }
        else
        {
            gameController.showRunTime = false;
            gameController.ToggleGameCanvas(true);
            gameController.LoadLevelByIndex(buildIndex);
        }
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
    public void QuitGame()
    {
        Application.Quit();
    }
}
