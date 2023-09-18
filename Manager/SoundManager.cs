using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource BGM;
    public AudioSource[] Eff;
    public AudioClip[] Musics;

    public Slider Volume;
    public Slider EffVolume;

    public TMP_Text VolumePer;
    public TMP_Text EffVolumePer;

    public Toggle VolumeMute;
    public Toggle EffVolumeMute;
    [SerializeField] bool MainMenu = false;

    Coroutine cor_music;
    // Start is called before the first frame update
    void Start()
    {
        GetVolume();
        StartCoroutine(Volumeset());
        if(!MainMenu) ChangeMusic();
    }

    public void GetVolume()
    {
        BGM.volume = PlayerPrefs.GetFloat("volume");
        foreach (AudioSource source in Eff)
        {
            source.volume = PlayerPrefs.GetFloat("eff");
        }
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
        while(true)
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
    public void ChangeMusic()
    {
        if(cor_music != null)
        {
            StopCoroutine(cor_music);
            cor_music = null;
        }
        cor_music =  StartCoroutine(MusicSet());
    }
    IEnumerator MusicSet()
    {
        if (GameManager.Instance.currentNum == 0)
        {
            BGM.clip = Musics[0];
        }
        else
        {
            BGM.clip = Musics[1];
        }
        BGM.Play();
        yield return null;
    }
}
