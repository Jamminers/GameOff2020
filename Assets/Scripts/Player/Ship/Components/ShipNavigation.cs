using UnityEngine;

public class ShipNavigation : ShipComponent
{
    [SerializeField]
    float m_intensity;

    float m_direction;

    Transform m_shipTransform;

    public override void Init(ShipController.ShipContext context)
    {
        m_shipTransform = context.rigidbody.transform;
        context.onDirection += (float direction) => m_direction = direction;
    }

    private void FixedUpdate()
    {
        m_shipTransform.RotateAround(transform.position, Vector3.forward, -m_direction * m_intensity);
    }
}
