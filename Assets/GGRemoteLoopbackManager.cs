using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar2;


public class GGRemoteLoopbackManager : MonoBehaviour
{
    [SerializeField] GGMetaAvatarEntity m_localAvatar;
    [SerializeField] GGMetaAvatarEntity m_remoteAvatar;
    byte[] m_recordedBytes;

    private void LateUpdate()
    {
        if (m_localAvatar != null)
        {
            Debug.Log("Active Stream LOD " + m_localAvatar.activeStreamLod.ToString());
            m_recordedBytes = m_localAvatar.RecordStreamData(m_localAvatar.activeStreamLod);
            Debug.Log("Bytes length: " + m_recordedBytes.Length);
        }
    }

    void Update()
    {
        if (m_remoteAvatar != null)
        {
            Debug.Log("Applying Stream Data");
            m_remoteAvatar.ApplyStreamData(m_recordedBytes);
        }
    }
}
