using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValues : MonoBehaviour {
    public static int maxScore=15;
    public static float bgmVolume=0.5f;
    public static float seVolume=0.8f;
    public static float voiceVolume=0.7f;

    public static bool is1P=true;

    static void GetPrefs()
    {
        ////PlayerPrefs.DeleteAll();
        GlobalValues.maxScore = PlayerPrefs.GetInt("maxScore", GlobalValues.maxScore);
        GlobalValues.bgmVolume = PlayerPrefs.GetFloat("bgmVolume", GlobalValues.bgmVolume);
        GlobalValues.seVolume = PlayerPrefs.GetFloat("seVolume", GlobalValues.seVolume);
        GlobalValues.voiceVolume = PlayerPrefs.GetFloat("voiceVolume", GlobalValues.voiceVolume);
    }

    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        GetPrefs();
    }

    public static void PlaySE(AudioSource source,AudioClip clip)
    {
        source.PlayOneShot(clip, seVolume);
    }
    public static void PlayVoice(AudioSource source, AudioClip clip)
    {
        source.Stop();
        source.PlayOneShot(clip, voiceVolume);
    }

}
