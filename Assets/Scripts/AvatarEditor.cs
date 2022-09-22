using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;

public class AvatarEditor : MonoBehaviour
{
    [SerializeField] AudioSource m_audioSource;
    [SerializeField] AudioClip m_audioClip;

    void Update()
    {
        bool buttonIsClicked = OVRInput.GetUp(OVRInput.RawButton.A) || Input.GetKeyUp(KeyCode.Space);
        if (buttonIsClicked)
        {
            Debug.Log("Button Clicked");
            PlaySound();
            //Deep link to system editor
            AvatarEditorDeeplink.LaunchAvatarEditor();
        }
        
    }

    void PlaySound()
    {
        m_audioSource.PlayOneShot(m_audioClip);
    }
}
