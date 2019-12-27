using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool loopCurrentSong;
    public bool musicPlayerPaused;
    public bool musicPlayerStopped;
    [HideInInspector]public float masterVolume;
    [HideInInspector]public float soundEffectVolume;
    [HideInInspector]public float musicVolume;
    public AudioMixer audioMixer;
    public GameController gameController;
    public Sound[] soundEffects;
    public Sound[] songs;
    public Sound currentSong;
    public String[] mixerGroups;

    private void Awake()
    {
        UnpackAudioArray(soundEffects);
        UnpackAudioArray(songs);
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    private void Start()
    {
        CheckInitializeAudio();
        currentSong = null;
        //RunMusicPlayer(true);
        //PlayNextSong();
    }
    void CheckInitializeAudio()
    {
        if (!gameController.initiateAudio)
        {
            foreach (string mixerGroup in mixerGroups)
            {
                ChangeVolume(1, mixerGroup);
            }
        }
    }
    private void Update()
    {
        if (!musicPlayerStopped)
        {
            RunMusicPlayer(true);
        }
        
    }
    void UnpackAudioArray(Sound[] soundArray)
    {
        foreach (Sound sound in soundArray)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.outputAudioMixerGroup = sound.mixerGroup;
        }
    }
    public void PlaySoundEffect(string soundEffectName)
    {
        Sound thisSound = Array.Find(soundEffects, soundEffect => soundEffect.name == soundEffectName);
        if (thisSound == null)
        {
            Debug.Log("sound: " + soundEffectName + " not found");
            return;
        }
        thisSound.source.Play();
    }
    public void LoopSoundEffect (string soundEffectName)
    {
        Sound thisSound = Array.Find(soundEffects, soundEffect => soundEffect.name == soundEffectName);
        if (thisSound == null)
        {
            Debug.Log("sound: " + soundEffectName + " not found");
            return;
        }
        thisSound.source.loop = true;
        if (!thisSound.source.isPlaying)
        {
            thisSound.source.Play();
        }
    }
    public void PauseSoundEffect(string soundEffectName)
    {
        Sound thisSound = Array.Find(soundEffects, soundEffect => soundEffect.name == soundEffectName);
        if (thisSound == null)
        {
            Debug.Log("sound: " + soundEffectName + " not found");
            return;
        }
        if (thisSound.source.isPlaying)
        {
            thisSound.source.Pause();
        }
    }
    public void StopSoundEffect(string soundEffectName)
    {
        Sound thisSound = Array.Find(soundEffects, soundEffect => soundEffect.name == soundEffectName);
        if (thisSound == null)
        {
            Debug.Log("sound: " + soundEffectName + " not found");
            return;
        }
        thisSound.source.loop = false;
        thisSound.source.Stop();
    }
    public void PlaySong(string songName)
    {
        Sound thisSound = Array.Find(songs, song => song.name == songName);
        if (thisSound == null)
        {
            Debug.Log("sound: " + songName + " not found");
            return;
        }
        thisSound.source.Play();
    }
    int GetSongIndex()
    {
        if (currentSong != null)
        {
            int currentSongIndex = Array.IndexOf(songs, currentSong);
            return currentSongIndex;
        }
        return 0;
    }
    void PlayNextSong()
    {
        if (currentSong == null)
        {
            currentSong = songs[0];
            PlaySong(currentSong.name);
            return;
        }
        if (loopCurrentSong)
        {
            PlaySongByRelativeIndex(0);
            return;
        }
        PlaySongByRelativeIndex(1);
    }
    public void PlaySongByRelativeIndex(int indexRelation)
    {
        if (currentSong.source.isPlaying)
        {
            currentSong.source.Stop();
        }
        int songIndex = GetSongIndex();
        if (songIndex + indexRelation < songs.Length && songIndex + indexRelation >= 0 )
        {
            currentSong = songs[songIndex + indexRelation];
        }
        else
        {
            if (songIndex+indexRelation < 0 )
            {
                currentSong = songs[songs.Length - 1];
            }
            if (songIndex + indexRelation >= songs.Length )
            {
                currentSong = songs[0];
            }
            
        }
        PlaySong(currentSong.name);
    }
    void RunMusicPlayer(bool isRunning)
    {
        if (isRunning)
        {
            if (currentSong == null)
            {
                PlayNextSong();
                return;
            }
            else if (currentSong != null && !musicPlayerPaused)
            {
                if (currentSong.source != null)
                {
                    if (!currentSong.source.isPlaying)
                    {
                        PlayNextSong();
                        return;
                    }
                }
            }
        }
    }
    public void PauseMusicPlayer()
    {
        if (!musicPlayerPaused)
        {
            musicPlayerPaused = true;
            if (currentSong != null)
            {
                currentSong.source.Pause();
            }
        }
    }
    public void ResumeMusicPlayer()
    {
        if (musicPlayerPaused)
        {
            musicPlayerPaused = false;
            if (currentSong != null)
            {
                currentSong.source.UnPause();
            }
        }
    }
    public void ChangeVolume(float volume, string mixerGroup)
    {
        //float newDecibel = (-10 + volume) * 8;
        float newDecibel = Mathf.Clamp((volume / 10), .0001f, 10);
        newDecibel = Mathf.Log10(newDecibel) * 20; // this scales the volume correctly
        audioMixer.SetFloat(mixerGroup, newDecibel);
        gameController.StoreOptionsValue(mixerGroup, volume);
    }
}
