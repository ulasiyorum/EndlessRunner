using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public static float volume;
    [SerializeField] Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        volume = PlayerPrefs.GetFloat("volume", 1);
        AudioListener.volume = volume;
        volumeSlider.value = volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateVolume()
    {
        volume = volumeSlider.value;
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
    }
}
