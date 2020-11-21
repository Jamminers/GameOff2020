using System.Threading;
using System.Collections;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(Spline))]
public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private Spline m_circuitSpline;

    public CurveSample getCircuitProjection(Vector3 target)
    {
        return m_circuitSpline.GetProjectionSample(target);
    }
}
