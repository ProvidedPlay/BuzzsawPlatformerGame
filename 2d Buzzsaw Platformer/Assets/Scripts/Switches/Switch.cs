using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Switch : MonoBehaviour
{
    public string switchName;
    public bool resetOnGround;
    public bool flipLinkedSwitch;
    public bool linkedSwitch;
    public bool activateByDirection;
    public Sprite activeSprite;
    public Sprite inActiveSprite;
    public string[] interruptableAbilityNames;
    public float deactivationCheckTimer = 0;
    public float activationCheckTimer = 0;
    [HideInInspector] public bool switchActive;
    [HideInInspector] public bool stoppingMotion;
    [HideInInspector] public SpriteRenderer sp;
    [HideInInspector] public Player player;
    [HideInInspector] public List<MoveAbility> interruptableAbilities;
    [HideInInspector] public Collider2D col;
    [HideInInspector] public GameController gameController;
    [HideInInspector] public TronTail tronTail;

    public virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        tronTail = GameObject.FindGameObjectWithTag("Player").GetComponent<TronTail>();
        sp = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        SetupSwitch();

    }
    public virtual void SetupSwitch()
    {
        UpdateSprite();
        FindInterruptableAbilities();
    }
    void FindInterruptableAbilities()
    {
        foreach (MoveAbility moveAbility in player.moveAbilities)
        {
            foreach (string interruptableAbilityName in interruptableAbilityNames)
            {
                if (interruptableAbilityName == moveAbility.abilityName)
                {
                    interruptableAbilities.Add(moveAbility);
                }
            }
        }
    }
    public virtual void UpdateSprite()
    {
        sp.sprite = switchActive ? activeSprite : inActiveSprite;
    }
    public virtual void ActivateSwitch()
    {
        if (!switchActive)
        {
            //switchActive = true;
            DoAction();
        }
    }
    public abstract void DoAction();
    public virtual void ResetSwitch()
    {
        if (switchActive)
        {
            switchActive = false;
            UpdateSprite();
        }
    }
    public void InterruptAbilities()
    {
        foreach (MoveAbility interruptableAbility in interruptableAbilities)
        {
            if (interruptableAbility.moveActive)
            {
                interruptableAbility.CancelAction(true);

                interruptableAbility.ResetAction(); //TEST THIS OUT FIRST
                if (!interruptableAbility.canAct)
                {
                    interruptableAbility.canAct = true;
                }
            }
        }
    }
    public virtual void CancelAction(bool interrupted)
    {

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !switchActive && !gameController.gameOver)
        {
            if (!activateByDirection)
            {
                ActivateSwitch();
            }
        }
    }
}
