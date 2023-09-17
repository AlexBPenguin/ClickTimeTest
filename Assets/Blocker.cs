using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net.NetworkInformation;

public class Blocker : MonoBehaviour
{
    public EnemyScriptableObject enemy;

    public bool midCombo;

    public GameObject atkObject;

    public int atkIndex;
    public float waitIndex;

    //retaliate stuff
    public bool retaliating;
    public int retaliationChance;
    public int retaliationAtkChance;
    public bool swipedUpOnTime;

    public Transform spawner;

    //Display stuff
    public int enemyHealth;
    public int playerHealth;
    public int playerPostureCount;
    public int enemyPostureCount;

    //Display References
    public EnemyDisplay enemyDisplay;
    public PlayerDisplay playerDisplay;

    //EnemySpriteReferences
    public EnemySpriteHandler enemySpriteHandler;
    //PlayerSpriteReference
    public PlayerSpriteHandler playerSpriteHandler;

    //camera shake references
    public CameraShakeScript cameraShakeScript;
    //enemy shake references
    public EnemyShakeScript enemyShakeScript;

    //material swap
    public SpriteRenderer spriteRenderer;
    public Material flashMaterial;
    private Material originalMaterial;
    private Color currentColor;
    public float flashDuration;
    private Coroutine flashRoutine;

    //Audio Reference
    private AudioManager audioManager;

    public int deflectCount;
    public int blockTryCount;

    public float blockTimer;

    public float playerAtkDelay;
    public bool midAttack;
    public bool atkRest;
    public bool atkInputDelay;
    public float atkInputDelayTime;
    public bool atkCommit;
    public bool blockInputDelay;
    public float blockInputDelayTime;
    public bool blockButtonUpDuringInputDelay;

    public float deflectWindowTime = 0.5f;
    public float mouseInputTime;
    public bool canMouseInput;

    public bool spam;
    public bool spamCheck;
    public float spamWindowTime;

    public float currentMouseButtonTime;
    public float previousMouseButtonTime;

    int androidAttackSwitch;
    int androidPlayerAttack;
    int androidPlayerBlock;
    int androidPlayerHit;
    int streamId;

    public bool block;
    public bool holdingBlock;
    public bool blockOnTime;
    public bool deflected;

    public bool tookDmg;
    public float dmgStunTime;

    public bool disableButtons;

    public bool enemyNeutralStance;
    public bool playerNeutralStance;

    private float playerPostureTimer = 1.25f;
    private float playerStanceTimer = 1f;

    private float enemyPostureTimer = 1.0f;
    public bool enemyPostureEmpty;
    public float postureMultiplier;

    //swiping stuff
    public Vector2 startMousePos;
    public Vector2 endMousePos;
    public bool canSwipe;
    public bool swipedUp;
    public bool swipedDown;

    // Start is called before the first frame update
    void Start()
    {
        //Material stuff
        originalMaterial = spriteRenderer.material;

        //health bar stuff
        enemyHealth = enemyDisplay.enemy.health;
        playerHealth = playerDisplay.playerMaxHealth;

        //Audio refernce
        audioManager = FindObjectOfType<AudioManager>();

        canMouseInput = true;
        deflectCount = 0;

        //android audio set up
        AndroidNativeAudio.makePool();
        androidAttackSwitch = AndroidNativeAudio.load("Android Native Audio/465338__o-toener__zap.ogg");
        androidPlayerAttack = AndroidNativeAudio.load("Android Native Audio/334169__loudernoises__sword-clash (1).wav");
        androidPlayerBlock = AndroidNativeAudio.load("Android Native Audio/500927__sawuare__wood-click-3");
        androidPlayerHit = AndroidNativeAudio.load("Android Native Audio/HitAudio");
    }

