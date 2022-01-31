using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar2;
using Oculus.Platform;

public class TestingLoadingMetaAvatars : MonoBehaviour
{

    [ContextMenu("Load Avatar")]
    void LoadAvatar()
    {
        Users.GetAccessToken().OnComplete(HandleAccessToken);
    }

    void HandleAccessToken(Message<string> msg)
    {
        Debug.Log("We got a access token");
    }


}
