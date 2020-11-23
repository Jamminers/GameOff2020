using UnityEngine;

public class ShipNavigation : ShipComponent
{
    [SerializeField]
    float m_intensity;

    [SerializeField]
    float m_steerRange = 90;

    float m_direction;

    Ship m_ship;

    public override void Init(ShipController.ShipContext context)
    {
        m_ship = context.ship;
        context.onDirection += (float direction) => m_direction = direction;
    }

    private void FixedUpdate()
    {
        Vector3 vectorDown = m_ship.transform.position - m_ship.CircuitPosition;
        Vector3 vectorLeft = Quaternion.AngleAxis(-m_steerRange, m_ship.m_followFront.position) * vectorDown;
        Vector3 vectorRight = Quaternion.AngleAxis(m_steerRange, m_ship.m_followFront.position) * vectorDown;

        Debug.DrawRay(m_ship.CircuitPosition, vectorDown, Color.blue, Time.fixedDeltaTime);
        Debug.DrawRay(m_ship.CircuitPosition, vectorLeft, Color.red, Time.fixedDeltaTime);
        Debug.DrawRay(m_ship.CircuitPosition, vectorRight, Color.green, Time.fixedDeltaTime);

        Quaternion targetDirection = m_ship.CircuitRotation;
        if (m_direction > 0)
            targetDirection = Quaternion.LookRotation(vectorRight - vectorDown, -vectorDown);
        else if (m_direction < 0)
            targetDirection = Quaternion.LookRotation(vectorLeft - vectorDown, -vectorDown);

        m_ship.Rigidbody.rotation = Quaternion.Lerp(
            m_ship.Rigidbody.rotation,
            targetDirection,
            Time.fixedDeltaTime * m_intensity
        );
    }
}
