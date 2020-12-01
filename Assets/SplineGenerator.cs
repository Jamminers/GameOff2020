using System;
using System.Linq;
using SplineMesh;
using UnityEngine;


[ExecuteInEditMode]
public class SplineGenerator : MonoBehaviour
{
    [SerializeField] private Spline _spline;
    [SerializeField] private SplineSmoother _splineSmoother;
    [SerializeField] private bool _generate;
    [SerializeField] private bool _clear;
    [SerializeField] private Transform _parent;

    [SerializeField] private Vector3 _initialPosition;
    [Header("HelixDatas"), SerializeField] private HelixData[] _helixDatas;

    [Serializable]
    private struct HelixData
    {
        [SerializeField] private int _nodeNumber;
        [SerializeField] private float _step;
        [SerializeField] private float _xSize;
        [SerializeField] private float _ySize;
        [SerializeField] private float _zSize;

        public int NodeNumber => _nodeNumber;
        public float Step => _step;
        public float XSize => _xSize;
        public float YSize => _ySize;
        public float ZSize => _zSize;
    }

    private void Update()
    {
        if (_clear)
        {
            _clear = false;
            _spline.nodes.Clear();
            _spline.RefreshCurves();
            var parentChildCount = _parent.childCount;
            for (var i = 0; i < parentChildCount; i++)
                DestroyImmediate(_parent.transform.GetChild(i).gameObject);
        }

        if (_generate)
        {
            _generate = false;

            _spline.Reset();

            var t = 0f;
            var initialPosition = _initialPosition;
            foreach (var helixData in _helixDatas)
            {
                t = GenerateHelix(t, helixData, initialPosition);
                var splineNode = _spline.nodes.Last();
                initialPosition = splineNode.Position;
            }


            _spline.RefreshCurves();
            _splineSmoother.SmoothAll();
        }
    }

    private float GenerateHelix(float t, HelixData helixData, Vector3 initialPosition)
    {
        for (var i = 0; i < helixData.NodeNumber; i++, t += helixData.Step)
        {
            var position = CircularHelixVector(t, helixData.XSize, helixData.YSize, helixData.ZSize) + initialPosition;
            var nextNodePosition = CircularHelixVector(t + helixData.Step, helixData.XSize, helixData.YSize, helixData.ZSize) + initialPosition;
            var node = new SplineNode(position, new Vector3()) {Up = (nextNodePosition - position) / 2};
            _spline.AddNode(node);
        }

        return t;
    }

    private static Vector3 CircularHelixVector(float t, float xSize, float ySize, float zSize)
    {
        return new Vector3(xSize * Mathf.Cos(t), ySize * Mathf.Sin(t), zSize * t / 2);
    }
}