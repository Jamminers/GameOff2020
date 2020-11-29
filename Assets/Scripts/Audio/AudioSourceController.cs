using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceController : MonoBehaviour
{
    [SerializeField]
    float m_transitionTime;
    [SerializeField]
    AnimationCurve m_volumeCurve;

    AudioSource m_source;

    private bool m_volumeActive = false;
    private float m_currentTime = 0;

    private void Awake()
    {
        m_source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        m_currentTime = Mathf.Clamp01(m_currentTime + ((m_volumeActive ? Time.deltaTime : -Time.deltaTime) / m_transitionTime));
        m_source.volume = m_volumeCurve.Evaluate(m_currentTime);
    }

    public void PlayClip(AudioClip clip)
    {
        m_source.clip = clip;
        m_source.Play();
    }

    public void SetVolume(bool enabled)
    {
        m_volumeActive = enabled;
    }
}
