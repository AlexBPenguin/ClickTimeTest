using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Awake()
    {
        //Unity Audio
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip; 

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.playOnAwake = s.playOnAwake;
            s.source.loop = s.loop;
            s.source.mute = s.mute;
        }
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, s => s.name == name);
        s.source.Play();
    }
}
