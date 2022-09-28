using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnAudioSourceInput : MonoBehaviour
{
    [SerializeField] AudioSource m_audioSource;
    [SerializeField] Vector3 m_minScale = Vector3.one;
    [SerializeField] Vector3 m_maxScale = new Vector3(3, 3, 3);
    [SerializeField] float m_loudnessSensitivity = 20f;
    [SerializeField] float m_threshold = 0.1f;
    int m_sampleWindow = 64;

    private void Update()
    {
        float volume;
        if (m_audioSource.clip == null)
        {
            volume = 0;
        }
        else
        {
            volume = GetVolumeFromAudioClip(m_audioSource.timeSamples, m_audioSource.clip) * m_loudnessSensitivity;
        }
        if (volume < m_threshold)
        {
            volume = 0;
        }

        transform.localScale = Vector3.Lerp(m_minScale, m_maxScale, volume);
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
}
