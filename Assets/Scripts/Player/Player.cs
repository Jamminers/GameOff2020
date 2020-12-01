using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    const int MASK_START_PLAYER = 20;

    [SerializeField]
    Text m_title;

    [SerializeField]
    Material[] m_availableSkins;

    void Awake()
    {
        var input = GetComponentInChildren<PlayerInput>();

        // Assign correct cameras layer
        var cam = GetComponentInChildren<Camera>();
        var vCam = cam.GetComponentInChildren<CinemachineVirtualCamera>();
        vCam.gameObject.layer = LayerMask.NameToLayer($"Player {input.playerIndex}");
        for (int i = MASK_START_PLAYER; i < 24; i++)
        {
            if (i != MASK_START_PLAYER + input.playerIndex)
                cam.cullingMask &= ~(1 << i);
        }

        m_title.text = string.Format(m_title.text, input.playerIndex + 1);

        transform.eulerAngles = new Vector3(0, 0, 90 * input.playerIndex);

        GetComponentInChildren<Ship>().m_material = m_availableSkins[input.playerIndex];
    }
}
