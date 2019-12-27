using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Player player;
    public Text gameOverText;
    public Text powerUpText;
    public Text timerText;
    public Text miniMapText;
    public RawImage miniMap;
    public AudioManager audioManager;
    public DPad dPad;
    public PauseMenuCommands pauseMenuCommands;
    public bool loadMinimap = true;
    public bool initiateAudio;

    public StoredOptionDictEntry[] storedOptionDefaultValues;
    public Dictionary<string, float> storedOptionValues = new Dictionary<string, float>();
    [HideInInspector] public float timeToggled;
    [HideInInspector] public bool gameOver = false;
    [HideInInspector] public bool showRunTime;
    [HideInInspector] public bool runComplete = false;
    [HideInInspector] public bool gamePaused;
    [HideInInspector] public bool inMainMenu;
    

    bool canRestartLevel = false;
    bool canLoadNextLevel = false;
    //bool canRestartRun;
    
//    bool levelLost = false;
//    bool levelWon = false;
    bool toggleStats = false;
    float levelElapsedTime = 0;
    float gameElapsedTime = 0;
    double roundedGameElapsedTime;
    double roundedLevelElapsedTime;
    public GameObject gameCanvas;

    private void Awake()
    {
        if (gameCanvas == null)
        {
            gameCanvas = GameObject.FindGameObjectWithTag("Canvas");
        }
        UnpackStoredOptionDictionary();
        InitiateAudioManager();
        InitiatePauseMenu();
    }
    private void Start()
    {
        ResetGame();
        ResetMinimap();
        SetOptionsToDefault();
    }
    private void Update()
    {
        if (gameOver)
        {
            LevelChange();
        }
        if (!gamePaused && (Input.GetKeyDown(KeyCode.F1)||dPad.isDown))
        {
            toggleStats = !toggleStats;
        }
        if (!gamePaused && (Input.GetKeyDown(KeyCode.M) || dPad.isUp))
        {
            ToggleMinimap();
        }
        if ((Input.GetKeyDown("escape") || Input.GetKeyDown("joystick button 7")) && !inMainMenu)
        {
            TogglePauseMenu();
        }
        UpdateTimer();
        
    }
    void InitiateAudioManager()
    {
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("AudioManager");
        if (audioManagerObject == null)
        {
            audioManagerObject = Instantiate(Resources.Load("AudioManager")) as GameObject;
        }
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }
    void InitiatePauseMenu()
    {
        GameObject pauseMenuCanvasObject = GameObject.FindGameObjectWithTag("PauseMenuCanvas");
        if (pauseMenuCanvasObject == null)
        {
            pauseMenuCanvasObject = Instantiate(Resources.Load("Pause Menu Canvas")) as GameObject;
        }
        pauseMenuCommands = GameObject.FindGameObjectWithTag("PauseMenuCanvas").GetComponent<PauseMenuCommands>();
    }
    public void ToggleGameCanvas(bool turnOn)
    {
        if (turnOn)
        {
            if (!gameCanvas.activeInHierarchy)
            {
                gameCanvas.SetActive(true);
            }
        }
        if (!turnOn)
        {
            if (gameCanvas.activeInHierarchy)
            {
                gameCanvas.SetActive(false);
            }
        }
    }
    public bool CheckIfMainMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void TogglePauseMenu()
    {

            if (gamePaused)
            {
                pauseMenuCommands.UnPauseGame();
            return;
            }
            else if (!gamePaused)
            {
                pauseMenuCommands.PauseGame();
            return;
            }
    }
    void ResetText()
    {
        ClearText(gameOverText);
        ClearText(powerUpText);
        levelElapsedTime = 0;
    }
    void ResetGame()
    {
        inMainMenu = CheckIfMainMenu();
        ResetText();
        canRestartLevel = false;
        canLoadNextLevel = false;
        //canRestartRun = false;
        gameOver = false;
//        levelWon = false;
//        levelLost = false;
        player = GameObject.FindGameObjectWithTag("Player") != null ? GameObject.FindGameObjectWithTag("Player").GetComponent<Player>() : null;
        dPad = GetComponent<DPad>();
        if (runComplete)
        {
            ResetRun();
        }
        if (gamePaused)
        {
            pauseMenuCommands.UnPauseGame();
        }
        
    }
    public void ResetRun()
    {
        runComplete = false;
        gameElapsedTime = 0;
    }
    void ResetMinimap()
    {
        if (miniMap != null && miniMapText != null && loadMinimap)
        {
            miniMap.enabled = false;
            miniMapText.enabled = true;
        }
        if (!loadMinimap)
        {
            miniMap.enabled = false;
            miniMapText.enabled = false;
        }
    }
    void UpdateTimer()
    {
        if (!gameOver)
        {
            levelElapsedTime += Time.deltaTime;
            roundedLevelElapsedTime = System.Math.Round(levelElapsedTime, 1);

            gameElapsedTime += Time.deltaTime;
            roundedGameElapsedTime = System.Math.Round(gameElapsedTime, 1);
        }

        ShowTimer();
    }
    void ShowTimer()
    {
        if (timerText)
        {
            if (toggleStats == true)
            {
                timerText.text = showRunTime ? "Level Time Elapsed: " + roundedLevelElapsedTime + "\n" + "Run Time Elapsed: " + roundedGameElapsedTime : "Level Time Elapsed: " + roundedLevelElapsedTime;
            }
            if (toggleStats == false)
            {
                timerText.text = "Press 'F1' to toggle stats";
            }
        }
    }
    void ToggleMinimap()
    {
        if (miniMap != null && miniMapText != null && loadMinimap)
        {
            miniMap.enabled = !miniMap.enabled;
            miniMapText.enabled = !miniMapText.enabled;
        }
    }
    public void GameOver()
    {
        if (player.playerIsDead)
        {
            LevelLost();
        }
        if (player.playerReachedGoal)
        {
            LevelWon();
        }
        gameOver = true;
    }
    public void AddPowerUp(PowerUp powerUp)
    {
        string current = "";
        foreach (MoveAbility moveAbility in player.moveAbilities)
        {
            if (moveAbility.movementIsEnabled)
            {
                current += moveAbility.abilityName + "; ";
            }
        }
        powerUpText.text = current;
    }
    void LevelLost()
    {
        gameOverText.text = "Try again!" + "\n" + "'R' to Restart" + "\n";
        canRestartLevel = true;
        //canRestartRun = true;
//        levelLost = true;
    }
    void LevelWon()
    {
        gameOverText.text = "Nice!" + "\n" + "'Enter' to continue" + "\n" + "'R' to Restart";
        canRestartLevel = true;
        canLoadNextLevel = true;
        //canRestartRun = true;
        //levelWon = true;
    }
    void LevelChange()
    {
        if (canRestartLevel)
        {
            if (Input.GetKeyDown("r")|| Input.GetKeyDown("joystick button 1"))
            {
                LoadLevelByRelativeIndex(0);
            }
        }
        if (canLoadNextLevel)
        {
            if (Input.GetButtonDown("Submit"))
            {
                LoadLevelByRelativeIndex(1);
            }
        }
        //if (canRestartRun)
        //{
        //    if (Input.GetKeyDown("escape") || Input.GetKeyDown("joystick button 7"))
        //    {
        //        QuitToMainMenu();
        //    }
        //}
    }
    public void QuitToMainMenu()
    {
        runComplete = true;
        showRunTime = false;
        LoadLevelByIndex(0);
        ToggleGameCanvas(false);
    }
    void LoadLevelByRelativeIndex(int nextLevelRelativeIndex)//0 means restart level, 1 means go forward one, -1 means go back one
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + nextLevelRelativeIndex;
        LoadLevelByIndex(nextSceneIndex);

    }
    public void LoadLevelByIndex(int index)
    {
        if (!CheckLevelByIndex(index))
        {
            QuitToMainMenu();
        }
        else
        {
            Scene nextScene = SceneManager.GetSceneByBuildIndex(index);
            StartCoroutine(InitiateLoadLevel(index));
        }
    }
    bool CheckLevelByIndex(int levelIndex)
    {
        int maxScenes = SceneManager.sceneCountInBuildSettings;
        if (levelIndex+1 > maxScenes)
        {
            return false;
        }
        return true;
    }
    IEnumerator InitiateLoadLevel (int sceneIndex)
    {
        var asyncScene = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncScene.isDone)
        {
            yield return null;
        }
        ResetGame();
    }
    void ClearText(Text text)
    {
        if (text)
        {
            if (text == powerUpText)
            {
                text.text = "Powerups: ";
            }
            if (text == gameOverText)
            {
                text.text = "";
            }
        }
        
    }
    public void StoreOptionsValue(string optionKey, float newValue)
    {
        if (storedOptionValues.ContainsKey(optionKey))
        {
            storedOptionValues[optionKey] = newValue;
        }
    }
    public void StoreOptionsBool(string optionKey, bool newBool)
    {
        float newValue = newBool == true ? 1 : 0;
        StoreOptionsValue(optionKey, newValue);
    }
    public float GrabOptionsValue(string optionKey)
    {
        if (storedOptionValues.ContainsKey(optionKey))
        {
            return storedOptionValues[optionKey];
        }
        else
        {
            return 0;
            
        }
    }
    public bool GrabOptionsBool(string optionKey)
    {
        bool newBool = GrabOptionsValue(optionKey) == 1 ? true : false;
        return newBool;
    }
    void UnpackStoredOptionDictionary()
    {
        if (storedOptionDefaultValues != null)
        {
            foreach (StoredOptionDictEntry storedOptionDefaultValue in storedOptionDefaultValues)
            {
                storedOptionValues.Add(storedOptionDefaultValue.key, storedOptionDefaultValue.value);
            }
        }
        
    }
    void SetOptionsToDefault()
    {
        foreach (StoredOptionDictEntry storedOptionDefaultValue in storedOptionDefaultValues)
        {
            if (storedOptionDefaultValue.optionType == "Audio" && storedOptionDefaultValue.value != 0)
            {
                audioManager.ChangeVolume(storedOptionDefaultValue.value, storedOptionDefaultValue.key);
            }
        }
        audioManager.loopCurrentSong = GrabOptionsBool("Loop Button");
    }

}
