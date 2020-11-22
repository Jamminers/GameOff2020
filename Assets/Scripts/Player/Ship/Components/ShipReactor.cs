using UnityEngine;

public class ShipReactor : ShipComponent
{
    [SerializeField]
    float m_intensity;
    [SerializeField]
    float m_speedMax;

    bool m_active;
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
        Vector3 force = m_intensity * m_ship.Rigidbody.transform.forward * (m_active ? 1 : 0);

        if (m_ship.AbsoluteVelocity.magnitude < m_speedMax)
            m_ship.Rigidbody.AddForceAtPosition(force, transform.position, ForceMode.Acceleration);

        m_trail.emitting = m_active;
    }
}
