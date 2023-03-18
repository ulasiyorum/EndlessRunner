using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioClips : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    private AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(int id)
    {
        source.PlayOneShot(clips[id]);
    }
}
