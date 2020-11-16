using UnityEngine;

public class ShipReactor : ShipComponent
{
    [SerializeField]
    float m_intensity;

    bool m_active;
    Rigidbody2D m_shipRigidbody2d;
    TrailRenderer m_trail;

    public override void Init(ShipController.ShipContext context)
    {
        m_shipRigidbody2d = context.rigidbody;
        context.onAccelerate += (bool accelerate) => m_active = accelerate;

        m_trail = GetComponentInChildren<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        Vector2 force = Time.fixedDeltaTime * m_intensity * m_shipRigidbody2d.transform.up * (m_active ? 1 : 0);
        m_shipRigidbody2d.AddForceAtPosition(force, transform.position);

        m_trail.emitting = m_active;
    }
}
