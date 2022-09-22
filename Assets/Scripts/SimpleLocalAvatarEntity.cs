using Oculus.Avatar2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLocalAvatarEntity : OvrAvatarEntity
{
    [SerializeField] string[] m_avatarAssetPathToLoad;

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
        LoadAssetsFromZipSource(m_avatarAssetPathToLoad);
    }
}
