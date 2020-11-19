using UnityEngine;

public class ShipReactor : ShipComponent
{
    [SerializeField]
    float m_intensity;

    bool m_active;
    Rigidbody m_shipRigidbody;
    TrailRenderer m_trail;

    public override void Init(ShipController.ShipContext context)
    {
        m_shipRigidbody = context.ship.Rigidbody;
        context.onAccelerate += (bool accelerate) => m_active = accelerate;

        m_trail = GetComponentInChildren<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        Vector3 force = m_intensity * m_shipRigidbody.transform.forward * (m_active ? 1 : 0);
        m_shipRigidbody.AddForceAtPosition(force, transform.position, ForceMode.Acceleration);

        m_trail.emitting = m_active;
    }
}
