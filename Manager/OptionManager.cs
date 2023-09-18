using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public AudioSource BGM;
    public AudioSource[] Eff;

    public Slider Volume;
    public Slider EffVolume;

    public TMP_Text VolumePer;
    public TMP_Text EffVolumePer;

    public Toggle VolumeMute;
    public Toggle EffVolumeMute;

    // Start is called before the first frame update
    void Start()
    {
        BGM.volume = PlayerPrefs.GetFloat("volume");
        StartCoroutine(Volumeset());
    }
    public void SetVolume()
    {
        PlayerPrefs.SetFloat("volume", Volume.value);
        BGM.volume = PlayerPrefs.GetFloat("volume");
        PlayerPrefs.SetFloat("eff", EffVolume.value);
        foreach (AudioSource source in Eff)
        {
            source.volume = PlayerPrefs.GetFloat("eff");
        }
    }

    IEnumerator Volumeset()
    {
        while (true)
        {
            VolumePer.text = Volume.value.ToString("00%");
            BGM.volume = Volume.value;
            EffVolumePer.text = EffVolume.value.ToString("00%");
            foreach (AudioSource source in Eff)
            {
                source.mute = VolumeMute.isOn;
                source.volume = EffVolume.value;
            }
            BGM.mute = VolumeMute.isOn;

            yield return null;
        }
    }
}
