using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CircuitBody : MonoBehaviour
{
    LevelManager m_level;

    Rigidbody m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_level = LevelManager.Instance;
    }

    private void FixedUpdate()
    {
        Vector3 circuitPosition = m_level.getCircuitProjectedPosition(transform.position);
        Vector3 upAxis = (circuitPosition - transform.position).normalized;
        Vector3 F = upAxis * Physics.gravity.y;

        m_rigidbody.AddForce(F, ForceMode.Acceleration);

        Vector3 rotation = transform.eulerAngles;
        rotation.z = Quaternion.FromToRotation(Vector3.up, upAxis).eulerAngles.z;
        transform.eulerAngles = rotation;
    }
}
