using System.Collections;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;
    public AudioClip[] GameSounds;
    private int totalSounds;
    private ArrayList soundObjectList;
    private SoundObject tempSoundObj;
    public float volume = 1;
    
    public void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        // we will grab the volume from PlayerPrefs when this script first starts
        if (PlayerPrefs.HasKey("SFXVol"))
        {
            volume = PlayerPrefs.GetFloat("SFXVol");         
        }
        else
        {
            volume = 1;
        }
    
        soundObjectList = new ArrayList();

        // make sound objects for all of the sounds in GameSounds array
        foreach (AudioClip theSound in GameSounds)
        {
            tempSoundObj = new SoundObject(theSound, theSound.name, volume);
            soundObjectList.Add(tempSoundObj);
            totalSounds++;
        }
    }
    public void PlaySoundByIndex(int anIndexNumber, Vector3 aPosition)
    {
        // make sure we're not trying to play a sound indexed higher than exists in the array
        if (anIndexNumber > soundObjectList.Count)
        {
            Debug.LogWarning("BaseSoundController>Trying to do PlaySoundByIndex with invalid index number.Playing last sound in array, instead.");
            anIndexNumber = soundObjectList.Count - 1;
        }
        
        tempSoundObj = (SoundObject)soundObjectList[anIndexNumber];
        tempSoundObj.PlaySound(aPosition);
    }
}

public class SoundObject
{
    public AudioSource source;
    public GameObject sourceGO;
    public AudioClip clip;
    public string name;

    public SoundObject(AudioClip aClip, string aName, float aVolume)
    {
        // in this (the constructor) we create a new audio source 
        // and store the details of the sound itself
        sourceGO = new GameObject("AudioSource_" + aName);
        source = sourceGO.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.clip = aClip;
        source.volume = aVolume;
        clip = aClip;
        name = aName;
    }
    public void PlaySound(Vector3 atPosition)
    {
        sourceGO.transform.position = atPosition;
        source.PlayOneShot(clip);
    }
}
