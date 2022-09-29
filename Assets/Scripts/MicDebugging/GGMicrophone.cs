using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class GGMicrophone : MonoBehaviour
{
    [SerializeField] AudioClip m_audioClip;
    [Range(0f, 1f)] [SerializeField] float m_micInputVolume = 0f;
    [Range(0, 1000000)] [SerializeField] int m_microphonePosition;
    [SerializeField] string m_currentMicrophoneDevice;
    [SerializeField] string[] m_microphoneDevices;
    [SerializeField] int m_audioClipId;
    [SerializeField] float m_microphoneSensitivity = 50f;
    [SerializeField] float m_threshold = 0.1f;
    int m_sampleWindow = 16;
    AudioSource m_lipSyncAudioSource;

    static private GGMicrophone m_instance;
    static public GGMicrophone Instance
    {
        get 
        { 
            return m_instance; 
        }
    }

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [ContextMenu("Start Microphone")]
    public void StartMicrophone()
    {
        string microphoneName = GetMicrophoneDeviceName();
        m_audioClip = Microphone.Start(microphoneName, true, 1, 48000);
        string clipId = m_audioClip.GetInstanceID().ToString();
        m_audioClip.name = "GGMicAudioClip_" + clipId;
    }

    [ContextMenu("Stop Microphone")]
    public void StopMicrophone()
    {
        string microphoneName = GetMicrophoneDeviceName();
        Microphone.End(microphoneName);
    }

    public bool IsRecording()
    {
        string microphoneName = GetMicrophoneDeviceName();
        return Microphone.IsRecording(microphoneName);
    }

    public int GetPosition()
    {
        string microphoneName = GetMicrophoneDeviceName();
        return Microphone.GetPosition(microphoneName);
    }

    string GetMicrophoneDeviceName()
    {
        return Microphone.devices[0];
    }

    public AudioClip GetMicrophoneAudioClip()
    {
        return m_audioClip;
    }

    private void Start()
    {
        StartMicrophone();
    }

    private void Update()
    {
        if (m_audioClip != null)
        {
            float micInputVolume = GetVolumeFromMicrophone() * m_microphoneSensitivity;
            if (micInputVolume < m_threshold)
            {
                micInputVolume = 0;
            }
            m_micInputVolume = micInputVolume;
            m_audioClipId = m_audioClip.GetInstanceID();
        }

        m_microphonePosition = GetPosition();
    }

    float GetVolumeFromMicrophone()
    {
        return GetVolumeFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), m_audioClip);
    }

    float GetVolumeFromAudioClip(int clipPosition, AudioClip audioClip)
    {
        if (clipPosition - m_sampleWindow < 0)
        {
            return 0;
        }

        float[] data = new float[m_sampleWindow];
        audioClip.GetData(data, clipPosition - m_sampleWindow);

        float totalVolume = 0;
        for (int i = 0; i < data.Length; i++)
        {
            totalVolume += Mathf.Abs(data[i]);
        }

        float averageVolume = totalVolume / m_sampleWindow;
        return averageVolume;
    }

    void ShowMicrophoneDevices()
    {
        m_currentMicrophoneDevice = GetMicrophoneDeviceName();
        m_microphoneDevices = Microphone.devices;
    }

    [SerializeField] AudioClip m_photonAudioClip;
    public void SetPhotonAudioClipName(AudioClip clip)
    {
        m_photonAudioClip = clip;
    }

    [SerializeField] string m_photonMicName;
    public void SetPhotonMicName(string photonMicName)
    {
        m_photonMicName = photonMicName;
    }

    [Range(0, 1000000)][SerializeField] int m_photonMicPosition;
    public void SetPhotonMicPosition(int photonMicPosition)
    {
        m_photonMicPosition = photonMicPosition;
    }

    [SerializeField] AudioClip m_LipSyncAudioClip;
    public void SetLipSyncAudioClip(AudioClip clip)
    {
        m_LipSyncAudioClip = clip;
    }

    [SerializeField] string m_LipSyncMicName;
    public void SetLipSyncAudioSourceDeviceName(string deviceName)
    {
        m_LipSyncMicName = deviceName;
    }

    [Range(0, 1000000)][SerializeField] int m_lipSyncAudioSourcePosition;
    public void SetLipSyncAudioSourcePosition(int photonMicPosition)
    {
        m_lipSyncAudioSourcePosition = photonMicPosition;
    }
}
