using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using ReadOnlyAttribute = Unity.Collections.ReadOnlyAttribute;

public class MicrophoneVolumeDetector : MonoBehaviour
{
    [ReadOnly] [SerializeField] AudioClip m_audioClip;
    [SerializeField] int m_audioClipId;
    int m_sampleWindow = 64;
    public List<AudioClipUpdatable> m_audioClipUpdatablesList = new List<AudioClipUpdatable>();

    private void Start()
    {
       m_audioClip = GetMicrophoneAudioClip();
    }

    private void Update()
    {
        if (m_audioClip != null)
        {
            m_audioClipId = m_audioClip.GetInstanceID();
        }
    }

    [ContextMenu("Get Microphone Audio Clip")]
    void SetAudioClipFromMicrophone()
    {
        m_audioClip = GetMicrophoneAudioClip();
        foreach (AudioClipUpdatable audioClipUpdatable in m_audioClipUpdatablesList)
        {
            audioClipUpdatable.ApplyAudioClip(m_audioClip);
        }
    }

    AudioClip GetMicrophoneAudioClip()
    {
        string microphoneName = Microphone.devices[0];
        return Microphone.Start(microphoneName, true, 10, AudioSettings.outputSampleRate);
    }

    [ContextMenu("Stop Microphone")]
    void StopMicrophone()
    {
        string microphoneName = Microphone.devices[0];
        Microphone.End(microphoneName);
    }

    public float GetVolumeFromMicrophone()
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
            totalVolume += data[i];
        }

        float averageVolume = totalVolume / m_sampleWindow;
        return averageVolume;
    }

    public void Register(AudioClipUpdatable audioClipUpdatable)
    {
        m_audioClipUpdatablesList.Add(audioClipUpdatable);
    }

    public AudioClip GetCurrentAudioClip()
    {
        return m_audioClip;
    }
}
