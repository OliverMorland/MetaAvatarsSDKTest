using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvatarMovementController : MonoBehaviour
{
    const float EPSILON = 0.001f;
    [SerializeField] float m_speedMultiplier = 0.1f;
    PhotonView m_photonView;

    private void Start()
    {
        m_photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        MoveMyAvatar();
    }

    void MoveMyAvatar()
    {
        if (m_photonView != null && m_photonView.IsMine)
        {
            Vector2 leftThumbstickPos = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            Vector3 velocity = (GetVelocityFromArrowKeysInput() + new Vector3(leftThumbstickPos.x, 0, leftThumbstickPos.y)) * m_speedMultiplier;
            Vector3 displacement = velocity * Time.deltaTime;
            transform.position += displacement; 
        }
    }


    Vector3 GetVelocityFromArrowKeysInput()
    {
        Vector3 velocity = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.UpArrow) == true)
        {
            velocity += transform.forward;
        }
        if (Input.GetKey(KeyCode.LeftArrow) == true)
        {
            velocity -= transform.right;
        }
        if (Input.GetKey(KeyCode.RightArrow) == true)
        {
            velocity += transform.right;
        }
        if (Input.GetKey(KeyCode.DownArrow) == true)
        {
            velocity -= transform.forward;
        }
        return velocity;
    }
}
