using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(Spline))]
public class LevelManager : Singleton<LevelManager>
{
    public Spline SplineCircuit;

    private void Awake()
    {
        SplineCircuit = GetComponent<Spline>();
    }
}
