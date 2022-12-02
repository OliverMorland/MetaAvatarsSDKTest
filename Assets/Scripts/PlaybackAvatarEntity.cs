using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar2;

public class PlaybackAvatarEntity : OvrAvatarEntity
{
    public List<byte[]> m_recordedData;
    public bool m_playback;
    int m_counter;
    float m_startTime;
    float m_interval = 0.1f;

    [SerializeField] string[] m_avatarAssetPathToLoad;

    protected override void Awake()
    {
        //_creationInfo.features = Oculus.Avatar2.CAPI.ovrAvatar2EntityFeatures.Preset_Default;
        //SampleInputManager sampleInputManager = OvrAvatarManager.Instance.gameObject.GetComponent<SampleInputManager>();
        //SetBodyTracking(sampleInputManager);
        //OvrAvatarLipSyncContext lipSyncInput = GameObject.FindObjectOfType<OvrAvatarLipSyncContext>();
        //SetLipSync(lipSyncInput);
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadAssetsFromZipSource(m_avatarAssetPathToLoad);
    }

    void Update()
    {
        if (m_playback)
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
        ApplyStreamData(m_recordedData[m_counter]);
        m_counter++;
    }
}
