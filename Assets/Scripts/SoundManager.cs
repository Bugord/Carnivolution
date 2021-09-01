using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    [SerializeField] private List<AudioClip> clips;
    private Dictionary<string, AudioClip> _clipsDictionary;
    
    void Awake()
    {
        Instance = this;
        _clipsDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in clips)
        {
            _clipsDictionary.Add(clip.name, clip);
        }
    }

    public AudioClip GetAudioClip(string clipName)
    {
        return _clipsDictionary[clipName];
    }
}
