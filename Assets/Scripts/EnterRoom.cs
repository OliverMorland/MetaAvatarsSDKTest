using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class EnterRoom : MonoBehaviourPunCallbacks
{
    const string ROOM_NAME = "Meta Avatar Sdk Test Room";
    const float SPAWN_POS_Z = 1f;
    const string AVATAR_PREFAB_NAME = "GGMetaAvatarEntity";

    [SerializeField] Text m_screenText;
    [SerializeField] float m_minSpawnPos_x = -5f;
    [SerializeField] float m_maxSpawnPos_x = 5f;
    [SerializeField] float m_minSpawnPos_z = -5f;
    [SerializeField] float m_maxSpawnPos_z = 5f;

    // Start is called before the first frame update
    void Start()
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
        float rand_x = Random.Range(m_minSpawnPos_x, m_maxSpawnPos_x);
        float rand_z = Random.Range(m_minSpawnPos_z, m_maxSpawnPos_z);
        Vector3 spawnPos = new Vector3(rand_x, SPAWN_POS_Z, rand_z);
        PhotonNetwork.Instantiate(AVATAR_PREFAB_NAME, spawnPos, Quaternion.identity);
    }
}
