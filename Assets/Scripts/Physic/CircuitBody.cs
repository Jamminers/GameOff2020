using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CircuitBody : MonoBehaviour
{
    [Header("CircuitBody")]
    [SerializeField]
    float m_hoverHeight = 1;
    [SerializeField]
    float m_hoverForce = 1;

    protected LevelManager m_level;

    [HideInInspector]
    public Rigidbody Rigidbody;
    [HideInInspector]
    public Vector3 Up, CircuitPosition, UpShip;
    [HideInInspector]
    public Quaternion CircuitRotation;
    [HideInInspector]
    public Vector3 AbsoluteVelocity;

    Vector3 m_oldPosition;

    protected SplineMesh.CurveSample m_circuitProjection;

    protected void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        m_level = LevelManager.Instance;
    }

    protected void FixedUpdate()
    {
        m_circuitProjection = m_level.SplineCircuit.GetProjectionSample(transform.position);
        CircuitPosition = m_circuitProjection.location;

        AbsoluteVelocity = (Rigidbody.position - m_oldPosition) / Time.fixedDeltaTime;
        Up = (CircuitPosition - transform.position).normalized;

        UpShip = Vector3.ProjectOnPlane(Up, m_circuitProjection.tangent);
        Vector3 upCircuit = Vector3.ProjectOnPlane(m_circuitProjection.up, m_circuitProjection.tangent);
        CircuitRotation = Quaternion.FromToRotation(upCircuit.normalized, UpShip.normalized) * m_circuitProjection.Rotation;

        m_oldPosition = Rigidbody.position;

        // Hover effect
        Ray rayToFloor = new Ray(transform.position, -Up);
        LayerMask levelLayerMask = 1 << m_level.gameObject.layer;
        RaycastHit hit;
        if (Physics.Raycast(rayToFloor, out hit, m_hoverHeight, levelLayerMask))
        {
            Vector3 downForce = Vector3.Project(AbsoluteVelocity, -Up);
            float downVelocity = 0;
            if (Vector3.Dot(Rigidbody.velocity, -Up) > 0)
            {
                downVelocity = downForce.magnitude;
                downVelocity *= downVelocity;
            }
            float hoverRatio = 1 - (hit.distance / m_hoverHeight);
            Rigidbody.AddForce(Up * (m_hoverForce * hoverRatio + downVelocity), ForceMode.Acceleration);
        }
        else
        {
            Rigidbody.AddForce(Up * Physics.gravity.y, ForceMode.Acceleration);
        }
    }
}
