using System;
using UnityEngine;

namespace SplineMesh
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SplineExtrusion))]
    public class SplineExtrusionGenerator : MonoBehaviour
    {
        [SerializeField]
        bool m_enableOuter = true;
        [SerializeField]
        int m_sides = 5;
        [SerializeField]
        float m_radiusInner = 1f;
        [SerializeField]
        float m_radiusOuter = 1.1f;

        SplineExtrusion m_splineExtrusion;


        private void OnEnable()
        {
            m_splineExtrusion = GetComponent<SplineExtrusion>();
        }

        [ContextMenu("Generate")]
        private void Generate()
        {
            m_splineExtrusion.shapeVertices.Clear();

            // Inner
            for (int s = 0; s < m_sides; s++)
            {
                float alpha = 2 * Mathf.PI * s / m_sides;
                AddPointOnCircle(alpha, m_radiusInner, s / (float)(m_sides), true);
            }
            AddPointOnCircle(0, m_radiusInner, 1, true);

            if (m_enableOuter)
            {
                // Outer
                for (int s = 0; s > -m_sides; s--)
                {
                    float alpha = 2 * Mathf.PI * s / m_sides;
                    AddPointOnCircle(alpha, m_radiusOuter, s / (float)(m_sides), false);
                }
                AddPointOnCircle(0, m_radiusOuter, -1, false);
            }

            m_splineExtrusion.SetToUpdate();
        }

        private void AddPointOnCircle(float alpha, float radius, float u, bool invertNormal)
        {
            Vector2 point = new Vector2(Mathf.Cos(alpha), Mathf.Sin(alpha)) * radius;
            m_splineExtrusion.shapeVertices.Add(new ExtrusionSegment.Vertex(
                point,
                (invertNormal ? -point : point).normalized * .1f,
                u
            ));
        }
    }
}
