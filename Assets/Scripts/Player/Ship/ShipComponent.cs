using UnityEngine;

public abstract class ShipComponent : MonoBehaviour
{
    protected ShipContext m_context;

    public void Init(ShipContext context)
    {
        m_context = context;
        InitSpecific();
    }

    protected abstract void InitSpecific();
}
