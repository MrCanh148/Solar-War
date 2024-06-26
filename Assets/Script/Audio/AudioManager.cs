using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] musicSound, sfxSound;
    public AudioSource musicSource, sfxSource;

    private Sound s;

    private void Awake()
    {
        instance = this;
    }

    public void PlayMusic(string name)
    {
        s = Array.Find(musicSound, x => x.name == name);
        musicSource.clip = s.clip;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        s = Array.Find(sfxSound, x => x.name == name);
        sfxSource.PlayOneShot(s.clip);
    }

    public void StopMusic(string name)
    {
        s = Array.Find(musicSound, x => x.name == name);
        musicSource.clip = s.clip;
        musicSource.Stop();
    }
}