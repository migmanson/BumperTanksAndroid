using UnityEngine;
using System.Collections;
public class MusicController : MonoBehaviour
{
    private float volume;
    public string gamePrefsName = "BumperTanks";
    public AudioClip[] music;
    public bool loopMusic;
    private AudioSource source;
    private GameObject sourceGO;
    private int fadeState;
    private int targetFadeState;
    private float volumeON;
    private float targetVolume;
    public float fadeTime = 5f;
    public bool shouldFadeInAtStart = true;
    public static MusicController Instance;
    public bool playThisOnAwake = false;
    public int initialIndexClip;
    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }


    void Start()
    {
        // we will grab the volume from PlayerPrefs when this script 
        // first starts
        if (PlayerPrefs.HasKey("_MusicVol"))
        {
            volumeON = PlayerPrefs.GetFloat("_MusicVol");
        }
        else
        {
            PlayerPrefs.SetFloat("_MusicVol", 1.0f);
            PlayerPrefs.Save();
            volumeON = PlayerPrefs.GetFloat("_MusicVol");
        }
        // create a game object and add an AudioSource to it, to 
        // play music on
        sourceGO = new GameObject("Music_AudioSource");
        source = sourceGO.AddComponent<AudioSource>();
        source.name = "MusicAudioSource";
        source.playOnAwake = playThisOnAwake;
        source.clip = music[initialIndexClip];
        source.volume = volume;
        // the script will automatically fade in if this is set
        if (shouldFadeInAtStart)
        {
            fadeState = 0;
            volume = 0;
        }
        else
        {
            fadeState = 1;
            volume = volumeON;
        }
        // set up default values
        targetFadeState = 0;
        targetVolume = volumeON;
        source.volume = volume;
    }

    void Update()
    {
        // if the audiosource is not playing and it's supposed to loop, play it again
        if (!source.isPlaying && loopMusic)
            source.Play();
        // deal with volume fade in/out
        if (fadeState != targetFadeState)
        {
            if (targetFadeState > 0.99f)
            {
                if (volume == volumeON)
                    fadeState = 1;
            }
            else
            {
                if (volume < 0.01f)
                {
                    fadeState = 0;
                    source.Stop();
                }
            }
            volume = Mathf.Lerp(volume, targetVolume, Time.deltaTime * fadeTime);
            source.volume = volume;
        }
    }

    public void FadeIn(float fadeAmount)
    {
        //Debug.LogError("Fade In");
        volume = 0;
        fadeState = 0;
        targetFadeState = 1;
        targetVolume = volumeON;
        fadeTime = fadeAmount;
    }

    public void FadeOut(float fadeAmount)
    {
        //Debug.LogError("Fade Out");
        volume = volumeON;
        fadeState = 1;
        targetFadeState = 0;
        targetVolume = 0;
        fadeTime = fadeAmount;
    }

    public void PauseMusic(float fadeAmount)
    {
        if (source.isPlaying)
        {
            //Debug.LogError("Stop music");        
            loopMusic = false;
            source.Pause();
        }
    }

    public void ResumePlayMusic(float fadeAmount)
    {
        if (!source.isPlaying)
        {
            //Debug.LogError("Play music");
            loopMusic = true;
            source.Play();
            FadeIn(5);
        }
        else
        {
            source.Play();
        }
    }

    public void SwitchClip(int newClip, float waitAmount = 0, float fadeAmount = 5)
    {
        StartCoroutine(CambioDeMusica(newClip, waitAmount, fadeAmount));
    }

    IEnumerator CambioDeMusica(int newClip, float waitAmount, float fadeAmount)
    {
        FadeOut(fadeAmount);
        yield return new WaitForSeconds(0.25f);
        yield return new WaitForSeconds(waitAmount);
        source.clip = music[newClip];
        FadeIn(50);
    }
}