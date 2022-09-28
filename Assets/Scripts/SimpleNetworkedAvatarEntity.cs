using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar2;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System;

public class SimpleNetworkedAvatarEntity : OvrAvatarEntity, IPunObservable
{
    [SerializeField] ulong m_instantiationData;
    PhotonView m_photonView;
    [SerializeField] float m_maxExpectedLatency = 0.03f;

    protected override void Awake()
    {
        ConfigureAvatarEntity();
        base.Awake();
    }

    void Start()
    {
        m_instantiationData = GetUserIdFromPhotonInstantiationData();
        _userId = m_instantiationData;
        StartCoroutine(TryToLoadUser());
    }

    private void Update()
    {
        if (m_photonView.IsMine == false)
        {
            float delay = (1f / PhotonNetwork.SerializationRate) + m_maxExpectedLatency;
            SetPlaybackTimeDelay(delay);
        }
    }

    void ConfigureAvatarEntity()
    {
        m_photonView = GetComponent<PhotonView>();
        if (m_photonView.IsMine)
        {
            SetIsLocal(true);
            _creationInfo.features = Oculus.Avatar2.CAPI.ovrAvatar2EntityFeatures.Preset_Default;
            SampleInputManager sampleInputManager = OvrAvatarManager.Instance.gameObject.GetComponent<SampleInputManager>();
            SetBodyTracking(sampleInputManager);
            OvrAvatarLipSyncContext lipSyncInputContext = GameObject.FindObjectOfType<OvrAvatarLipSyncContext>();
            SetLipSync(lipSyncInputContext);
            gameObject.name = "MyAvatar";
        }
        else
        {
            SetIsLocal(false);
            _creationInfo.features = Oculus.Avatar2.CAPI.ovrAvatar2EntityFeatures.Preset_Remote;
            gameObject.name = "OtherAvatar";
        }
    }

    ulong GetUserIdFromPhotonInstantiationData()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        object[] instantiationData = photonView.InstantiationData;
        Int64 data_as_int = (Int64)instantiationData[0];
        return Convert.ToUInt64(data_as_int);
    }

    IEnumerator TryToLoadUser()
    {
        var hasAvatarRequest = OvrAvatarManager.Instance.UserHasAvatarAsync(_userId);
        while (hasAvatarRequest.IsCompleted == false)
        {
            yield return null;
        }
        LoadUser();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            byte[] bytes = RecordStreamData(activeStreamLod);
            stream.SendNext(bytes);
        }
        else if (stream.IsReading)
        {
            byte[] bytes = (byte[])stream.ReceiveNext();
            ApplyStreamData(bytes);
        }
    }

}
