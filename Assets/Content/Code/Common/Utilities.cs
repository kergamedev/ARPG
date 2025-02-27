using Photon.Deterministic;
using UnityEngine;

namespace ARPG.Common
{
    public static class Utilities
    {
        public static void DebugCircle(FPVector3 position, FP radius, Color color, float duration)
        {
            DebugCircle(new Vector3(position.X.AsFloat, position.Y.AsFloat, position.Z.AsFloat), radius.AsFloat, color, duration);
        }
        public static void DebugCircle(Vector3 position, float radius, Color color, float duration)
        {
            var step = Mathf.PI * 2.0f / 32.0f;
            var previous = new Vector3(Mathf.Cos(0.0f) * radius, 0.0f, Mathf.Sin(0.0f) * radius);
            for (var i = 1; i < 32; i++)
            {
                var radians = step * i;
                var current = new Vector3(Mathf.Cos(radians) * radius, 0.0f, Mathf.Sin(radians) * radius);
                Debug.DrawLine(position + previous, position + current, color, duration);

                previous = current;
            }
        }
    }
}
