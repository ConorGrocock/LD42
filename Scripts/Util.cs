using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Util
    {
        private static float clamp(float x, float lower, float upper) {
            return Math.Max(lower, Math.Min(upper, x));
        }
        
        public static Vector3 getNearestPointInPerimeter(Rect rect, float x, float y) {
            float l = rect.x, t = rect.y, w = rect.width, h = rect.height;

            float r = l + w, b = t + h;

            x = clamp(x, l, r);
            y = clamp(y, t, b);

            float dl = Math.Abs(x - l), dr = Math.Abs(x - r), dt = Math.Abs(y - t), db = Math.Abs(y - b);
            float m = Math.Min(Math.Min(Math.Min(dl, dr), dt), db);

            if (m == dt) return new Vector3(x, t,0);
            if (m == db) return new Vector3(x, b,0);
            if (m == dl) return new Vector3(l, y,0);
            return new Vector3(r, y,0);
        }
    }
}