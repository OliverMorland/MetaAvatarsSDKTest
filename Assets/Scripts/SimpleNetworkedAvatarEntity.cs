using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar2;
using Photon.Pun;

public class SimpleNetworkedAvatarEntity : OvrAvatarEntity, IPunObservable
{
    [SerializeField] string [] m_avatarAssetPathToLoad;
    PhotonView m_photonView;
    List<byte[]> m_streamedDataList = new List<byte[]>();
    float m_cycleStartTime = 0;
    float m_intervalToSendData = 0.08f;

    protected override void Awake()
    {
        ConfigureAvatarEntity();
        base.Awake();
    }

    void Start()
    {
        LoadAssetsFromZipSource(m_avatarAssetPathToLoad);
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
            OvrAvatarLipSyncContext lipSyncInput = GameObject.FindObjectOfType<OvrAvatarLipSyncContext>();
            SetLipSync(lipSyncInput);
            gameObject.name = "MyAvatar";
        }
        else
        {
            SetIsLocal(false);
            _creationInfo.features = Oculus.Avatar2.CAPI.ovrAvatar2EntityFeatures.Preset_Remote;
            gameObject.name = "OtherAvatar";
        }
    }

    //private void LateUpdate()
    //{
    //    if (PhotonNetwork.IsConnected)
    //    {
    //        float elapsedTime = Time.time - m_cycleStartTime;
    //        if (elapsedTime > m_intervalToSendData)
    //        {
    //            RecordAndSendStreamDataIfMine();
    //            m_cycleStartTime = Time.time;
    //        }
    //    }
    //}

    //void RecordAndSendStreamDataIfMine()
    //{
    //    if (m_photonView.IsMine)
    //    {
    //        byte[] bytes = RecordStreamData(activeStreamLod);
    //        m_photonView.RPC("RecieveStreamData", RpcTarget.Others, bytes);
    //    }
    //}

    //[PunRPC]
    //public void RecieveStreamData(byte[] bytes)
    //{
    //    m_streamedDataList.Add(bytes);
    //}

    private void Update()
    {
        RestartMicWhenControllerButtonIsClicked();
        //if (m_streamedDataList.Count > 0)
        //{
        //    if (IsLocal == false)
        //    {
        //        byte[] firstBytesInList = m_streamedDataList[0];
        //        if (firstBytesInList != null)
        //        {
        //            ApplyStreamData(firstBytesInList);
        //        }
        //        m_streamedDataList.RemoveAt(0);
        //    }
        //}
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            byte[] bytes = RecordStreamData(activeStreamLod);
            Debug.Log("OLILOG About to send bytes stream...");
            stream.SendNext(bytes);
        }
        else if (stream.IsReading)
        {
            byte[] bytes = (byte[])stream.ReceiveNext();
            ApplyStreamData(bytes);
        }
    }


    void RestartMicWhenControllerButtonIsClicked()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            LipSyncMicInput lipSyncInput = GameObject.FindObjectOfType<LipSyncMicInput>();
            lipSyncInput.StopMicrophone();
            lipSyncInput.StartMicrophone();
        }
    }
}
