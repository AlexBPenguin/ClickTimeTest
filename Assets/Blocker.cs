using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blocker : MonoBehaviour
{
    public AudioSource atkSound;
    public AudioSource atkSoundWeak;
    public AudioSource atkSoundDeflect;
    public AudioSource atkStartSound;
    public AudioSource atkPlayerSound;
    public AudioSource atkSoundBlock;

    //sword sprite art
    public GameObject swordNtrl;
    public GameObject swordAtk1;
    public GameObject swordAtk2;
    public GameObject swordBlock1;
    public GameObject swordBlock2;
    public GameObject shieldBlockNtrl;

    public GameObject enemySprite;
    public Vector3 enemySpriteCurrentScale;
    public Vector3 startSize;
    public Vector3 smallestSize;
    public Vector3 largestSize;
    public float pulseSize;
    public float returnSpeed;
    public float shrinkSpeed;
    public float expandSpeed;
    public bool enemyAtkPulse;
    public bool midCombo;
    public bool pauseShrink;

    public GameObject atkObject;

    public int atkIndex;
    public float waitIndex;

    //retaliate stuff
    public bool retaliating;
    public int retaliationChance;
    public int retaliationAtkChance;
    public GameObject relatiate1;
    public GameObject relatiate2Up;
    public bool swipedUpOnTime;

    public GameObject atkCombo;
    public GameObject atkCombo1;
    public GameObject atkCombo2;
    public GameObject atkCombo3;
    public GameObject atkCombo4;
    public Transform spawner;
    public TMP_Text deflectCountText;
    public TMP_Text postureText;
    public TMP_Text playerPostureText;
    public TMP_Text playerHealthText;
    public TMP_Text enemyHealthText;

    public int enemyHealth;
    public int enemyMaxHealth = 100;
    public int playerHealth;
    public int playerMaxHealth;
    public int playerPostureCount;
    public int playerMaxPosture;
    //public int enemyMaxPosture;
    public int postureCount;
    public int enemyMaxPosture;
    //Health Bar Referernces
    public HealthBars healthBars;
    //Posutre Bar References
    public PostureBars postureBars;

    
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

    public float deflectWindowTime = 0.5f;
    public float mouseInputTime;
    public bool canMouseInput;

    public bool spam;
    public bool spamCheck;
    public float spamWindowTime;

    public float currentMouseButtonTime;
    public float previousMouseButtonTime;

    int soundId;
    int soundTwoId;
    int soundThreeId;
    int soundFourId;
    int soundFiveId;
    int soundSixId;
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
        startSize = enemySprite.transform.localScale;
        smallestSize = enemySprite.transform.localScale / 2;
        largestSize = enemySprite.transform.localScale * 1.3f;
        
        //health bar stuff
        playerHealth = playerMaxHealth;
        healthBars.SetMaxPlayerHealth(playerMaxHealth);
        enemyHealth = enemyMaxHealth;
        healthBars.SetMaxHealth(enemyMaxHealth);

        //posture bar stuff
        postureBars.SetMaxEnemyPosture(enemyMaxPosture);
        postureBars.SetMaxPlayerPosture(playerMaxPosture);
        
        canMouseInput = true;
        deflectCount = 0;
        AndroidNativeAudio.makePool();
        soundId = AndroidNativeAudio.load("Android Native Audio/Tone Native.wav");
        soundTwoId = AndroidNativeAudio.load("Android Native Audio/201766__waveplaysfx__tick.wav");
        soundThreeId = AndroidNativeAudio.load("Android Native Audio/465338__o-toener__zap.ogg");
        soundFourId = AndroidNativeAudio.load("Android Native Audio/619231__strangehorizon__tiger_sword_13.wav");
        soundFiveId = AndroidNativeAudio.load("Android Native Audio/19421__awfulthesample__awfultheaudio_watschn2.wav");
        soundSixId = AndroidNativeAudio.load("Android Native Audio/334169__loudernoises__sword-clash (1).wav");
    }

    // Update is called once per frame
    void Update()
    {
        //swipe input
        if (holdingBlock)
        {
            endMousePos = Input.mousePosition;
        }


        if (!midCombo && !enemyAtkPulse && !pauseShrink)
        {
            //return to orignal size
            enemySprite.transform.localScale = Vector3.Lerp(enemySprite.transform.localScale, startSize, Time.deltaTime * returnSpeed);
        }
        

        else if(!enemyAtkPulse && !pauseShrink)
        {
            //shrink before attacking
            enemySprite.transform.localScale = Vector3.Lerp(enemySprite.transform.localScale, smallestSize, Time.deltaTime * shrinkSpeed);
        }

        if (enemyAtkPulse && !pauseShrink)
        {
            //quickly expand to demonstrate attack
            enemySprite.transform.localScale = Vector3.Lerp(enemySprite.transform.localScale, largestSize, Time.deltaTime * expandSpeed);
        }

        //show scale in inspector for debugging purposes
        enemySpriteCurrentScale = enemySprite.transform.localScale;

        if(enemySprite.transform.localScale.y >= largestSize.y - 0.01f)
        {
            Debug.Log("Fully Expanded");
            enemyAtkPulse = false;
        }



        postureText.text = "ESP: " + postureCount;
        playerPostureText.text = "SP: " + playerPostureCount;
        playerHealthText.text = "HP: " + playerHealth;
        enemyHealthText.text = "EHP: " + enemyHealth;

        //stun player when player posture bar is full
        if(playerPostureCount > playerMaxPosture)
        {
            disableButtons = true;
            Invoke(nameof(EnableButtons), 1);
        }

        if(postureCount > enemyMaxPosture)
        {
            enemyHealth -= 10;
            healthBars.SetHealth(enemyHealth);
            postureCount = 0;
            postureBars.SetEnemyPosture(postureCount);
        }

        //only regain posture when enemy is in neutral stance and enemy has more than zero posture
        if (enemyNeutralStance && !enemyPostureEmpty)
        {
            enemyPostureTimer -= Time.deltaTime;

            if (enemyPostureTimer <= 0)
            {
                postureCount--;
                postureBars.SetEnemyPosture(postureCount);
                postureMultiplier = enemyHealth / 120; 
                enemyPostureTimer = 1f - postureMultiplier; // Reset the timer

                if (postureCount <= 0)
                {
                    postureCount = 0;
                    postureBars.SetEnemyPosture(postureCount);
                    enemyPostureEmpty = true;
                }
            }
        }
        if(postureCount > 0)
        {
            enemyPostureEmpty = false;
        }



        if (playerNeutralStance)
        {
            playerPostureTimer -= Time.deltaTime;

            if (playerPostureTimer <= 0)
            {
                playerPostureCount--;
                postureBars.SetPlayerPosture(playerPostureCount);
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
                    postureBars.SetPlayerPosture(playerPostureCount);
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
            SpawnComboTwo();
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
            postureCount += 2;
            postureBars.SetEnemyPosture(postureCount);
            swipedUp = false;
            swipedUpOnTime = false;

            //recycled shit
            Debug.Log("Deflected");
            CancelInvoke("DeflectedTimer");
            deflected = true;
            Invoke("DeflectedTimer", 0.25f);

            atkSoundDeflect.Play();
            streamId = AndroidNativeAudio.play(soundFourId);

            deflectWindowTime = 0.225f;
            mouseInputTime = 0.1f;
            StopCoroutine(ResetMouseButtonAfterDelay());
            canMouseInput = true;
            deflectCount++;
            deflectCountText.text = "Deflect: " + deflectCount.ToString();
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
            Invoke("DeflectedTimer", 0.25f);


            atkSoundDeflect.Play();
            streamId = AndroidNativeAudio.play(soundFourId);
            //Debug.Log("Delfect!");
            if (!spam)
            {
                //set sword sprite art to deflect
                swordBlock1.GetComponent<SpriteRenderer>().enabled = false;
                swordBlock2.GetComponent<SpriteRenderer>().enabled = true;

                CancelInvoke("DeflectArtReset");
                Invoke("DeflectArtReset", 0.5f);

                postureCount++;
                postureBars.SetEnemyPosture(postureCount);

                if (playerPostureCount != playerMaxPosture)
                {
                    playerPostureCount++;
                    postureBars.SetPlayerPosture(playerPostureCount);
                }
                
            }
            else
            {
                playerPostureCount += 2;
                postureBars.SetPlayerPosture(playerPostureCount);
            }
            
            deflectWindowTime = 0.225f;
            mouseInputTime = 0.1f;
            StopCoroutine(ResetMouseButtonAfterDelay());
            canMouseInput = true;
            deflectCount++;
            deflectCountText.text = "Deflect: " + deflectCount.ToString();
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

            atkSoundBlock.Play();
            streamId = AndroidNativeAudio.play(soundTwoId);

            CancelInvoke("DeflectedTimer");
            deflected = true;
            Invoke("DeflectedTimer", 0.25f);

            playerPostureCount += 2;
            postureBars.SetPlayerPosture(playerPostureCount);
            blockOnTime = false;
        }



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


            //if (!atkRest && !tookDmg)
            if (!atkCommit && !tookDmg)
            {
                atkRest = false;
                swordAtk1.GetComponent<SpriteRenderer>().enabled = false;
                CancelInvoke("PlayerAttack");

                //set sword block sprite art
                //swordBlock2.GetComponent<SpriteRenderer>().enabled = false;
                //swordNtrl.GetComponent<SpriteRenderer>().enabled = false;
                shieldBlockNtrl.GetComponent<SpriteRenderer>().enabled = false;
                swordBlock1.GetComponent<SpriteRenderer>().enabled = true;

                //temporary fix for block input delay making holding block equal to true without putting block button back up (since it's on a delay)
                if (!blockInputDelay)
                {
                    //holding block
                    holdingBlock = true;
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

        //i can lift button up before a deflection is counted
        if (!block && !deflected && !atkRest)
        {
            Debug.Log("blockbuttonup2");
            CancelInvoke("DeflectArtReset");
            //swordBlock2.GetComponent<SpriteRenderer>().enabled = false;
            swordBlock1.GetComponent<SpriteRenderer>().enabled = false;
            //swordNtrl.GetComponent<SpriteRenderer>().enabled = true;
            shieldBlockNtrl.GetComponent<SpriteRenderer>().enabled = true;
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
                //switch sword sprites
                swordBlock2.GetComponent<SpriteRenderer>().enabled = false;
                swordBlock1.GetComponent<SpriteRenderer>().enabled = false;
                swordNtrl.GetComponent<SpriteRenderer>().enabled = false;
                swordAtk1.GetComponent<SpriteRenderer>().enabled = true;

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
        swordAtk1.GetComponent<SpriteRenderer>().enabled = false;
        swordAtk2.GetComponent<SpriteRenderer>().enabled = true;

        atkPlayerSound.Play();
        streamId = AndroidNativeAudio.play(soundSixId);
        //midAttack = false;
        enemyHealth--;
        healthBars.SetHealth(enemyHealth);
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
            CancelInvoke("SpawnComboTwo");
            CancelInvoke("SpawnComboThree");
            CancelInvoke("SpawnComboFour");
            atkStartSound.Play();
            streamId = AndroidNativeAudio.play(soundThreeId);
            //retaliating = true;

            retaliationAtkChance = Random.Range(0, 2);

            if(retaliationAtkChance == 0)
            {
                //enemySprite.GetComponent<MeshRenderer>().material.color = Color.green;
                enemySprite.GetComponent<SpriteRenderer>().material.color = Color.green;
                enemySprite.transform.localScale = startSize * pulseSize;
                Instantiate(relatiate1, spawner.transform.position, Quaternion.identity);
            }

            else
            {
                //enemySprite.GetComponent<MeshRenderer>().material.color = Color.gray;
                enemySprite.GetComponent<SpriteRenderer>().material.color = Color.gray;
                enemySprite.transform.localScale = startSize * pulseSize;
                Instantiate(relatiate2Up, spawner.transform.position, Quaternion.identity);
            }

        }
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

    private void ResetAtkRest()
    {
        Debug.Log("ResetAtkRest");

        atkCommit = false;

        //reset sword pixel art
        swordBlock2.GetComponent<SpriteRenderer>().enabled = false;
        swordBlock1.GetComponent<SpriteRenderer>().enabled = false;
        swordAtk2.GetComponent<SpriteRenderer>().enabled = false;
        swordNtrl.GetComponent<SpriteRenderer>().enabled = true;
        shieldBlockNtrl.GetComponent<SpriteRenderer>().enabled = true;

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
        //resets twice?
        Debug.Log("ResetBlock1");

        //rest back to swordntrl sprite art
        //if (!holdingBlock && !deflected && !atkRest)
        if (!holdingBlock && !deflected && !atkRest)
        {
            Debug.Log("ResetBlock2");
            //CancelInvoke("ResetBlockArt"); //should just cancel sword block 2 below
            swordBlock2.GetComponent<SpriteRenderer>().enabled = false;
            swordBlock1.GetComponent<SpriteRenderer>().enabled = false;
            swordAtk1.GetComponent<SpriteRenderer>().enabled = false;
            swordNtrl.GetComponent<SpriteRenderer>().enabled = true;
            shieldBlockNtrl.GetComponent<SpriteRenderer>().enabled = true;
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
        
        swordBlock2.GetComponent<SpriteRenderer>().enabled = false;

        //swordBlock1.GetComponent<SpriteRenderer>().enabled = false;//update for shield
        if (block || holdingBlock)
        {
            Debug.Log("DeflectArtResetBlock1");
            swordBlock1.GetComponent<SpriteRenderer>().enabled = true;
            //shieldBlockNtrl.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            Debug.Log("DeflectArtResetNTRL");
            swordNtrl.GetComponent<SpriteRenderer>().enabled = true;
            shieldBlockNtrl.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void ResetBlockArt()
    {
        //rest back to swordntrl sprite art
        if (!holdingBlock)
        {
            Debug.Log("ResetBlockArt");
            swordBlock2.GetComponent<SpriteRenderer>().enabled = false;
            swordBlock1.GetComponent<SpriteRenderer>().enabled = false;
            swordNtrl.GetComponent<SpriteRenderer>().enabled = true;
            shieldBlockNtrl.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void SpawnCombo()
    {
        enemyNeutralStance = false;
        Instantiate(atkCombo1, spawner.transform.position, Quaternion.identity);
    }

    public void SpawnComboTwo()
    {
        enemyNeutralStance = false;
        Instantiate(atkCombo2, spawner.transform.position, Quaternion.identity);
    }

    public void SpawnComboThree()
    {
        enemyNeutralStance = false;
        Instantiate(atkCombo3, spawner.transform.position, Quaternion.identity);

    }

    public void SpawnComboFour()
    {
        enemyNeutralStance = false;
        Instantiate(atkCombo4, spawner.transform.position, Quaternion.identity);
    }

    private void SpamFalse()
    {
        //Debug.Log("CancelSpam");
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
        postureBars.SetPlayerPosture(playerPostureCount);
    }

    private void StartPoseStuff()
    {
        midCombo = true;
        atkStartSound.Play();
        streamId = AndroidNativeAudio.play(soundThreeId);
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
            //enemySprite.GetComponent<MeshRenderer>().material.color = Color.red;
            enemySprite.GetComponent<SpriteRenderer>().material.color = Color.red;
            //atkStartSound.Play();
            //streamId = AndroidNativeAudio.play(soundThreeId);
            StartPoseStuff();

            //enemySprite.transform.localScale = startSize * pulseSize;
        }

        else if (other.gameObject.CompareTag("StartPoseTwo"))
        {
            //Debug.Log("StartPoseTwo");
            //enemySprite.GetComponent<MeshRenderer>().material.color = Color.blue;
            enemySprite.GetComponent<SpriteRenderer>().material.color = new Color(0.25f, 0.65f, 0.85f);
            //atkStartSound.Play();
            //streamId = AndroidNativeAudio.play(soundThreeId);
            StartPoseStuff();

            //enemySprite.transform.localScale = startSize * pulseSize;
        }

        else if (other.gameObject.CompareTag("StartPoseThree"))
        {
            //enemySprite.GetComponent<MeshRenderer>().material.color = Color.black;
            enemySprite.GetComponent<SpriteRenderer>().material.color = new Color(1, 0.3f, 0.5f);//dragon pink
            //atkStartSound.Play();
            //streamId = AndroidNativeAudio.play(soundThreeId);
            StartPoseStuff();

            //enemySprite.transform.localScale = startSize * pulseSize;
        }

        else if (other.gameObject.CompareTag("StartPoseFour"))
        {
            //enemySprite.GetComponent<MeshRenderer>().material.color = Color.yellow;
            enemySprite.GetComponent<SpriteRenderer>().material.color = new Color(0, 1, 0.13f);
            //atkStartSound.Play();
            //streamId = AndroidNativeAudio.play(soundThreeId);
            StartPoseStuff();

            //enemySprite.transform.localScale = startSize * pulseSize;
        }

        //swiping up stuff
        else if (other.gameObject.CompareTag("Retaliate2Up"))
        {
            
            Debug.Log("Retaliate2Up");

            CancelInvoke("PlayerAttack");
            //swordAtk1.GetComponent<SpriteRenderer>().enabled = false;
            //swordNtrl.GetComponent<SpriteRenderer>().enabled = true;
            PlayerStanceReset();
            streamId = AndroidNativeAudio.play(soundId);
            atkSound.Play();

            swipedUpOnTime = true;


        }

        else if (!spam)
        {
            
            //cancel player attack
            CancelInvoke("PlayerAttack");
            //swordAtk1.GetComponent<SpriteRenderer>().enabled = false;
            //swordNtrl.GetComponent<SpriteRenderer>().enabled = true;

            PlayerStanceReset();
            streamId = AndroidNativeAudio.play(soundId);
            atkSound.Play();
            blockOnTime = true;

            //enemyAtkPulse = true;
            

            //new stuff added for enemy pause right before attack as a sort of tell
            //pauseShrink = true;
            //Invoke("onTriggerFunction", 0.2f);
        }
        else
        {   
            //cancel player attack
            CancelInvoke("PlayerAttack");
            //swordAtk1.GetComponent<SpriteRenderer>().enabled = false;
            //swordNtrl.GetComponent<SpriteRenderer>().enabled = true;

            PlayerStanceReset();
            streamId = AndroidNativeAudio.play(soundTwoId);
            atkSoundWeak.Play();
            blockOnTime = true;

            //enemyAtkPulse = true;
        }

        //pulse to demonstrate attack/add some "juice"
        enemySprite.transform.localScale = startSize * pulseSize;

        //a stab at making an "animation" that better displays when an enemy attack will land/gives player something to react to, even though it might be really fast, but at least bot instant like it is
        //enemyAtkPulse = true;

        CancelInvoke("ResetBlock");

        


        //Debug.Log("OnTriggerEnter" + Time.time);

    }

    private void onTriggerFunction()
    {
        //NOT IN USE AS OF NOW

        //cancel player attack
        CancelInvoke("PlayerAttack");
        //swordAtk1.GetComponent<SpriteRenderer>().enabled = false;
        //swordNtrl.GetComponent<SpriteRenderer>().enabled = true;

        PlayerStanceReset();
        streamId = AndroidNativeAudio.play(soundId);
        atkSound.Play();
        blockOnTime = true;

        //enemyAtkPulse = true;

        pauseShrink = false;
        enemySprite.transform.localScale = startSize * pulseSize;
    }//not in use

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Exit" + Time.time);

        CancelInvoke("DeflectedTimer");

        if (other.gameObject.CompareTag("FinalAtk") || other.gameObject.CompareTag("RetaliateOne") || other.gameObject.CompareTag("Retaliate2Up"))
        {
            midCombo = false;//for decrease in size animation

            //enemySprite.GetComponent<MeshRenderer>().material.color = Color.white;
            enemySprite.GetComponent<SpriteRenderer>().material.color = Color.white;
            enemyNeutralStance = true;

            atkIndex = Random.Range(0, 7);
            waitIndex = Random.Range(0, 1.75f);
            if(atkIndex < 2)
            {
                Invoke("SpawnCombo", waitIndex);
            }
            else if(atkIndex == 2)
            {
                Invoke("SpawnComboTwo", waitIndex);
            }
            else if(atkIndex > 2 && atkIndex < 5)
            {
                Invoke("SpawnComboThree", waitIndex);
            }
            else if (atkIndex > 4)
            {
                Invoke("SpawnComboFour", waitIndex);
            }
        }

        blockOnTime = false;

        if (!deflected && (!other.gameObject.CompareTag("StartPose") && !other.gameObject.CompareTag("StartPoseTwo") && !other.gameObject.CompareTag("StartPoseThree") && !other.gameObject.CompareTag("StartPoseFour")))
        {
            playerPostureCount += 2;
            postureBars.SetPlayerPosture(playerPostureCount);
            playerHealth--;
            healthBars.SetPlayerHealth(playerHealth);
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
}
