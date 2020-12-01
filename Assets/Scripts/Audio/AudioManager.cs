using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Serializable]
    class AudioClipByGameState
    {
        public GameState State;
        public AudioClip Clip;
    }

    [SerializeField]
    List<AudioClipByGameState> m_clipByState;

    AudioSource m_source;

    private void Awake()
    {
        m_source = GetComponent<AudioSource>();
    }

    public void OnChangeGameState(GameState state)
    {
        AudioClipByGameState found = m_clipByState.Find(i => i.State == state);
        if (found != null)
        {
            m_source.clip = found.Clip;
            m_source.Play();
        }
    }
}