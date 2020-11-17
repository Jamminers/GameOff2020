using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CircuitBody : MonoBehaviour
{
    [SerializeField]
    float m_hoverHeight = 1;
    [SerializeField]
    float m_hoverForce = 1;
    [SerializeField]
    Transform m_circuitFollow;

    LevelManager m_level;
    Rigidbody m_rigidbody;

    int m_raycastLayerMask;

    Vector3 m_oldPosition;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_level = LevelManager.Instance;

        m_raycastLayerMask = LayerMask.GetMask("Environment");
    }

    protected void FixedUpdate()
    {
        Vector3 absoluteVelocity = m_rigidbody.position - m_oldPosition;
        Vector3 circuitPosition = m_level.getCircuitProjectedPosition(transform.position);
        Vector3 upAxis = (circuitPosition - transform.position).normalized;

        Ray rayToFloor = new Ray(transform.position, -upAxis);
        RaycastHit hit;
        if (Physics.Raycast(rayToFloor, out hit, m_hoverHeight))
        {
            Vector3 downForce = Vector3.Project(absoluteVelocity, -upAxis) / Time.fixedDeltaTime;
            float downVelocity = 0;
            if (Vector3.Dot(m_rigidbody.velocity, -upAxis) > 0)
            {
                downVelocity = downForce.magnitude;
                downVelocity *= downVelocity;
            }
            float hoverRatio = 1 - (hit.distance / m_hoverHeight);
            m_rigidbody.AddForce(upAxis * (m_hoverForce * hoverRatio + downVelocity), ForceMode.Acceleration);
        }
        else
        {
            m_rigidbody.AddForce(upAxis * Physics.gravity.y, ForceMode.Acceleration);
        }

        Vector3 rotation = transform.eulerAngles;
        rotation.z = Quaternion.FromToRotation(Vector3.up, upAxis).eulerAngles.z;
        transform.eulerAngles = rotation;

        m_oldPosition = m_rigidbody.position;
        m_circuitFollow.transform.position = circuitPosition;
        m_circuitFollow.transform.eulerAngles = rotation;
    }
}
