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
    [SerializeField] private Vector3 _initialDirection;
    [Header("HelixDatas"), SerializeField] private HelixData[] _helixDatas;

    [Serializable]
    private enum Type
    {
        Helix,
        Looping
    }

    [Serializable]
    private struct HelixData
    {
        [SerializeField] private int _nodeNumber;
        [SerializeField] private float _step;
        [SerializeField] private float _xSize;
        [SerializeField] private float _ySize;
        [SerializeField] private float _zSize;
        [SerializeField] private Type _type;

        public int NodeNumber => _nodeNumber;
        public float Step => _step;
        public float XSize => _xSize;
        public float YSize => _ySize;
        public float ZSize => _zSize;
        public Type Type => _type;
    }

    private void Update()
    {
        if (_clear)
        {
            _clear = false;
            Clear();
        }

        if (_generate)
        {
            _generate = false;

            _spline.nodes.Clear();
            _spline.curves.Clear();

            var t = 0f;
            var initialPosition = _initialPosition;
            _spline.AddNode(new SplineNode(_initialPosition, _initialDirection));

            for (var index = 0; index < _helixDatas.Length; index++)
            {
                var helixData = _helixDatas[index];
                t = GenerateHelix(t, helixData, initialPosition);
                var splineNode = _spline.nodes.Last();
                initialPosition = splineNode.Position;
            }

            _spline.RaiseNodeListChanged(new ListChangedEventArgs<SplineNode>()
            {
                type = ListChangeType.clear
            });
            _spline.UpdateAfterCurveChanged();

            _splineSmoother.SmoothAll();

            var cubicBezierCurves = _spline.curves;
            for (var i = 0; i < cubicBezierCurves.Count; i++)
            {
                if (i > 0)
                {
                    cubicBezierCurves[i].Previous = cubicBezierCurves[i - 1];
                }
            }
        }
    }

    private void Clear()
    {
        _spline.Reset();
        var parentChildCount = _parent.childCount;
        for (var i = 0; i < parentChildCount; i++)
            DestroyImmediate(_parent.transform.GetChild(i).gameObject);
    }

    private float GenerateHelix(float t, HelixData helixData, Vector3 initialPosition)
    {
        var offsetPosition = Strategy(t - helixData.Step, helixData) - initialPosition;
        for (var i = 0; i < helixData.NodeNumber; i++, t += helixData.Step)
        {
            var position = Strategy(t, helixData) - offsetPosition;

            var previousNodePosition = Strategy(t - helixData.Step, helixData) - offsetPosition;
            var nextNodePosition = Strategy(t + helixData.Step, helixData) - offsetPosition;
            var magnitude = (nextNodePosition - previousNodePosition).magnitude / 4;

            var previousPosition = Strategy(t - 0.1f, helixData) - offsetPosition;
            var nextPosition = Strategy(t + 0.1f, helixData) - offsetPosition;
            var direction = (nextPosition - previousPosition).normalized * magnitude;

            var node = new SplineNode(position, direction);
            _spline.AddNode(node);
        }

        return t + 2 * helixData.Step;
    }

    private static Vector3 Strategy(float t, HelixData helixData)
    {
        switch (helixData.Type)
        {
            case Type.Helix:
                return CircularHelixVector(t, helixData.XSize, helixData.YSize, helixData.ZSize);
            case Type.Looping:
                return LoopingVector(t, helixData.XSize, helixData.YSize, helixData.ZSize);
            default:
                throw new ArgumentOutOfRangeException(nameof(helixData.Type), helixData.Type, null);
        }
    }

    private static Vector3 CircularHelixVector(float t, float xSize, float ySize, float zSize)
    {
        return new Vector3(xSize * Mathf.Cos(t), ySize * Mathf.Sin(t), zSize * t / 2);
    }

    private static Vector3 LoopingVector(float t, float xSize, float ySize, float zSize)
    {
        return new Vector3(xSize * t / 2, ySize * Mathf.Sin(t), zSize * Mathf.Cos(t));
    }
}