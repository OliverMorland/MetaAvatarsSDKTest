using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LipSyncDebugger : MonoBehaviour
{
    private void Update()
    {
        OnAButtonPressed();
        OnBButtonPressed();
        OnXButtonPressed();
        OnYButtonPressed();
    }

    void OnAButtonPressed()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            StartLipSyncMic();
        }
    }

    void StartLipSyncMic()
    {
        Debug.Log("Start Lip Sync");
        LipSyncMicInput lipSyncMic = FindObjectOfType<LipSyncMicInput>();
        lipSyncMic.StartMicrophone_Internal();
    }

    void OnBButtonPressed()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            StopLipSyncMic();
        }
    }

    void StopLipSyncMic()
    {
        Debug.Log("Stop Lip Sync");
        LipSyncMicInput lipSyncMic = FindObjectOfType<LipSyncMicInput>();
        lipSyncMic.StopMicrophone_Internal();
    }

    void OnXButtonPressed()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            StopRecorder();
        }
    }

    void PrintRecorderAudioClipName()
    {
        Recorder recorder = FindObjectOfType<Recorder>();
        AudioClip audioClip = recorder.AudioClip;
        if (audioClip == null)
        {
            Debug.Log("Recorder has no AudioClip");
        }
        else
        {
            Debug.Log("Audio Clip Name: " + audioClip.name);
        }
    }

    void SetLipSyncAudioClipToEqualRecorders()
    {
        Recorder recorder = FindObjectOfType<Recorder>();
        AudioClip audioClip = recorder.AudioClip;
        LipSyncMicInput lipSyncMicInput = FindObjectOfType<LipSyncMicInput>();
        lipSyncMicInput.audioSource.clip = audioClip;
    }

    void SetLipSyncAudioCLipToEqualSpeakers()
    {
        Debug.Log("OLILOG Setting Lip Sync Audio Clip");
        Speaker speaker = FindObjectOfType<Speaker>();
        AudioSource audioSource = speaker.GetComponent<AudioSource>();
        LipSyncMicInput lipSyncMicInput = FindObjectOfType<LipSyncMicInput>();
        lipSyncMicInput.audioSource.Stop();
        lipSyncMicInput.audioSource.clip = audioSource.clip;
        lipSyncMicInput.audioSource.Play();
    }

    void OnYButtonPressed()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.Y))
        {
            StartRecorder();
        }
    }

    void StartRecorder()
    {
        Debug.Log("Start Recorder");
        Recorder recorder = FindObjectOfType<Recorder>();
        recorder.StartRecording();
    }

    void StopRecorder()
    {
        Debug.Log("Stop Recorder");
        Recorder recorder = FindObjectOfType<Recorder>();
        recorder.StopRecording();
    }
}
