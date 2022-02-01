using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar2;
using Photon.Pun;
using System;

public class GGMetaAvatarEntity : OvrAvatarEntity
{
    [SerializeField] int m_avatarToUseInZipFolder = 2;
    PhotonView m_photonView;
    List<byte[]> m_streamedDataList = new List<byte[]>();
    int m_maxBytesToLog = 15;
    

    private void Start()
    {
        m_photonView = GetComponent<PhotonView>();
        if (m_photonView.IsMine)
        {
            SetIsLocal(true);
            _creationInfo.features = CAPI.ovrAvatar2EntityFeatures.Preset_Default;
            SampleInputManager sampleInputManager = OvrAvatarManager.Instance.gameObject.GetComponent<SampleInputManager>();
            SetBodyTracking(sampleInputManager);
            gameObject.name = "MyAvatar";
        }
        else
        {
            SetIsLocal(false);
            _creationInfo.features = CAPI.ovrAvatar2EntityFeatures.Preset_Remote;
            gameObject.name = "OtherAvatar";
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
            byte[] bytes = RecordStreamData(activeStreamLod);
            m_photonView.RPC("SetStreamData", RpcTarget.Others, bytes);
        }
    }

    [PunRPC]
    public void SetStreamData(byte [] bytes)
    {
        m_streamedDataList.Add(bytes);
        RTDebug.Log("Streamed Data List count = " + m_streamedDataList.Count);
        //LogFirstAndLastBytes();
        //LogBytesContent(bytes);
    }

    void LogBytesContent(byte [] bytes)
    {
        for (int i = 0; i < m_maxBytesToLog; i++)
        {
            string bytesString = Convert.ToString(bytes[i], 2).PadLeft(8, '0');
            Debug.Log($"bytesString " + bytesString);
        }
    }

    void LogFirstAndLastBytes()
    {
        if (m_streamedDataList.Count > 0)
        {
            byte[] firstBytes = m_streamedDataList[0];
            byte[] lastBytes = m_streamedDataList[m_streamedDataList.Count-1];
            string firstBytesString = Convert.ToString(firstBytes[150], 2).PadLeft(8, '0');
            string lastBytesString = Convert.ToString(lastBytes[150], 2).PadLeft(8, '0');
            RTDebug.Log($"First bytes: {firstBytesString} Last Bytes {lastBytesString}");
        }
    }

    private void Update()
    {
        if (m_streamedDataList.Count > 0)
        {
            if (IsLocal == false)
            {
                ApplyStreamData(m_streamedDataList[0]);
                m_streamedDataList.RemoveAt(0);
            }
        }
    }


}
