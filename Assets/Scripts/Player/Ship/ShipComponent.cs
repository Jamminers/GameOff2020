using UnityEngine;

public abstract class ShipComponent : MonoBehaviour
{
    protected Ship m_ship;

    public void Init(ShipController.ShipContext context)
    {
        m_ship = context.ship;
        InitSpecific(context);
    }

    protected abstract void InitSpecific(ShipController.ShipContext context);
}
