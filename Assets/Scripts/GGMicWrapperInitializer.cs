using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGMicWrapperInitializer : MonoBehaviour
{
    private void Awake()
    {
        Recorder recorder = PhotonVoiceNetwork.Instance.PrimaryRecorder;
        recorder.InputFactory = () => new GGMicWrapper();
    }
}
