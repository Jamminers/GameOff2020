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

    [HideInInspector]
    public Rigidbody Rigidbody;
    [HideInInspector]
    public Vector3 Up, Forward, CircuitPosition;
    [HideInInspector]
    public Quaternion CircuitRotation;
    [HideInInspector]
    public Vector3 AbsoluteVelocity;

    int m_raycastLayerMask;

    Vector3 m_oldPosition;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        m_level = LevelManager.Instance;

        m_raycastLayerMask = LayerMask.GetMask("Environment");
    }

    protected void FixedUpdate()
    {
        var projection = m_level.getCircuitProjection(transform.position);
        CircuitPosition = projection.location;
        Forward = projection.tangent;

        AbsoluteVelocity = (Rigidbody.position - m_oldPosition) / Time.fixedDeltaTime;
        Up = (CircuitPosition - transform.position).normalized;

        Vector3 upShip = Vector3.ProjectOnPlane(Up, projection.tangent).normalized;
        Vector3 upCircuit = Vector3.ProjectOnPlane(projection.up, projection.tangent).normalized;
        CircuitRotation = Quaternion.FromToRotation(upCircuit, upShip) * projection.Rotation;

        m_oldPosition = Rigidbody.position;
        m_circuitFollow.transform.position = CircuitPosition;
        Vector3 eulerRotation = m_circuitFollow.transform.eulerAngles;
        eulerRotation.z = upShip.z;
        m_circuitFollow.transform.rotation = CircuitRotation;

        // Hover effect
        Ray rayToFloor = new Ray(transform.position, -Up);
        RaycastHit hit;
        if (Physics.Raycast(rayToFloor, out hit, m_hoverHeight))
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
