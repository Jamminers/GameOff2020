﻿using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Ship))]
public class ShipController : MonoBehaviour, ShipControls.IShipActions
{
    public delegate void delegateDirection(float direction);
    public delegate void delegateAccelerate(bool accelerate);

    public class ShipContext
    {
        public Ship ship;

        public delegateDirection onDirection;
        public delegateAccelerate onAccelerate;
    }
    ShipContext m_context;

    ShipControls m_controls;

    void Start()
    {
        m_controls = new ShipControls();
        m_controls.Ship.SetCallbacks(this);
        m_controls.Ship.Enable();

        m_context = new ShipContext()
        {
            ship = GetComponent<Ship>(),
        };

        foreach (var component in GetComponentsInChildren<ShipComponent>())
        {
            component.Init(m_context);
        }
    }

    public void OnDirection(InputAction.CallbackContext context)
    {
        // float value = context.phase == InputActionPhase.Started ? context.ReadValue<float>() : 0;
        float value = context.ReadValue<float>();
        if (m_context.onDirection != null)
            m_context.onDirection(value);
    }

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        if (m_context.onAccelerate != null)
            m_context.onAccelerate(context.ReadValueAsButton());
    }
}
