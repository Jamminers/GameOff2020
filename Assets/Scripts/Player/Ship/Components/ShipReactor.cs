using UnityEngine;
using UnityEngine.Events;

public class ShipReactor : ShipComponent
{
    [SerializeField]
    AudioClip m_clip;
    [SerializeField]
    float m_coolDown, m_releaseTime;
    [SerializeField]
    bool m_oneShot;

    [Header("Power")]
    [SerializeField]
    float m_intensityMax;
    [SerializeField]
    public float SpeedMax;

    [Header("Acceleration")]
    [SerializeField]
    float m_delayAcceleration;
    [SerializeField]
    AnimationCurve m_curveAcceleration;

    [Header("Deceleration")]
    [SerializeField]
    float m_delayDeceleration;
    [SerializeField]
    AnimationCurve m_curveDeceleration;

    [Header("Events")]
    [SerializeField]
    UnityEvent<bool> m_onAccelerate;
    [SerializeField]
    UnityEvent<float> m_onSpeed;

    bool m_active, m_released = true;
    float m_lastActive;
    float m_t, m_intensityCurrent;

    protected override void InitSpecific()
    {
        m_context.onAccelerate += (float value) => m_active = value == 1;

        m_context.audioSourceController.PlayClip(m_clip);
        m_onAccelerate.AddListener(m_context.audioSourceController.SetVolume);
    }

    private void FixedUpdate()
    {
        bool isCool = m_oneShot ? m_released : m_lastActive + m_coolDown < Time.time;

        bool isReleasing = m_lastActive + m_releaseTime > Time.time;
        if ((m_active && isCool) || isReleasing)
        {
            m_t = Mathf.Clamp01(m_t + Time.fixedDeltaTime / m_delayAcceleration);
            m_intensityCurrent = m_curveAcceleration.Evaluate(m_t);
            if (m_active && !isReleasing)
            {
                m_lastActive = Time.time;
            }
        }
        else
        {
            m_t = Mathf.Clamp01(m_t - Time.fixedDeltaTime / m_delayDeceleration);
            m_intensityCurrent = m_curveDeceleration.Evaluate(1 - m_t);
        }
        m_intensityCurrent *= m_intensityMax;

        Vector3 force = m_intensityCurrent * m_context.ship.Rigidbody.transform.forward;
        if (m_context.ship.AbsoluteVelocity.magnitude < SpeedMax)
            m_context.ship.Rigidbody.AddForceAtPosition(force, transform.position, ForceMode.Acceleration);

        m_onAccelerate.Invoke(m_intensityCurrent != 0);
        m_onSpeed.Invoke(m_intensityCurrent);

        m_released = !m_active;
    }
}
