using System.Threading;
using System.Collections;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(Spline))]
public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private Spline m_circuitSpline;

    public Vector3 getCircuitProjectedPosition(Vector3 target)
    {
        CurveSample projection = m_circuitSpline.GetProjectionSample(target);
        return projection.location;
    }
}
