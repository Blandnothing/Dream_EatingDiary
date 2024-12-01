using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : SingletonMono<MusicManager>
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip defaultMusic;
    private Dictionary<string, AudioClip> audioClipDictionary = new Dictionary<string, AudioClip>();

    protected override void Awake()
    {
        base.Awake();
        foreach (var clip in audioClips)
        {
            audioClipDictionary.Add(clip.name, clip);
        }
        audioSource.clip = defaultMusic;
        audioSource.Play();
    }

    public void PlayMusic(string clipName)
    {
        audioSource.clip = audioClipDictionary[clipName];
        audioSource.Play();
    }
    public void PlaySound(string clipName)
    {
        audioSource.PlayOneShot(audioClipDictionary[clipName]);
    }
}
