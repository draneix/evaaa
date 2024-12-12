using UnityEngine;

namespace Assets.Scripts.Utility
{
    [System.Serializable]
    public class ThreeDVector
    {
        public float x;
        public float y;
        public float z;

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
    [System.Serializable]
    public class ColorVector
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public Color ToColor()
        {
            return new Color(r, g, b, a);
        }
    }
    [System.Serializable]
    public class EVRange
    {
        public float min;
        public float max;
    }

    [System.Serializable]
    public class Coefficient
    {
        public float change_0;
        public float change_1;
        public float change_2;
        public float change_3;
        public float change_4;
        public float change_5;
    }
    [System.Serializable]
    public class PositionRange
    {
        public float xMin = 0, xMax = 0, yMin = 0, yMax = 0, zMin = 0, zMax = 0;
    }

    [System.Serializable]
    public class RotationRange
    {
        public float x = 0, y = 0, z = 0;
    }

    [System.Serializable]
    public class ScaleRange
    {
        public float xMin = 1, xMax = 1, yMin = 1, yMax = 1, zMin = 1, zMax = 1;
    }
}