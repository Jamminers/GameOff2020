using System;
using UnityEngine;

public class ShipReactor : ShipComponent
{
    [SerializeField]
    float m_intensityMax;
    [SerializeField]
    float m_speedMax;

    [SerializeField]
    float m_delayAcceleration, m_delayDeceleration;
    [SerializeField]
    AnimationCurve m_curveAcceleration, m_curveDeceleration;

    bool m_active;
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
        if (m_active)
        {
            m_t = Mathf.Clamp01(m_t + Time.fixedDeltaTime / m_delayAcceleration);
            m_intensityCurrent = m_curveAcceleration.Evaluate(m_t);
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

        m_trail.emitting = force.magnitude != 0;
    }
}
