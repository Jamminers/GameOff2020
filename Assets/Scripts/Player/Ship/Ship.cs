using UnityEngine;

public class Ship : CircuitBody
{
    [SerializeField]
    public Transform m_followBack, m_followFront;

    [SerializeField]
    float m_followBackDistance, m_followFrontDistance;

    new void FixedUpdate()
    {
        base.FixedUpdate();

        Vector3 heightDiff = CircuitPosition - transform.position;
        float currentDistance = m_circuitProjection.distancePrevious + m_circuitProjection.distanceInCurve;

        float backDistance = Mathf.Max(0, currentDistance - m_followBackDistance);
        var sampleBack = m_level.SplineCircuit.GetSampleAtDistance(backDistance);
        m_followBack.position = sampleBack.location - heightDiff;
        m_followBack.rotation = CircuitRotation;

        float frontDistance = Mathf.Min(m_level.SplineCircuit.Length, currentDistance + m_followFrontDistance);
        var sampleFront = m_level.SplineCircuit.GetSampleAtDistance(frontDistance);
        m_followFront.position = sampleFront.location - heightDiff;
        m_followFront.rotation = CircuitRotation;
    }
}
