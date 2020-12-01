using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    GameState m_currentState;

    PlayerInputManager m_playerInputManager;

    [SerializeField]
    UnityEvent<GameState> m_onMenuWelcome, m_onMenuLobby, m_onGameMain;

    void Awake()
    {
        m_playerInputManager = GetComponentInChildren<PlayerInputManager>();
        m_playerInputManager.onPlayerJoined += (playerInput) => SetState(GameState.MenuLobby);
        SetState(GameState.MenuWelcome);
    }

    public void SetState(GameState state)
    {
        print($"[GameManager] Set state : {state.ToString()}");
        m_currentState = state;

        switch (m_currentState)
        {
            case GameState.MenuWelcome:
                m_onMenuWelcome.Invoke(m_currentState);
                break;
            case GameState.MenuLobby:
                m_onMenuLobby.Invoke(m_currentState);
                break;
            case GameState.GameMain:
                m_onGameMain.Invoke(m_currentState);
                break;
        }
    }
}
