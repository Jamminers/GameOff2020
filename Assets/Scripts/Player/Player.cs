using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    const int MASK_START_PLAYER = 20;

    PlayerInput m_input;


    void Awake()
    {
        m_input = GetComponent<PlayerInput>();

        // Assign correct cameras layer
        var cam = GetComponentInChildren<Camera>();
        var vCam = cam.GetComponentInChildren<CinemachineVirtualCamera>();
        vCam.gameObject.layer = LayerMask.NameToLayer($"Player {m_input.playerIndex}");
        for (int i = MASK_START_PLAYER; i < 24; i++)
        {
            if (i != MASK_START_PLAYER + m_input.playerIndex)
                cam.cullingMask &= ~(1 << i);
        }
    }
}
