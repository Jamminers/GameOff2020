using System;
using UnityEngine;
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
        }
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
}
