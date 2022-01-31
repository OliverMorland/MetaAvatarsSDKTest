using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar2;
using Photon.Pun;

public class GGMetaAvatarEntity : OvrAvatarEntity
{
    [SerializeField] int m_avatarToUseInZipFolder = 2;
    PhotonView m_photonView;
    byte[] m_streamedData;
    

    private void Start()
    {
        m_photonView = GetComponent<PhotonView>();
        if (m_photonView.IsMine)
        {
            SetIsLocal(true);
            _creationInfo.features = CAPI.ovrAvatar2EntityFeatures.Preset_Default;
            //CreateEntity();
            SampleInputManager sampleInputManager = OvrAvatarManager.Instance.gameObject.GetComponent<SampleInputManager>();
            SetBodyTracking(sampleInputManager);
        }
        else
        {
            SetIsLocal(false);
            _creationInfo.features = CAPI.ovrAvatar2EntityFeatures.Preset_Remote;
            //CreateEntity();
        }
         
        if (IsLocal)
        {
            string[] zipPaths = new string[] { m_avatarToUseInZipFolder + "_rift.glb" };
            LoadAssetsFromZipSource(zipPaths);
        }
        else
        {
            string[] zipPaths = new string[] { m_avatarToUseInZipFolder + 1 + "_rift.glb" };
            LoadAssetsFromZipSource(zipPaths);
        }
    }

    private void LateUpdate()
    {
        if (m_photonView.IsMine)
        {
            byte [] bytes = RecordStreamData(activeStreamLod);
            Debug.Log("Sending streamed data with bytes " + bytes.Length);
            m_photonView.RPC("SetStreamData", RpcTarget.Others, bytes);
        }
    }

    [PunRPC]
    public void SetStreamData(byte [] bytes)
    {
        Debug.Log("Recieving Streamed data with bytes " + bytes.Length);
        m_streamedData = bytes;
    }

    private void Update()
    {
        if (m_photonView.IsMine == false)
        {
            Debug.Log("Applying streamed data");
            ApplyStreamData(m_streamedData);
        }
    }




}
