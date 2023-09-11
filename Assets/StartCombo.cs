using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCombo : MonoBehaviour
{
    public float time;

    public float[] comboOne = { };
    public int arrayCount;
    public float atkTime;

    public bool canAtk;
    public AudioSource atkSound;
    public AudioSource deflectSound;


    public bool deflected;
    int soundId;
    int soundTwoId;
    int streamId;

    // Start is called before the first frame update
    void Start()
    {
        AndroidNativeAudio.makePool();
        //soundId = AndroidNativeAudio.load("201766__waveplaysfx__tick.wav");
        soundId = AndroidNativeAudio.load("Android Native Audio/Tone Native.wav");
        soundTwoId = AndroidNativeAudio.load("Android Native Audio/201766__waveplaysfx__tick.wav");
        arrayCount = 0;
        canAtk = false;
    }

    // Update is called once per frame
    void Update()
    {
        time = Time.time;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("StartCombo");
            StartTheCombo();
        }

        if(Time.time >= atkTime && canAtk)
        {
            canAtk = false;

            PlayAtkAudio();
 
            //Debug.Log("stop time");
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("MouseDown");
            if(Time.time > (atkTime - 0.4f) && Time.time <= (atkTime + 0.1f))
            {
                Debug.Log("deflect");
                deflected = true;
                //deflectSound.Play();
                streamId = AndroidNativeAudio.play(soundTwoId);
            }
        }
    }

    public void StartTheCombo()
    {
        atkTime = Time.time + comboOne[arrayCount];
        canAtk = true;
       // Debug.Log("can atk");
    }

    private void PlayAtkAudio()
    {
        //Debug.Log("play audio");
        //atkSound.Play();

        if (!deflected)
        {
            Debug.Log("AtkHidAudio");
            streamId = AndroidNativeAudio.play(soundId);
        }
        else
        {
            Debug.Log("DeflectAudio");
            //streamId = AndroidNativeAudio.play(soundTwoId);
            deflected = false;
        }

        if(arrayCount < (comboOne.Length - 1))
        {
            arrayCount++;
            StartTheCombo();
        }

        else
        {
            arrayCount = 0;
        }
        
        
    }

}