    // Update is called once per frame
    void Update()
    {
        //swipe input
        if (holdingBlock)
        {
            endMousePos = Input.mousePosition;
        }


        if (!midCombo) //if enemy is not in the middle of an atk combo
        {
            //return to orignal size
            enemySpriteHandler.ReturnToOriginalSpriteSize();
        }
        else
        {
            //shrink before attacking
            enemySpriteHandler.ShrinkSpriteSize();
        }

        //stun player when player posture bar is full
        if(playerPostureCount > playerDisplay.playerMaxPosture)
        {
            disableButtons = true;
            Invoke(nameof(EnableButtons), 1);
        }

        if(enemyPostureCount > enemyDisplay.enemy.posture)
        {
            enemyHealth -= 10;
            enemyDisplay.SetHealth(enemyHealth);
            enemyPostureCount = 0;
            enemyDisplay.SetPosture(enemyPostureCount);

        }

        //only regain posture when enemy is in neutral stance and enemy has more than zero posture
        if (enemyNeutralStance && !enemyPostureEmpty)
        {
            enemyPostureTimer -= Time.deltaTime;

            if (enemyPostureTimer <= 0)
            {
                enemyPostureCount--;
                enemyDisplay.SetPosture(enemyPostureCount);
                postureMultiplier = enemyHealth / 120; 
                enemyPostureTimer = 1f - postureMultiplier; // Reset the timer

                if (enemyPostureCount <= 0)
                {
                    enemyPostureCount = 0;
                    enemyDisplay.SetPosture(enemyPostureCount);
                    enemyPostureEmpty = true;
                }
            }
        }
        if(enemyPostureCount > 0)
        {
            enemyPostureEmpty = false;
        }



        if (playerNeutralStance)
        {
            playerPostureTimer -= Time.deltaTime;

            if (playerPostureTimer <= 0)
            {
                playerPostureCount--;
                playerDisplay.SetPosture(playerPostureCount);
                if (holdingBlock && enemyNeutralStance)
                {
                    playerPostureTimer = 0.75f;
                }
                else
                {
                    playerPostureTimer = 1.25f;
                }
                

                if (playerPostureCount <= 0)
                {
                    playerPostureCount = 0;
                    playerDisplay.SetPosture(playerPostureCount);
                    playerNeutralStance =false;
                    
                }
            }
        }

        if (!playerNeutralStance)
        {
            playerStanceTimer -= Time.deltaTime;

            if(playerStanceTimer <= 0)
            {
                playerNeutralStance = true;
            }
        }

        if (block)
        {
            blockTimer += Time.deltaTime;


            //put this under OntriggerEnter/ when attack hits trigger/when canBlock == true
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCombo();

        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            //SpawnComboTwo();
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Click");

            previousMouseButtonTime = currentMouseButtonTime;
            currentMouseButtonTime = Time.time;
            //CancelInvoke("SpamFalse");
            //Invoke("SpamFalse", 0.3f);
        }

        /*
        if (Input.GetMouseButtonDown(0) && !canMouseInput)
        {
            Debug.Log("can't input mouse");
            spam = true;
            Invoke("SpamFalse", 0.3f);
        }*/

        if(currentMouseButtonTime - previousMouseButtonTime < 0.25f)
        {
            //Debug.Log("spam");
        }

        //if (Input.GetMouseButtonDown(0) && canMouseInput)

        //THIS IS THE OG Block BUTTON
        /*
        if (Input.GetMouseButtonDown(0) && !tookDmg)
        {
            //if(spamCheck && !blockOnTime)
            if (spamCheck)
            {
                Debug.Log("spammer");
                //CancelInvoke("ResetBlock");
                CancelInvoke("ResetSpamCheck");
                CancelInvoke("SpamFalse");
                spam = true;
                Invoke("SpamFalse", 0.5f);
            }

            spamCheck = true;

            StartCoroutine(ResetMouseButtonAfterDelay());
            canMouseInput = false;
            block = true;
            blockTryCount++;
           
            //deflectWindowTime -= (blockTryCount * 0.1f);
            
            if(deflectWindowTime > 0.06f)
            {
                deflectWindowTime -= 0.05f;
            }

            //mouseInputTime = 0.25f;

            //StartCoroutine(ResetIsClickedAfterDelay());
            Invoke("ResetBlock", deflectWindowTime);
            Invoke("ResetSpamCheck", spamWindowTime);
        }*/

        //swipe stuff
        if(swipedUp && swipedUpOnTime)
        {
            Debug.Log("MikiriCounter");
            enemyPostureCount += 2;
            enemyDisplay.SetPosture(enemyPostureCount);
            swipedUp = false;
            swipedUpOnTime = false;

            //recycled shit
            Debug.Log("Deflected");
            CancelInvoke("DeflectedTimer");
            deflected = true;
            Invoke("DeflectedTimer", 0.225f);

            audioManager.Play("Player Deflect");
            streamId = AndroidNativeAudio.play(androidPlayerBlock);

            deflectWindowTime = 0.225f;
            mouseInputTime = 0.1f;
            StopCoroutine(ResetMouseButtonAfterDelay());
            canMouseInput = true;
            deflectCount++;
            block = false;
            blockOnTime = false;
            blockTryCount = 0;

            spamCheck = false;
            CancelInvoke("ResetSpamCheck");
        }

        if (block && blockOnTime)
        {
            Debug.Log("Deflected");
            CancelInvoke("DeflectedTimer");
            deflected = true;
            Invoke("DeflectedTimer", 0.225f);

            //camera shake
            //cameraShakeScript.shakeIntensity = 1.05f;
            //cameraShakeScript.shakeTime = 0.08f;
            //cameraShakeScript.ShakeCamera();

            //shake enemy
            enemyShakeScript.ShakeMe();

            audioManager.Play("Player Deflect");
            streamId = AndroidNativeAudio.play(androidPlayerBlock);
            //Debug.Log("Delfect!");
            if (!spam)
            {

                enemyPostureCount++;
                enemyDisplay.SetPosture(enemyPostureCount);

                if (playerPostureCount != playerDisplay.playerMaxPosture)
                {
                    playerPostureCount++;
                    playerDisplay.SetPosture(playerPostureCount);
                }
                
            }
            else
            {
                playerPostureCount += 2;
                playerDisplay.SetPosture(playerPostureCount);

                //camera shake
                cameraShakeScript.shakeIntensity = 1.08f;
                cameraShakeScript.shakeTime = 0.125f;
                cameraShakeScript.ShakeCamera();
            }
            
            deflectWindowTime = 0.225f;
            mouseInputTime = 0.1f;
            StopCoroutine(ResetMouseButtonAfterDelay());
            canMouseInput = true;
            deflectCount++;
            block = false;
            blockOnTime = false;
            blockTryCount = 0;

            spamCheck = false;
            CancelInvoke("ResetSpamCheck");
            //this shit above just saved my ass NICE!
        }

        else if (holdingBlock && blockOnTime && !disableButtons)
        {
            //Debug.Log("HoldBlocking");

            audioManager.Play("Player Block");
            streamId = AndroidNativeAudio.play(androidPlayerBlock);

            CancelInvoke("DeflectedTimer");
            deflected = true;
            Invoke("DeflectedTimer", 0.225f);

            playerPostureCount += 2;
            playerDisplay.SetPosture(playerPostureCount);

            //camera shake
            cameraShakeScript.shakeIntensity = 1.08f;
            cameraShakeScript.shakeTime = 0.125f;
            cameraShakeScript.ShakeCamera();

            blockOnTime = false;
        }



    }

