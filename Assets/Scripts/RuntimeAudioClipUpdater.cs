using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeAudioClipUpdater : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (audioSource != null)
        {
            AudioClip clip = audioSource.clip;
            AudioClip referenceClip = GGMicrophone.Instance.GetMicrophoneAudioClip();
            int referenceClipId = referenceClip.GetInstanceID();
            if (clip != null)
            {
                int currentClipId = clip.GetInstanceID();
                if (currentClipId != referenceClipId)
                {
                    Debug.Log("OLILOG Setting clip to reference because it changed");
                    //audioSource.Stop();
                    audioSource.clip = referenceClip;
                    audioSource.Play();
                }
            }
            else
            {
                Debug.Log("OLILOG Setting clip to reference because there was none");
                //audioSource.Stop();
                audioSource.clip = referenceClip;
                audioSource.Play();
            }
        }

        audioSource.timeSamples = GGMicrophone.Instance.GetPosition();
    }
}
