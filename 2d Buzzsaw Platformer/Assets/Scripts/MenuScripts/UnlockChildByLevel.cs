using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockChildByLevel : MonoBehaviour
{
    public int unlockingLevel;
    public GameObject childObject;
    GameController gameController;


    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    private void Start()
    {
        CheckToUnlockChild();
    }
    private void OnEnable()
    {
        CheckToUnlockChild();
    }

    void CheckToUnlockChild()
    {
        bool unlockChild = gameController.unlockedLevels.Contains(unlockingLevel);
        if (unlockingLevel != 0 && childObject != null)
        {
            childObject.SetActive(unlockChild);
        }
    }
}
