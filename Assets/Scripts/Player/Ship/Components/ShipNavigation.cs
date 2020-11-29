using UnityEngine;

public class ShipNavigation : ShipComponent
{
    [SerializeField]
    float m_reactivity;

    [SerializeField]
    float m_steerIntensity = 90;
    [SerializeField]
    float m_lookAheadDistance = 3;
    [SerializeField]
    float m_lookForwardDistance = 3;

    float m_currentRotation = 0;
    float m_direction;

    LayerMask m_levelLayerMask;

    protected override void InitSpecific()
    {
        m_levelLayerMask = 1 << LevelManager.Instance.gameObject.layer;
        m_context.onDirection += (float direction) => m_direction = direction;
    }

    private void FixedUpdate()
    {
        m_currentRotation += m_direction * m_steerIntensity * Time.fixedDeltaTime;

        Vector3 forwardDirection = m_context.circuitFollow.forward * m_lookAheadDistance;
        forwardDirection = Quaternion.AngleAxis(m_currentRotation, m_context.circuitFollow.up) * forwardDirection;

        Vector3 lookForwardDirection = m_context.circuitFollow.forward * m_lookForwardDistance;
        lookForwardDirection = Quaternion.AngleAxis(m_currentRotation, m_context.circuitFollow.up) * lookForwardDirection;


        Vector3 forwardPostion = transform.position + forwardDirection * m_lookAheadDistance;
        Vector3 lookForwardPostion = transform.position + lookForwardDirection * m_lookForwardDistance;
        Vector3 directionDown = m_context.ship.transform.position - m_context.ship.CircuitPosition;
        Vector3 directionDownForward = forwardPostion - m_context.ship.CircuitPosition;
        Vector3 directionLookForwardPostion = lookForwardPostion - m_context.ship.CircuitPosition;

        Vector3 directionStart, directionEnd;

        RaycastHit hit;
        Physics.Raycast(m_context.ship.CircuitPosition, directionDown.normalized, out hit, 100, m_levelLayerMask);
        Debug.DrawRay(m_context.ship.CircuitPosition, directionDown.normalized * hit.distance, Color.red, Time.fixedDeltaTime);
        directionStart = hit.point;

        float currentHeight = (m_context.ship.transform.position - directionStart).magnitude;

        Physics.Raycast(m_context.ship.CircuitPosition, directionDownForward.normalized, out hit, 100, m_levelLayerMask);
        Debug.DrawRay(m_context.ship.CircuitPosition, directionDownForward.normalized * hit.distance, Color.red, Time.fixedDeltaTime);
        directionEnd = hit.point;

        Physics.Raycast(m_context.ship.CircuitPosition, directionLookForwardPostion.normalized, out hit, 100, m_levelLayerMask);
        Debug.DrawRay(m_context.ship.CircuitPosition, directionLookForwardPostion.normalized * hit.distance, Color.red, Time.fixedDeltaTime);
        m_context.circuitForward.position = hit.point + currentHeight * -directionLookForwardPostion.normalized;

        Vector3 direction = directionEnd - directionStart;

        Debug.DrawRay(m_context.ship.transform.position, direction, Color.red, Time.fixedDeltaTime);

        m_context.ship.Rigidbody.rotation = Quaternion.Lerp(
            m_context.ship.Rigidbody.rotation,
            Quaternion.LookRotation(direction, -directionDown),
            Time.fixedDeltaTime * m_reactivity
        );
    }
}