    //enemy flash
    public void Flash()
    {
        // If the flashRoutine is not null, then it is currently running.
        if (flashRoutine != null)
        {
            // In this case, we should stop it first.
            // Multiple FlashRoutines the same time would cause bugs.
            StopCoroutine(flashRoutine);
        }

        // Start the Coroutine, and store the reference for it.
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Swap to the flashMaterial.
        currentColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;
        spriteRenderer.material = flashMaterial;

        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(flashDuration);

        // After the pause, swap back to the original material.
        spriteRenderer.color = currentColor;
        spriteRenderer.material = originalMaterial;

        // Set the routine to null, signaling that it's finished.
        flashRoutine = null;
    }

    private IEnumerator ResetIsClickedAfterDelay()
    {
        yield return new WaitForSeconds(deflectWindowTime);
        block = false;
    }

    public void BlockButton()
    {
        //CancelInvoke("DeflectArtReset");
        if (!disableButtons)
        {
            PlayerStanceReset();

            //swipe feature testing
            startMousePos = Input.mousePosition;
            canSwipe = true;
            Invoke("CantSwipe", 0.25f);
            Debug.Log("startmousepos: " + startMousePos);


            //if player is not commited to attack...
            if (!atkCommit && !tookDmg)
            {
                Debug.Log("Block went through");
                atkRest = false;
                CancelInvoke("PlayerAttack");

                //set sword block sprite art
                playerSpriteHandler.BlockShieldArt();
                playerSpriteHandler.NeutralSwordArt();

                //temporary fix for block input delay making holding block equal to true without putting block button back up (since it's on a delay)
                if (!blockButtonUpDuringInputDelay)
                {
                    holdingBlock = true;
                }
                else
                {
                    blockButtonUpDuringInputDelay = false;
                }
                

                //Debug.Log("block");
                //if(spamCheck && !blockOnTime)
                if (spamCheck)
                {
                    Debug.Log("spammer");
                    //CancelInvoke("ResetBlock");
                    CancelInvoke("ResetSpamCheck");
                    CancelInvoke("SpamFalse");
                    spam = true;
                    Invoke("SpamFalse", 0.5f);
                }

                spamCheck = true;

                StartCoroutine(ResetMouseButtonAfterDelay());
                canMouseInput = false;
                block = true;
                blockTryCount++;

                //deflectWindowTime -= (blockTryCount * 0.1f);
                /*
                if (deflectWindowTime > 0.06f)
                {
                    deflectWindowTime -= 0.05f;
                }*/

                //mouseInputTime = 0.25f;

                //StartCoroutine(ResetIsClickedAfterDelay());
                //Invoke("ResetBlockArt", 0.5f);

                //Try to see if this makes things more consisntet or messes stuff up (Added 8/27)
                CancelInvoke("ResetBlock");

                Invoke("ResetBlock", deflectWindowTime);
                Invoke("ResetSpamCheck", spamWindowTime);
            }

            else if (atkRest || tookDmg)
            {
                Debug.Log("BlockInputDelay");
                Invoke("BlockInputDelay", blockInputDelayTime);
                blockInputDelay = true;
            }
        }

    }

