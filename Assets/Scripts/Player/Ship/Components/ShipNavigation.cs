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
        m_context.circuitForward.position = ProjectAtDistance(m_lookForwardDistance);

        Vector3 direction = ProjectAtDistance(m_lookAheadDistance) - ProjectAtDistance(0);

        Debug.DrawRay(transform.position, direction, Color.red, Time.fixedDeltaTime);

        m_context.ship.Rigidbody.rotation = Quaternion.Lerp(
            m_context.ship.Rigidbody.rotation,
            Quaternion.LookRotation(direction, m_context.circuitFollow.up),
            Time.fixedDeltaTime * m_reactivity
        );
    }

    Vector3 ProjectAtDistance(float distance)
    {
        Vector3 circuitTarget = m_context.circuitFollow.forward * distance;
        circuitTarget = Quaternion.AngleAxis(m_currentRotation, m_context.circuitFollow.up) * circuitTarget;
        Vector3 target = transform.position + circuitTarget * distance;
        Vector3 direction = target - m_context.ship.CircuitPosition;
        Vector3 result = target;
        RaycastHit hit;
        if (Physics.Raycast(m_context.ship.CircuitPosition, direction.normalized, out hit, 100, m_levelLayerMask))
        {
            Debug.DrawRay(m_context.ship.CircuitPosition, direction.normalized * hit.distance, Color.red, Time.fixedDeltaTime);
            result = hit.point;
        }
        return result;
    }
}
