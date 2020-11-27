using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(Spline))]
public class LevelManager : Singleton<LevelManager>
{
    [HideInInspector]
    public Spline SplineCircuit;

    private void Awake()
    {
        SplineCircuit = GetComponent<Spline>();
    }
}
