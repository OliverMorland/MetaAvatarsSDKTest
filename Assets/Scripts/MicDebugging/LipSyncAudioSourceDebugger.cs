using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LipSyncAudioSourceDebugger : MonoBehaviour
{
    AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        int position = m_audioSource.timeSamples;
        GGMicrophone.Instance.SetLipSyncAudioSourcePosition(position);
        GGMicrophone.Instance.SetLipSyncAudioClip(m_audioSource.clip);
    }
}
