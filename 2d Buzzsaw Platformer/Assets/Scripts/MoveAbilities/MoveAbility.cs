using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveAbility : MonoBehaviour
{
    [HideInInspector] public  int counter;
    [HideInInspector] public bool movementIsEnabled;
    [HideInInspector] public bool hitKey;
    [HideInInspector] public bool canAct;
    [HideInInspector] public float actTimer;
    [HideInInspector] public bool moveActive = false;
     public List<Switch> interruptableSwitches;

    public string abilityName;
    public int maxCounter;
    public float actDelay;
    public string inputName;
    public MoveAbility[] interruptableAbilities;
    public MoveAbility[] replacedAbilities;
    public MoveAbility[] replacedByAbilities;
    public string[] interruptableSwitchNames;

    [HideInInspector] public Player player;
    [HideInInspector] public GameController gameController;
    public virtual void Awake()
    {
        player = GetComponent<Player>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        FindInterruptableSwitches();
    }
    private void OnEnable()
    {
        hitKey = false;
        counter = 0;
        actTimer = 0;
    }
    public virtual void Update()
    {
        if (!canAct)
        {
            if (counter < maxCounter && Time.time > actTimer)
            {
                canAct = true;
            }
        }
    }
    public abstract void DoAction(bool input);
    public virtual void ResetAction()
    {
        counter = 0;
    }
    public virtual void EnableMovement()
    {
        if (!CheckIfReplaced())
        {
            movementIsEnabled = true;
            ReplaceAbilities();
            CheckCanAct();
        }
    }
    public virtual void CancelAction(bool movementInterrupted)
    {

    }
    public virtual void CheckCanAct()
    {
        if (canAct)
        {
            if (counter >= maxCounter || Time.time < actTimer)
            {
                canAct = false;
            }
        }
    }
    public void FindInterruptableSwitches()
    {
        player.FindLevelSwitches();
        foreach (Switch levelSwitch in player.levelSwitches)
        {
            foreach (string interruptableSwitchName in interruptableSwitchNames)
            {
                if (levelSwitch.switchName == interruptableSwitchName)
                {
                    interruptableSwitches.Add(levelSwitch);
                }
            }
        }
    }
    public void InterruptSwitches()
    {
        foreach (Switch interruptableSwitch in interruptableSwitches)
        {
            if (interruptableSwitch.stoppingMotion)
            {
                interruptableSwitch.CancelAction(true);
            }
        }
    }
    public void InterruptAbilities()
    {
        foreach (MoveAbility interruptableAbility in interruptableAbilities)
        {
            if (interruptableAbility.moveActive)
            {
                interruptableAbility.CancelAction(true);
            }
        }
    }
    public void ReplaceAbilities()
    {
        foreach (MoveAbility replacedAbility in replacedAbilities)
        {
            if (replacedAbility.movementIsEnabled)
            {
                counter = replacedAbility.counter;
                replacedAbility.movementIsEnabled = false;
            }
        }
    }
    public bool CheckIfReplaced()
    {
        foreach (MoveAbility replacedByAbility in replacedByAbilities)
        {
            if (replacedByAbility.movementIsEnabled)
            {
                return true;
            }
        }
        return false;
    }
    public virtual void PlaySound(string sound)
    {
        if (gameController != null)
        {
            if (!gameController.gameOver)
            {
                player.audioManager.PlaySoundEffect(sound);
            }
            
        }
    }
}
