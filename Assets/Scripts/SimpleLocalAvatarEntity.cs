using Oculus.Avatar2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLocalAvatarEntity : OvrAvatarEntity
{
    [SerializeField] string[] m_avatarAssetPathToLoad;

    public bool m_startRecording = false;

    int m_counter;
    float m_startTime;
    float m_interval = 0.1f;
    PlaybackAvatarEntity m_playbackAvatarEntity;

    protected override void Awake()
    {
        _creationInfo.features = Oculus.Avatar2.CAPI.ovrAvatar2EntityFeatures.Preset_Default;
        SampleInputManager sampleInputManager = OvrAvatarManager.Instance.gameObject.GetComponent<SampleInputManager>();
        SetBodyTracking(sampleInputManager);
        OvrAvatarLipSyncContext lipSyncInput = GameObject.FindObjectOfType<OvrAvatarLipSyncContext>();
        SetLipSync(lipSyncInput);
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_playbackAvatarEntity = GameObject.FindObjectOfType<PlaybackAvatarEntity>();
        LoadAssetsFromZipSource(m_avatarAssetPathToLoad);
    }

    void Update()
    {
        if (m_startRecording)
        {
            float elapsedTime = Time.time - m_startTime;
            if (elapsedTime > m_interval)
            {
                PlaybackRecordedData();
                m_startTime = Time.time;
            }
        }
    }

    void PlaybackRecordedData()
    {
        byte[] data = RecordStreamData(activeStreamLod);
        m_playbackAvatarEntity.m_recordedData.Add(data);
        m_counter++;
    }
}
