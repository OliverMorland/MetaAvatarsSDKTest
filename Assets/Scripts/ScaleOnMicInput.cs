using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;

public class ScaleOnMicInput : MonoBehaviour
{
    [SerializeField] MicrophoneVolumeDetector m_microphoneVolumeDetector;
    [SerializeField] Vector3 m_minScale = Vector3.one;
    [SerializeField] Vector3 m_maxScale = new Vector3(3, 3, 3);
    [SerializeField] float m_loudnessSensitivity = 100f;
    [SerializeField] float m_threshold = 0.1f;

    private void Update()
    {
        float volume = m_microphoneVolumeDetector.GetVolumeFromMicrophone() * m_loudnessSensitivity;
        if (volume < m_threshold)
        {
            volume = 0;
        }

        transform.localScale = Vector3.Lerp(m_minScale, m_maxScale, volume);
    }
}