    public void BlockButtonUp()
    {
        //swipe feature testing
        Debug.Log("EndMousePos" + endMousePos);
        if(canSwipe && ((endMousePos.y - startMousePos.y) >= 125))
        {
            Debug.Log("SwipedUp!");
            swipedUp = true;
            Invoke("SwipeUpReset", deflectWindowTime);

        }
        else if(canSwipe && ((endMousePos.y - startMousePos.y) <= -125))
        {
            Debug.Log("swipedDown!");
            swipedDown = true;
        }

        Debug.Log("BlockButtonUp1");
        holdingBlock = false;
        if (blockInputDelay)
        {
            blockButtonUpDuringInputDelay = true;
        }

        //i can lift button up before a deflection is counted
        //if (!block && !atkRest)
        if (!block && !deflected && !atkRest)
        {
            Debug.Log("blockbuttonup2");
            CancelInvoke("DeflectArtReset");
            playerSpriteHandler.NeutralSwordArt();
            playerSpriteHandler.NeutralShieldArt();
        }
    }

    private void CantSwipe()
    {
        canSwipe = false;
    }

    private void SwipeUpReset()
    {
        swipedUp = false;
    }

    public void AttackButton()
    {
        if (!disableButtons)
        {
            PlayerStanceReset();

            if ((!atkRest && !tookDmg))
            {
                CancelInvoke("DeflectArtReset");
                playerSpriteHandler.PreAttackArt();

                CancelInvoke("PlayerAttack");
                Invoke("PlayerAttack", playerAtkDelay);
                atkRest = true;
                //midAttack = true;
                block = false;
            }
            else if ((atkRest || tookDmg) && !atkInputDelay)
            {
                Invoke("AtkInputDelay", atkInputDelayTime);
                atkInputDelay = true;
            }
        }
    }

