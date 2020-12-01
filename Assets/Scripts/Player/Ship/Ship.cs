using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class ShipContext
{
    [HideInInspector]
    public Ship ship;

    public Transform circuitFollow, circuitForward;

    public AudioSourceController audioSourceController;

    public delegateFloat onDirection;
    public delegateFloat onAccelerate;
    public delegateFloat onBrake;

    public ShipContext(Ship sourceShip)
    {
        ship = sourceShip;
    }
}

public class Ship : CircuitBody
{
    [Header("Ship")]
    [SerializeField]
    Transform m_componentsParent;

    [SerializeField]
    ShipContext m_context;

    [Header("Collision")]
    [SerializeField]
    float m_collisionThresholdRatio = 0.7f;
    float m_collisionThreshold;
    [SerializeField]
    float m_respawnDistance = 50;
    [SerializeField]
    GameObject m_deathParticles;

    [Header("Game Logic")]
    [SerializeField]
    UnityEvent<int> m_onFinish;

    [HideInInspector]
    public Material m_material;

    new void Awake()
    {
        base.Awake();
        m_context.ship = this;
    }

    public void BuildFromComponents(GameObject[] components)
    {
        foreach (var c in components)
        {
            var component = Instantiate(c, m_componentsParent).GetComponent<ShipComponent>();
            component.Init(m_context);

            foreach (var mr in component.GetComponentsInChildren<MeshRenderer>())
            {
                mr.material = m_material;
            }
        }

        m_collisionThreshold = m_componentsParent.GetComponentInChildren<ShipReactor>().SpeedMax * m_collisionThresholdRatio * 1000;
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        m_context.circuitFollow.position = CircuitPosition;
        m_context.circuitFollow.rotation = CircuitRotation;
    }

    public void OnDirection(InputValue value)
    {
        m_context.onDirection?.Invoke(value.Get<float>());
    }

    public void OnAccelerate(InputValue value)
    {
        m_context.onAccelerate?.Invoke(value.Get<float>());
    }

    public void OnBrake(InputValue value)
    {
        m_context.onBrake?.Invoke(value.Get<float>());
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.impulse.magnitude > m_collisionThreshold)
        {
            Debug.Log($"You dead : {other.impulse.magnitude}");
            Instantiate(m_deathParticles, Rigidbody.position, Quaternion.identity);
            Rigidbody.position = m_level.SplineCircuit.GetSampleAtDistance(m_circuitProjection.distanceInCurve - m_respawnDistance).location;
        }
    }

    public void EndGame(int rank)
    {
        m_onFinish.Invoke(rank);
    }
}
