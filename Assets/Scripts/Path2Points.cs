using UnityEngine;

public class Path2Points
{
    private Vector3 _v1;
    private Vector3 _v2;
    private float _t;
        
    public Path2Points(Vector3 v1, Vector3 v2)
    {
        _v1 = v1;
        _v2 = v2;
    }

    public Vector3 NewPoint(float delta)
    {
        _t += delta;
        if (_t >= 1)
        {
            _t = 0;
            (_v1, _v2) = (_v2, _v1);
        }

        return Vector3.Lerp(_v1, _v2, _t);
    }
}