    public void PlayerAttack()
    {
        atkCommit = true;

        //switch sword sprites
        playerSpriteHandler.PostAttackArt();

        //Unity Audio
        audioManager.Play("Player Attack");

        //Android Audio
        streamId = AndroidNativeAudio.play(androidPlayerAttack);
        //midAttack = false;
        enemyHealth--;
        enemyDisplay.SetHealth(enemyHealth);

        //enemy shake
        enemyShakeScript.ShakeMe();


        //Invoke("ResetAtkRest", 0.15f);
        Invoke("ResetAtkRest", playerAtkDelay);

        //pick a number 1-10, if less than 6, enemy "retaliates", quickly attacking back at player
        //this will change sprite pose from here along with instaniating the retaliate attack
        //pause/cancel all other combos
        //ENEMY CAN ONLY RETALIATE FROM THEIR NEUTRAL STANCE (not mid enemy atk combo)

        ///*

        retaliationChance = Random.Range(0, 9);
        if(retaliationChance < 5 && enemyNeutralStance)
        {
            //Debug.Log("retaliate");
            CancelInvoke("SpawnCombo");
            //CancelInvoke("SpawnComboTwo");
            //CancelInvoke("SpawnComboThree");
            //CancelInvoke("SpawnComboFour");
            //atkStartSound.Play();
            audioManager.Play("Attack Switch");
            streamId = AndroidNativeAudio.play(androidAttackSwitch);
            //retaliating = true;

            retaliationAtkChance = Random.Range(0, enemy.counterAtks.Length);

            SpawnCounter();

        }

        //enemy sprite flash
        Flash();
        //*/
    }

    private void PlayerStanceReset()
    {
        playerStanceTimer = 0.5f;
    }


    public void AtkInputDelay()
    {
        atkInputDelay = false;
    }

    public void BlockInputDelay()
    {
        blockInputDelay = false;
    }

    //complete an attack swing
    private void ResetAtkRest()
    {
        Debug.Log("ResetAtkRest");

        atkCommit = false;

        //reset sword pixel art
        playerSpriteHandler.NeutralSwordArt();

        atkRest = false;
        if (atkInputDelay)
        {
            //Invoke("PlayerAttack", playerAtkDelay);
            AttackButton();
        }
        
        //causes something to go wrong (calls Block button and makes block true at the wrong time? sets holding to true (when we don't want that) since we want to reset block?
        else if (blockInputDelay)
        {
            //Invoke("BlockButton", 0f);
            BlockButton();
        }
    }

    private void ResetBlock()
    {
        //rest back to swordntrl sprite art
        if (!holdingBlock && !atkRest)
        {
            //for future change, set current sprite to current sprite and then can just disable it here instead of ever sprite

            Debug.Log("ResetBlock2");
            playerSpriteHandler.NeutralSwordArt();
            playerSpriteHandler.NeutralShieldArt();
        }
        else if (holdingBlock)
        {
            Debug.Log("holding");
        }

        else if (deflected)
        {
            Debug.Log("Deflecting");
        }

        else if (atkRest)
        {
            Debug.Log("AtkResting");
        }

        block = false;
    }

    private void ResetSpamCheck()
    {
        //Debug.Log("CancelCheckSpam");
        spamCheck = false;
    }
    private IEnumerator ResetMouseButtonAfterDelay()
    {
        yield return new WaitForSeconds(mouseInputTime);
        canMouseInput = true;
    }

    private void DeflectArtReset()
    {
        if (block || holdingBlock)
        {
            Debug.Log("DeflectArtResetBlock1");
        }
        else
        {
            Debug.Log("DeflectArtResetNTRL");
            playerSpriteHandler.NeutralShieldArt();
            playerSpriteHandler.NeutralSwordArt();
        }
    }

    private void ResetBlockArt()
    {
        //rest back to swordntrl sprite art
        if (!holdingBlock)
        {
            Debug.Log("ResetBlockArt");
            playerSpriteHandler.NeutralShieldArt();
            playerSpriteHandler.NeutralSwordArt();
        }
    }

    public void SpawnCombo()
    {
        enemyNeutralStance = false;
        Instantiate(enemy.atkCombos[atkIndex], spawner.transform.position, Quaternion.identity);
    }

    public void SpawnCounter()
    {
        enemyNeutralStance = false;
        if(retaliationAtkChance > 0)
        {
            enemySpriteHandler.sprite.material.color = Color.gray;
        }

        else
        {
            enemySpriteHandler.sprite.material.color = Color.green;
        }

        enemySpriteHandler.PulseSprite();
        Instantiate(enemy.counterAtks[retaliationAtkChance], spawner.transform.position, Quaternion.identity);
    }

    private void SpamFalse()
    {
        spam = false;
    }

    private void DeflectedTimer()
    {
        deflected = false;
    }

    public void EnableButtons()
    {
        disableButtons = false;
        playerPostureCount = 0;
        playerDisplay.SetPosture(playerPostureCount);
    }

