using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip[] clips;
    public AudioClip[] runClips;
    public AudioClip[] dieClips;

    private AudioSource source;
    void Start()
    {
        if (GameHandler.Instance.player != null)
            source = GameHandler.Instance.player.GetComponent<AudioSource>();
        else
            source = GetComponent<AudioSource>();
        instance = this;
    }

    void Update()
    {
        
    }
    public static bool IsPlaying()
    {
        return instance.source.isPlaying;
    }
    public static void PlayRun()
    {
        int random = UnityEngine.Random.Range(0, instance.runClips.Length);
        instance.source.clip = instance.runClips[random];
        instance.source.Play();
    }
    public static void PlayDie()
    {
        int random = UnityEngine.Random.Range(0, instance.dieClips.Length);
        instance.source.clip = instance.dieClips[random];
        instance.source.Play();
    }

    public static void Play(int id)
    {
        instance.source.clip = instance.clips[id];
        instance.source.Play();
    }    
    public static void PlayOneShot(int id)
    {
        instance.source.PlayOneShot(instance.clips[id]);
    }

    public static void PlayCollect()
    {
        instance.source.PlayOneShot(instance.clips[2]);
    }
}
