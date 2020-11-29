using UnityEngine;

public class ShipBrake : ShipComponent
{
    [SerializeField]
    float m_intensity;

    bool m_active;

    protected override void InitSpecific()
    {
        m_context.onBrake += (float value) => m_active = value == 1;
    }

    private void FixedUpdate()
    {
        if (m_active)
        {
            Vector3 force = m_intensity * m_context.ship.AbsoluteVelocity * Time.fixedDeltaTime;
            m_context.ship.Rigidbody.AddForce(-force, ForceMode.VelocityChange);
        }
    }
}
