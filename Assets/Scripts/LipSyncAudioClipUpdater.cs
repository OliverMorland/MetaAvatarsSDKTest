using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LipSyncAudioClipUpdater : MonoBehaviour, AudioClipUpdatable
{
    [SerializeField] MicrophoneVolumeDetector m_detector;

    public void ApplyAudioClip(AudioClip clip)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
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
