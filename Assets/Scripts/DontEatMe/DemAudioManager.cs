using UnityEngine;
using System.Collections;

public class DemAudioManager : MonoBehaviour {

  public static AudioSource audioClick;
  public static AudioSource audioSelection;
  public static AudioSource audioUiLoad;
  public static AudioSource audioUiRollOver;
  public static AudioSource audioFail;
  public static AudioSource audioFail2;
  public static AudioSource audioBg;

	// Use this for initialization
  public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol) { 
    AudioSource newAudio = gameObject.AddComponent<AudioSource> ();
    newAudio.clip = clip; 
    newAudio.loop = loop;
    newAudio.playOnAwake = playAwake;
    newAudio.volume = vol; 
    return newAudio; 
  }

  void Awake(){
    // add the necessary AudioSources:
    audioClick = AddAudio(Resources.Load<AudioClip>("Audio/click"), false, false, 0.1f);
    audioSelection = AddAudio(Resources.Load<AudioClip>("Audio/selection") as AudioClip, false, false, 0.1f);
    audioUiLoad = AddAudio(Resources.Load<AudioClip>("Audio/ui_load") as AudioClip, false, false, 0.1f);
    audioUiRollOver = AddAudio(Resources.Load<AudioClip>("Audio/ui_rollover") as AudioClip, false, false, 0.1f);
    audioFail = AddAudio(Resources.Load<AudioClip>("Audio/fail") as AudioClip, false, false, 0.1f);
    audioFail2 = AddAudio(Resources.Load<AudioClip>("Audio/fail2") as AudioClip, false, false, 0.1f);
    audioBg = AddAudio(Resources.Load<AudioClip>("Audio/dem_loop") as AudioClip, true, false, 0.1f);
   
  } 

}
