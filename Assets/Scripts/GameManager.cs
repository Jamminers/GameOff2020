using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    GameState m_currentState = GameState.MenuIntro;

    PlayerInputManager m_playerInputManager;
    AudioManager m_audioManager;

    void Awake()
    {
        m_audioManager = AudioManager.Instance;
        m_playerInputManager = GetComponentInChildren<PlayerInputManager>();

        m_playerInputManager.onPlayerJoined += (playerInput) => SetState(GameState.GameMain);
    }

    public void SetState(GameState state)
    {
        print($"[GameManager] Set state : {state.ToString()}");
        m_currentState = state;
        m_audioManager.OnChangeGameState(m_currentState);
    }
}
