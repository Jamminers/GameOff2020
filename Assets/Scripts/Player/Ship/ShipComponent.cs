using UnityEngine;

public abstract class ShipComponent : MonoBehaviour
{
    public abstract void Init(ShipController.ShipContext context);
}
