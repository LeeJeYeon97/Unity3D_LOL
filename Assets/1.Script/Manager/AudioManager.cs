using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public class AudioManager 
{
    GameObject obj;
    public void Create()
    {
        obj = GameObject.Find("@Managers");
        obj.AddComponent<AudioSource>();
        PlayBGM("StartSound");
    }
    public void PlayBGM(string clipName, AudioSource source = null)
    {
        if (source == null)
        {
            source = obj.GetComponent<AudioSource>();
        }
        AudioClip clip = Resources.Load("Sounds/" + clipName) as AudioClip;
        source.Stop();
        source.PlayOneShot(clip);
        source.loop = true;
    }
    public void PlayClip(string clipName, AudioSource source = null, float volume = 1.0f)
    {
        if(source == null)
        {
            source = obj.GetComponent<AudioSource>();
        }
        AudioClip clip = Resources.Load("Sounds/" + clipName) as AudioClip;
        source.PlayOneShot(clip,volume);
    }
    public void PlayClip(AudioClip clip, AudioSource source = null)
    {
        if (source == null)
        {
            source = obj.GetComponent<AudioSource>();
        }
        source.PlayOneShot(clip);
    }
}
