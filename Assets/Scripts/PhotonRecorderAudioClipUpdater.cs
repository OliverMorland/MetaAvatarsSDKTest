using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonRecorderAudioClipUpdater : MonoBehaviour, AudioClipUpdatable
{
    [SerializeField] MicrophoneVolumeDetector m_detector;

    public void ApplyAudioClip(AudioClip clip)
    {
        Recorder recorder = GetComponent<Recorder>();
        recorder.AudioClip = clip;
        recorder.IsRecording = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_detector.Register(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
