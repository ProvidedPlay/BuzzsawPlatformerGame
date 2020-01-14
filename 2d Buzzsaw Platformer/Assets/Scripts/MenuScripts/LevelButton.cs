using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int levelNumber;
    public bool levelEnabled;

    bool isInitialized;
    GameController gameController;
    TextMeshProUGUI buttonText;
    Button button;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
    }

    void OnEnable()
    {
        if (levelNumber != 0)
        {
            levelEnabled = gameController.unlockedLevels.Contains(levelNumber);
        }
        buttonText.enabled = levelEnabled;
        button.interactable = levelEnabled;
        
    }

}
