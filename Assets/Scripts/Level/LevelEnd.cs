using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    int count = 0;

    private void OnTriggerEnter(Collider other)
    {
        Ship ship = other.attachedRigidbody?.GetComponent<Ship>();
        if (ship)
        {
            ship.EndGame(count);
        }
    }
}
