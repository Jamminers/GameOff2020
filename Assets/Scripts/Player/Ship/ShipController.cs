using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Ship))]
public class ShipController : MonoBehaviour
{
    public delegate void delegateFloat(float value);

    public class ShipContext
    {
        public Ship ship;

        public delegateFloat onDirection;
        public delegateFloat onAccelerate;
        public delegateFloat onBrake;
    }
    ShipContext m_context;

    void Start()
    {
        m_context = new ShipContext()
        {
            ship = GetComponent<Ship>(),
        };

        foreach (var component in GetComponentsInChildren<ShipComponent>())
        {
            component.Init(m_context);
        }
    }

    public void OnDirection(InputValue val)
    {
        if (m_context.onDirection != null)
            m_context.onDirection(val.Get<float>());
    }

    public void OnAccelerate(InputValue val)
    {
        if (m_context.onAccelerate != null)
            m_context.onAccelerate(val.Get<float>());
    }

    public void OnBrake(InputValue val)
    {
        if (m_context.onBrake != null)
            m_context.onBrake(val.Get<float>());
    }
}
