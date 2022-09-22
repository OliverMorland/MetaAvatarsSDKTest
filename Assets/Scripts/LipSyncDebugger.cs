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
        lipSyncMic.StartMicrophone();
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
        lipSyncMic.StartMicrophone();
    }

    void OnXButtonPressed()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
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

    void OnYButtonPressed()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            StopRecorder();
        }
    }

    void StopRecorder()
    {
        Debug.Log("Stop Recorder");
        Recorder recorder = FindObjectOfType<Recorder>();
        recorder.StopRecording();
    }
}
