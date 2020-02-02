using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class Player : MonoBehaviour
{
    public float playerMoveSpeed;
    public float jumpStrength;
    public float multiJumpStrength;
    public float delayedJumpPeriod;
    public MoveAbility[] moveAbilities;
    public string[] delayedActionAbilities;
    public GameObject spriteHandler;
    [HideInInspector] public bool tronTailActive;
    [HideInInspector] public bool inTronZone;
    [HideInInspector] public bool globalLightOn;
    [HideInInspector] public bool globalLightPresent;
    [HideInInspector] public float previousGravityScale;
    [HideInInspector] public float delayedJumpTimer;
    [HideInInspector] public bool wallLeft;
    [HideInInspector] public bool wallRight;
    [HideInInspector] public bool playerIsDead = false;
    [HideInInspector] public bool playerReachedGoal = false;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isGroundedMovingPlatform =false;
    [HideInInspector] public bool canWalk;
    [HideInInspector] public bool gravityReversed;
    [HideInInspector] public bool delayedJumpTimerActive;
    [HideInInspector] public bool playerIsWalking;
    [HideInInspector] public AudioManager audioManager;
    [HideInInspector] public List<Switch> levelSwitches;
    [HideInInspector] public GameObject[] levelSwitchObjects;
    [HideInInspector] public Light2D levelLight;
    [HideInInspector] public DashEcho dashEcho;
    [HideInInspector] public bool flipX;
    [HideInInspector] public Rigidbody2D rb;
    Animator anim;
    GameController controller;
    SpriteRenderer sp;
    Collider2D col;
    Light2D playerLight;
    
    



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = spriteHandler.GetComponent<Animator>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        sp = spriteHandler.GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        playerLight = GameObject.FindGameObjectWithTag("PlayerLight").GetComponent<Light2D>();
        dashEcho = gameObject.GetComponentInChildren<DashEcho>();
        FindLevelLight();
        FindLevelSwitches();
    }
    private void Start()
    {
        previousGravityScale = rb.gravityScale;
        canWalk = true;
    }
    private void Update()
    {
        if (!controller.gamePaused)
        {
            InputMovements();
            DelayedJumpTimerCountdown();
        }
    }
    private void FixedUpdate()
    {
        Movement();
    }
    void Movement()
    {
        if (!playerIsDead && !playerReachedGoal)
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            DoMovements();
            if (canWalk)
            {
                Walk(moveHorizontal);
            }

        }
    }
    void Walk(float horizontal)
    {
        //float currentHorizontal = horizontal == 0 ? horizontal : Mathf.Sign(horizontal);
        float currentHorizontal = horizontal;
        if (wallLeft)
        {
            currentHorizontal = Mathf.Clamp(currentHorizontal, 0, Mathf.Infinity);
        }
        if (wallRight)
        {
            currentHorizontal = Mathf.Clamp(currentHorizontal, Mathf.NegativeInfinity, 0);
        }
        float horizontalMovement = currentHorizontal * playerMoveSpeed * Time.fixedDeltaTime;
        Vector2 newPosition = rb.position;
        newPosition.x = newPosition.x + horizontalMovement;
        rb.position = newPosition;
        SetPlayerAnimationWalk(currentHorizontal);
        FacePlayer(currentHorizontal);
    }
    public void ActivateDelayedJumpTimer()
    {
        delayedJumpTimerActive = true;
        delayedJumpTimer = delayedJumpPeriod;
    }
    public void DeactivateDelayedJumpTimer()
    {
        delayedJumpTimerActive = false;
        delayedJumpTimer = 0;
    }
    void DelayedJumpTimerCountdown()
    {
        if (delayedJumpTimerActive)
        {
            if (delayedJumpTimer > 0)
            {
                delayedJumpTimer -= Time.deltaTime;
            }
            else if (delayedJumpTimer <= 0)
            {
                DeactivateDelayedJumpTimer();
            }
        }
    }
    void PlayerDead()
    {
        playerIsDead = true;
        dashEcho.DeactivateDashEcho();
        anim.SetTrigger("playerDead");
        controller.GameOver();
    }
    public void FlipPlayer()
    {
        Vector3 newScale = transform.localScale;
        newScale.y = -transform.localScale.y;
        transform.localScale = newScale;
    }
    public void FacePlayer(float horizontalMovement)
    {
        if(horizontalMovement > 0 && flipX)//(horizontalMovement > 0 && sp.flipX)
        {
            //sp.flipX = false;
            flipX = false;
            Vector3 newScale = spriteHandler.transform.localScale;
            newScale.x = 1;
            spriteHandler.transform.localScale = newScale;
        }
        else if (horizontalMovement < 0 && !flipX)//(horizontalMovement < 0 && !sp.flipX)
        {
            //sp.flipX = true;
            flipX = true;
            Vector3 newScale = spriteHandler.transform.localScale;
            newScale.x = -1;
            spriteHandler.transform.localScale = newScale;
        }
    }
    public void SetPlayerAnimationWalk(float walkValue)
    {
        bool playerIsCurrentlyWalking = walkValue == 0 ? false : true;
        if (playerIsCurrentlyWalking != playerIsWalking)
        {
            anim.SetBool("playerIsWalking", playerIsCurrentlyWalking);
            playerIsWalking = playerIsCurrentlyWalking;
        }
    }
    public void SetPlayerAnimationGrounded(bool isGrounded)
    {
        anim.SetBool("playerGrounded", isGrounded);
    }
    public void SetPlayerAnimationJump()
    {
        anim.SetTrigger("playerJump");
    }
    void PlayerReachedGoal()
    {
        anim.enabled = false;
        sp.enabled = false;
        playerLight.enabled = false;
        dashEcho.DeactivateDashEcho();
        playerReachedGoal = true;
        PlaySound("Win Sound");
        controller.GameOver();
    }
    public void FindLevelLight()
    {
        GameObject levelLightObject = GameObject.FindGameObjectWithTag("LevelLight");
        if (levelLightObject != null)
        {
            globalLightPresent = true;
            levelLight = levelLightObject.GetComponent<Light2D>();
        }
    }
    public void FindLevelSwitches()
    {
        if (levelSwitches.Count ==0)
        {
            levelSwitchObjects = GameObject.FindGameObjectsWithTag("Switch");
            foreach (GameObject levelSwitchObject in levelSwitchObjects)
            {
                levelSwitches.Add(levelSwitchObject.GetComponent<Switch>());
            }
        }
    }
    public void GroundResetSwitches()
    {
        if (!playerIsDead)
        {
            bool switchReset = false;
            foreach (Switch levelSwitch in levelSwitches)
            {
                if (levelSwitch.resetOnGround)
                {
                    if (levelSwitch.switchActive)
                    {
                        levelSwitch.ResetSwitch();
                        switchReset = true;
                    }
                }
            }
            if (switchReset)
            {
                switchReset = false;
                PlaySound("Switch Off");
            }
        }
    }
    public void SetLinkedSwitchesActive(string switchName, bool setActive)
    {
        foreach (Switch levelSwitch in levelSwitches)
        {
            if (levelSwitch.linkedSwitch && levelSwitch.switchName == switchName)
            {
                if (setActive)
                {
                    levelSwitch.switchActive = true;
                    if (levelSwitch.flipLinkedSwitch)
                    {
                        //levelSwitch.sp.flipY = !levelSwitch.sp.flipY;
                        Vector3 newScale = levelSwitch.transform.localScale;
                        newScale.y = -levelSwitch.transform.localScale.y;
                        levelSwitch.transform.localScale = newScale;
                    }
                    levelSwitch.UpdateSprite();
                    levelSwitch.deactivationCheckTimer = 0;
                }
                if (!setActive)
                {
                    levelSwitch.ResetSwitch();
                    //levelSwitch.deactivationCheckTimer = 0;
                }
            }
        }
    }
    public void UpdateLinkedLightSwitches(string switchName)
    {
        foreach (Switch levelSwitch in levelSwitches)
        {
            if (levelSwitch.linkedLight && levelSwitch.switchName == switchName)
            {
                levelSwitch.UpdateSprite();
                levelSwitch.UpdateSwitchLight();
            }
        }
    }
    //public void ResetLinkedTimers(string switchName)
    //{
    //    foreach (Switch levelSwitch in levelSwitches)
    //    {
    //        if (levelSwitch.linkedSwitch && levelSwitch.switchName == switchName)
    //        {
    //            if (levelSwitch.deactivationCheckTimer != 0)
    //            {
    //                levelSwitch.deactivationCheckTimer = 0;
    //            }
    //        }
    //    }
    //}
    public bool CheckLinkedSwitchesColliderOverlap(string switchName)
    {
        foreach (Switch levelSwitch in levelSwitches)
        {
            if (levelSwitch.linkedSwitch && levelSwitch.switchName == switchName)
            {
                if (col != null && levelSwitch.col != null)
                {
                    if (col.bounds.Intersects(levelSwitch.col.bounds))
                    {
                        return true;
                    }
                }
            }
            
        }
        return false;
    }
    public void ToggleLevelLight(bool toggleOn)
    {
        if (levelLight != null)
        {
            levelLight.enabled = toggleOn;
            playerLight.enabled = !toggleOn;
            globalLightOn = toggleOn;
        }
    }
    void InputMovements()
    {
        foreach (MoveAbility moveAbility in moveAbilities)
        {
            if (moveAbility.movementIsEnabled)
            {
                if(!moveAbility.hitKey)// && moveAbility.canAct)
                {
                    moveAbility.hitKey = Input.GetButtonDown(moveAbility.inputName);
                }
            }
        }
    }
    void DoMovements()
    {
        foreach (MoveAbility moveAbility in moveAbilities)
        {
            if (moveAbility.movementIsEnabled)
            {
                moveAbility.DoAction(moveAbility.hitKey);
                moveAbility.hitKey = false;
            }
        }
    }
    public void ResetMovements()
    {
        for (int i = 0; i < moveAbilities.Length; i++)
        {
            if (moveAbilities[i].movementIsEnabled)
            {
                if (moveAbilities[i].counter != 0)
                {
                    moveAbilities[i].ResetAction();
                }
            }
        }
        if (delayedJumpTimerActive)
        {
            foreach (string delayedActionAbility in delayedActionAbilities)
            {
                ForceMovementInput(delayedActionAbility);
            }
            DeactivateDelayedJumpTimer();
        }
    }
    public void ForceMovementInput(string abilityName)
    {
        foreach (MoveAbility moveAbility in moveAbilities)
        {
            if (moveAbility.abilityName == abilityName)
            {
                if (moveAbility.movementIsEnabled)
                {
                    if (!moveAbility.hitKey && moveAbility.canAct)
                    {
                        moveAbility.hitKey = true;
                    }
                }
            }
        }
    }
    void AddPowerup(PowerUp powerUp)
    {
        foreach (MoveAbility moveAbility in moveAbilities)
        {
            if (moveAbility.abilityName == powerUp.abilityName && moveAbility.movementIsEnabled == false)
            {
                moveAbility.EnableMovement();
                controller.AddPowerUp(powerUp);
            }
        }
    }
    public void PlaySound(string sound)
    {
        if (controller != null && !controller.gameOver)
        {
            audioManager.PlaySoundEffect(sound);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!controller.gameOver)
            {
                PlaySound("Hit");
            }
            
            PlayerDead();
        }
        if (collision.CompareTag("WinTile"))
        {
            if (!playerIsDead)
            {
                PlayerReachedGoal();
            }
        }
        if (collision.CompareTag("PowerUp"))
        {
            PlaySound("Power Up");
            PowerUp powerUp = collision.GetComponent<PowerUp>();
            if (powerUp != null && !playerIsDead)
            {
                AddPowerup(powerUp);
                collision.gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Tron Tail Zone"))
        {
            if (!inTronZone)
            {
                inTronZone = true;
            }
        }
    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Tron Tail Zone"))
    //    {
    //        if (true)
    //        {

    //        }
    //        inTronZone = false;
    //    }
    //}

}