    private void StartPoseStuff()
    {
        midCombo = true;
        //atkStartSound.Play();
        audioManager.Play("Attack Switch");
        streamId = AndroidNativeAudio.play(androidAttackSwitch);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Enter" + Time.time);

        enemyNeutralStance = false;

        //Debug.Log("Triggered");
        //Debug.Log(Time.time);

        //StopCoroutine(ResetIsClickedAfterDelay());

        //cancel player attack
        //CancelInvoke("PlayerAttack");
        //midAttack = false;
        atkRest = false;


        if (other.gameObject.CompareTag("StartPose"))
        {
            //Debug.Log("StartPose");
            enemySpriteHandler.sprite.material.color = Color.red;
            StartPoseStuff();
        }

        else if (other.gameObject.CompareTag("StartPoseTwo"))
        {
            //Debug.Log("StartPoseTwo");
            enemySpriteHandler.sprite.material.color = new Color(0.25f, 0.65f, 0.85f);
            StartPoseStuff();

        }

        else if (other.gameObject.CompareTag("StartPoseThree"))
        {
            enemySpriteHandler.sprite.material.color = new Color(1, 0.3f, 0.5f);
            StartPoseStuff();

        }

        else if (other.gameObject.CompareTag("StartPoseFour"))
        {
            enemySpriteHandler.sprite.material.color = new Color(0, 1, 0.13f);
            StartPoseStuff();

        }

        //swiping up stuff
        else if (other.gameObject.CompareTag("Retaliate2Up"))
        {
            
            Debug.Log("Retaliate2Up");

            CancelInvoke("PlayerAttack");
            PlayerStanceReset();


            swipedUpOnTime = true;


        }

        else if (!spam)
        {
            
            //cancel player attack
            CancelInvoke("PlayerAttack");

            PlayerStanceReset();
            blockOnTime = true;
            //Invoke("onTriggerFunction", 0.2f);
        }
        else
        {   
            //cancel player attack
            CancelInvoke("PlayerAttack");

            PlayerStanceReset();
            blockOnTime = true;
        }

        //pulse to demonstrate attack/add some "juice"
        enemySpriteHandler.PulseSprite();

        //why is this happening again?
        //CancelInvoke("ResetBlock");

        


        //Debug.Log("OnTriggerEnter" + Time.time);

    }

    private void onTriggerFunction()
    {
        //NOT IN USE AS OF NOW

        //cancel player attack
        CancelInvoke("PlayerAttack");

        PlayerStanceReset();
        blockOnTime = true;


        enemySpriteHandler.PulseSprite();
    }//not in use

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Exit" + Time.time);

        CancelInvoke("DeflectedTimer");

        if (other.gameObject.CompareTag("FinalAtk") || other.gameObject.CompareTag("RetaliateOne") || other.gameObject.CompareTag("Retaliate2Up"))
        {
            midCombo = false;//for decrease in size animation

            enemySpriteHandler.sprite.material.color = Color.white;
            enemyNeutralStance = true;

            atkIndex = Random.Range(0, enemy.atkCombos.Length);
            waitIndex = Random.Range(0, 1.75f);

            Invoke("SpawnCombo", waitIndex);
        }

        blockOnTime = false;

        if (!deflected && (!other.gameObject.CompareTag("StartPose") && !other.gameObject.CompareTag("StartPoseTwo") && !other.gameObject.CompareTag("StartPoseThree") && !other.gameObject.CompareTag("StartPoseFour")))
        {
            //take posture damage
            playerPostureCount += 2;
            playerDisplay.SetPosture(playerPostureCount);

            //take health damage
            playerHealth--;
            playerDisplay.SetHealth(playerHealth);

            //take damage audio
            audioManager.Play("Player Hit");
            streamId = AndroidNativeAudio.play(androidPlayerHit);

            //camera shake
            cameraShakeScript.shakeIntensity = 1.25f;
            cameraShakeScript.shakeTime = 0.2f;
            cameraShakeScript.ShakeCamera();


            if (!tookDmg)
            {
                tookDmg = true;
                Invoke("DamageStun", dmgStunTime);
            }
            

        }

        deflected = false;
        //Debug.Log("OntriggerExit" + Time.time);
        Destroy(other.gameObject);
    }

    private void DamageStun()
    {
        tookDmg = false;
    }

    //git kraken test
}
