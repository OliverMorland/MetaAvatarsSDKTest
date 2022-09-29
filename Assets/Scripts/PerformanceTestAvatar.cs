using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar2;
using Oculus.Platform;

public class PerformanceTestAvatar : OvrAvatarEntity
{
    [SerializeField] ulong m_userId;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetUserIdFromLoggedInUser());
        StartCoroutine(TryToLoadUserOnceUserIdIsFound());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SetUserIdFromLoggedInUser()
    {
        if (OvrPlatformInit.status == OvrPlatformInitStatus.NotStarted)
        {
            OvrPlatformInit.InitializeOvrPlatform();
        }

        while (OvrPlatformInit.status != OvrPlatformInitStatus.Succeeded)
        {
            if (OvrPlatformInit.status == OvrPlatformInitStatus.Failed)
            {
                Debug.LogError("OVR Platform failed to initialise");
                yield break;
            }
            yield return null;
        }

        Users.GetLoggedInUser().OnComplete(message =>
        {
            if (message.IsError)
            {
                Debug.LogError("Getting Logged in user error " + message.GetError());
            }
            else
            {
                m_userId = message.Data.ID;
                _userId = m_userId;
            }
        });
    }

    IEnumerator TryToLoadUserOnceUserIdIsFound()
    {
        while (_userId == 0)
        {
            Debug.Log("Waiting for User id to be set before connecting to room");
            yield return null;
        }
        OvrAvatarEntitlement.ResendAccessToken();
        StartCoroutine(TryToLoadUser());
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
}
