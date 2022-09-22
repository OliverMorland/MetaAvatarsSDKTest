using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar2;
using Photon.Pun;

public class SimpleNetworkedAvatarEntity : OvrAvatarEntity, IPunObservable
{
    [SerializeField] string [] m_avatarAssetPathToLoad;
    PhotonView m_photonView;

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
