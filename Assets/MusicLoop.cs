using UnityEngine;

public class MusicLoop : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip musicStart;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.PlayOneShot(musicStart);
        musicSource.PlayScheduled(AudioSettings.dspTime + musicStart.length - 0.1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}