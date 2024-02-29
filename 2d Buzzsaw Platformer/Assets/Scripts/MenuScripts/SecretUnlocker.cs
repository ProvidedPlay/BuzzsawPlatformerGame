using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretUnlocker : MonoBehaviour
{
    public int levelNumberToActivateButtons;
    public int secretLevelIndex;
    public string secretUnlockedSound;
    
    public GameObject secretButtons;
    public List<int> correctChimeSequence;

    [HideInInspector] public bool unlockerEnabled;
    [HideInInspector] List<int> currentChimeSequence = new List<int>();
    GameController gameController;
    AudioManager audioManager;
    

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    void OnEnable()
    {
        if (levelNumberToActivateButtons != 0)
        {
            unlockerEnabled = gameController.unlockedLevels.Contains(levelNumberToActivateButtons);
        }
        if (SecretUnlocked())
        {
            unlockerEnabled = false;
        }

        secretButtons.SetActive(unlockerEnabled);
        ResetChimeSequence();
    }
    public void inputChime(int chimeNumber)
    {
        currentChimeSequence.Add(chimeNumber);
        if (currentChimeSequence.Count <= correctChimeSequence.Count)
        {
            bool chimeCorrect = true;
            for (int i = 0; i < currentChimeSequence.Count; i++)
            {
                if (currentChimeSequence[i] != correctChimeSequence[i])
                {
                    chimeCorrect = false;
                }
            }
            if (chimeCorrect && currentChimeSequence.Count == correctChimeSequence.Count)
            {
                UnlockSecret();
                return;
            }
            if (!chimeCorrect)
            {
                ResetChimeSequence();
            }
        }
        if (currentChimeSequence.Count > correctChimeSequence.Count)
        {
            ResetChimeSequence();
        }

    }
    public void ResetChimeSequence()
    {
        currentChimeSequence.Clear();
    }
    public void UnlockSecret()
    {
        UnlockSecretLevelIndex();

        Debug.Log("Secret Unlocked");
        if (gameController != null)
        {
            audioManager.PlaySoundEffect(secretUnlockedSound);
        }

        secretButtons.SetActive(false);
    }
    public void UnlockSecretLevelIndex()
    {
        if (!gameController.unlockedLevels.Contains(secretLevelIndex))
        {
            gameController.unlockedLevels.Add(secretLevelIndex);
            gameController.SaveGame();
        }
    }
    public bool SecretUnlocked()
    {
        if (gameController.unlockedLevels.Contains(secretLevelIndex))
        {
            return true;
        }
        return false;
    }

}
