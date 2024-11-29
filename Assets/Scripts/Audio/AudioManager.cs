using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    [Header("音源")]
    public AudioSource BGMSource;
    public AudioSource FXSource;
    public AudioMixer MasterVolume;
    
    [Header("事件监听")]
    public PlayAudioSO FXEvent;//监听
    public PlayAudioSO BGMEvent;
    public FloatSO adjustVolumeEvent;
    public VoidSO PauseEvent;

    [Header("事件广播")]
    public FloatSO SyncVolumeEvent;

    private void OnEnable() 
    {
        FXEvent.OnEventRaised += OnFXEvent;
        BGMEvent.OnEventRaised += OnBGMEvent;
        adjustVolumeEvent.OnEventRaised += OnAdjustVolume;
        PauseEvent.OnEventRaised += OnPause;
    }

    private void OnPause()
    {
        MasterVolume.GetFloat("MasterVolume", out float volume); // 获取音量值
        SyncVolumeEvent.RaiseEvent(volume);
    }

    private void OnAdjustVolume(float volume)
    {
        MasterVolume.SetFloat("MasterVolume",volume * 100 - 80);
    }

    private void OnBGMEvent(AudioClip audioClip)
    {
        BGMSource.clip = audioClip;
        BGMSource.Play();
    }

    private void OnFXEvent(AudioClip audioClip)
    {
        FXSource.clip = audioClip;
        FXSource.Play();
    }

    private void OnDisable() 
    {
        FXEvent.OnEventRaised -= OnFXEvent;
        BGMEvent.OnEventRaised -= OnBGMEvent;
        adjustVolumeEvent.OnEventRaised -= OnAdjustVolume;
        PauseEvent.OnEventRaised -= OnPause;
    }
}
