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

    [SerializeField]
    public Transform CircuitForward;

    float m_currentRotation = 0;
    float m_direction;

    Ship m_ship;
    LayerMask m_levelLayerMask;

    private void Awake()
    {
        m_levelLayerMask = 1 << LevelManager.Instance.gameObject.layer;
    }

    public override void Init(ShipController.ShipContext context)
    {
        m_ship = context.ship;
        context.onDirection += (float direction) => m_direction = direction;
    }

    private void FixedUpdate()
    {
        m_currentRotation += m_direction * m_steerIntensity * Time.fixedDeltaTime;

        Vector3 forwardDirection = m_ship.CircuitFollow.forward * m_lookAheadDistance;
        forwardDirection = Quaternion.AngleAxis(m_currentRotation, m_ship.CircuitFollow.up) * forwardDirection;

        Vector3 lookForwardDirection = m_ship.CircuitFollow.forward * m_lookForwardDistance;
        lookForwardDirection = Quaternion.AngleAxis(m_currentRotation, m_ship.CircuitFollow.up) * lookForwardDirection;


        Vector3 forwardPostion = transform.position + forwardDirection * m_lookAheadDistance;
        Vector3 lookForwardPostion = transform.position + lookForwardDirection * m_lookForwardDistance;
        Vector3 directionDown = m_ship.transform.position - m_ship.CircuitPosition;
        Vector3 directionDownForward = forwardPostion - m_ship.CircuitPosition;
        Vector3 directionLookForwardPostion = lookForwardPostion - m_ship.CircuitPosition;

        Vector3 directionStart, directionEnd;

        RaycastHit hit;
        Physics.Raycast(m_ship.CircuitPosition, directionDown.normalized, out hit, 100, m_levelLayerMask);
        Debug.DrawRay(m_ship.CircuitPosition, directionDown.normalized * hit.distance, Color.red, Time.fixedDeltaTime);
        directionStart = hit.point;

        float currentHeight = (m_ship.transform.position - directionStart).magnitude;

        Physics.Raycast(m_ship.CircuitPosition, directionDownForward.normalized, out hit, 100, m_levelLayerMask);
        Debug.DrawRay(m_ship.CircuitPosition, directionDownForward.normalized * hit.distance, Color.red, Time.fixedDeltaTime);
        directionEnd = hit.point;

        Physics.Raycast(m_ship.CircuitPosition, directionLookForwardPostion.normalized, out hit, 100, m_levelLayerMask);
        Debug.DrawRay(m_ship.CircuitPosition, directionLookForwardPostion.normalized * hit.distance, Color.red, Time.fixedDeltaTime);
        CircuitForward.position = hit.point + currentHeight * -directionLookForwardPostion.normalized;

        Vector3 direction = directionEnd - directionStart;

        Debug.DrawRay(m_ship.transform.position, direction, Color.red, Time.fixedDeltaTime);

        m_ship.Rigidbody.rotation = Quaternion.Lerp(
            m_ship.Rigidbody.rotation,
            Quaternion.LookRotation(direction, -directionDown),
            Time.fixedDeltaTime * m_reactivity
        );
    }
}
