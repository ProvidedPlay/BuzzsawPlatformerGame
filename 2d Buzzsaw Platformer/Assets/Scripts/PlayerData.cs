using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public List<int> unlockedLevels;
    public Dictionary<int, double> bestLevelTimes = new Dictionary<int, double>();
    public double bestRunTime;

    public PlayerData(GameController gameController)
    {
        unlockedLevels = gameController.unlockedLevels;
        bestLevelTimes = gameController.bestLevelTimes;
        bestRunTime = gameController.bestRunTime;
    }

}
