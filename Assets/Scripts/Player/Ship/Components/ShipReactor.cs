﻿using UnityEngine;

public class ShipReactor : ShipComponent
{
    [SerializeField]
    float m_coolDown, m_releaseTime;

    [Header("Power")]
    [SerializeField]
    float m_intensityMax;
    [SerializeField]
    float m_speedMax;

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

    bool m_active;
    float m_lastActive;
    float m_t, m_intensityCurrent;
    Ship m_ship;
    TrailRenderer m_trail;

    public override void Init(ShipController.ShipContext context)
    {
        m_ship = context.ship;
        context.onAccelerate += (float value) => m_active = value == 1;

        m_trail = GetComponentInChildren<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        bool isCool = m_lastActive + m_coolDown < Time.time;
        bool isReleasing = m_lastActive + m_releaseTime > Time.time;
        if ((m_active && isCool) || isReleasing)
        {
            m_t = Mathf.Clamp01(m_t + Time.fixedDeltaTime / m_delayAcceleration);
            m_intensityCurrent = m_curveAcceleration.Evaluate(m_t);
            if (m_active && !isReleasing)
                m_lastActive = Time.time;
        }
        else
        {
            m_t = Mathf.Clamp01(m_t - Time.fixedDeltaTime / m_delayDeceleration);
            m_intensityCurrent = m_curveDeceleration.Evaluate(1 - m_t);
        }
        m_intensityCurrent *= m_intensityMax;

        Vector3 force = m_intensityCurrent * m_ship.Rigidbody.transform.forward;
        if (m_ship.AbsoluteVelocity.magnitude < m_speedMax)
            m_ship.Rigidbody.AddForceAtPosition(force, transform.position, ForceMode.Acceleration);

        m_trail.emitting = m_intensityCurrent != 0;
    }
}