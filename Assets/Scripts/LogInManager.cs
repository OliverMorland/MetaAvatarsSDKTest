using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Oculus.Avatar2;
using Oculus.Platform;
using System;

public class LogInManager : MonoBehaviourPunCallbacks
{
    const string ROOM_NAME = "Meta Avatar Sdk Test Room";
    const float SPAWN_POS_Z = 1f;
    const string AVATAR_PREFAB_NAME = "SimpleNetworkedAvatar";

    [SerializeField] Text m_screenText;
    [SerializeField] Camera m_camera;
    [SerializeField] float m_minSpawnPos_x = -5f;
    [SerializeField] float m_maxSpawnPos_x = 5f;
    [SerializeField] float m_minSpawnPos_z = -5f;
    [SerializeField] float m_maxSpawnPos_z = 5f;
    [SerializeField] ulong m_userId;

    //Singleton implementation
    private static LogInManager m_instance;
    public static LogInManager Instance
    {
        get
        {
            return m_instance;
        }
    }
    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(SetUserIdFromLoggedInUser());
        StartCoroutine(ConnectToPhotonRoomOnceUserIdIsFound());
        StartCoroutine(InstantiateNetworkedAvatarOnceInRoom());
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
                m_screenText.text = "OVR Platform failed to initialise";
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
            }
        });
    }

    IEnumerator ConnectToPhotonRoomOnceUserIdIsFound()
    {
        while (m_userId == 0)
        {
            Debug.Log("Waiting for User id to be set before connecting to room");  
            yield return null;
        }
        ConnectToPhotonRoom();
    }

    void ConnectToPhotonRoom()
    {
        PhotonNetwork.ConnectUsingSettings();
        m_screenText.text = "Connecting to Server";
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        m_screenText.text = "Connecting to Lobby";
    }

    public override void OnJoinedLobby()
    {
        m_screenText.text = "Creating Room";
        PhotonNetwork.JoinOrCreateRoom(ROOM_NAME, null, null);
    }

    public override void OnJoinedRoom()
    {
        string roomName = PhotonNetwork.CurrentRoom.Name;
        m_screenText.text = "Joined room with name " + roomName;
    }

    IEnumerator InstantiateNetworkedAvatarOnceInRoom()
    {
        while (PhotonNetwork.InRoom == false)
        {
            Debug.Log("Waiting to be in room before intantiating avatar");
            yield return null;
        }
        InstantiateNetworkedAvatar();
    }

    void InstantiateNetworkedAvatar()
    {
        float rand_x = UnityEngine.Random.Range(m_minSpawnPos_x, m_maxSpawnPos_x);
        float rand_z = UnityEngine.Random.Range(m_minSpawnPos_z, m_maxSpawnPos_z);
        Vector3 spawnPos = new Vector3(rand_x, SPAWN_POS_Z, rand_z);
        Int64 userId = Convert.ToInt64(m_userId);
        object[] objects = new object[1] { userId };
        GameObject myAvatar = PhotonNetwork.Instantiate(AVATAR_PREFAB_NAME, spawnPos, Quaternion.identity, 0, objects);
        m_camera.transform.SetParent(myAvatar.transform);
        m_camera.transform.localPosition = Vector3.zero;
        m_camera.transform.localRotation = Quaternion.identity;
    }

}
