using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Hasel
{
    public static class Calc
    {
        public const float Epsilon = 0.0001f;
        public const float Pi = (float)Math.PI;

        public const float BiasRelative = 0.95f;
        public const float BiasAbsolute = 0.01f;

        public static float Angle(Vector2 FROM, Vector2 TO)
        {
            return (float)Math.Atan2(TO.Y - FROM.Y, TO.X - FROM.X);
        }
        public static float Approach(float VALUE, float TARGET, float INTERVAL)
        {
            return VALUE > TARGET ? Math.Max(VALUE - INTERVAL, TARGET) : Math.Min(VALUE + INTERVAL, TARGET);
        }
        public static Vector2 AngleToVector(float RADIANS, float LENGTH)
        {
            return new Vector2((float)Math.Cos(RADIANS) * LENGTH, (float)Math.Sin(RADIANS) * LENGTH);
        }
        public static float CrossProduct(this Vector2 VECTORA, Vector2 VECTORB)
        {
            return (VECTORA.X * VECTORB.Y) - (VECTORA.Y * VECTORB.X);
        }
        public static Vector2 CrossProduct(this Vector2 VECTOR, float SCALAR)
        {
            return new Vector2(SCALAR * VECTOR.Y, -SCALAR * VECTOR.X);
        }
        public static Vector2 CrossProduct(this float SCALAR, Vector2 VECTOR)
        {
            return new Vector2(-SCALAR * VECTOR.Y, SCALAR * VECTOR.X);
        }
        public static Vector2 Perpendicular(this Vector2 VECTOR)
        {
            return new Vector2(-VECTOR.Y, VECTOR.X);
        }
        public static List<Vector2> Offset(this List<Vector2> POINTS, Vector2 OFFSET)
        {
            List<Vector2> temp = new List<Vector2>();
            for (int i = 0; i < POINTS.Count; i++)
            {
                temp.Add(POINTS[i] + OFFSET);
            }
            return temp;

        }
        public static int Factorial(int NUMBER)
        {
            int add = 1;
            for (int i = 2; i <= NUMBER; i++)
            {
                add *= i;
            }
            return add;
        }
        public static int Random(int MIN, int MAX)
        {
            //MIN and MAX are inclusive

            var rand = new Random();
            return rand.Next(MIN, MAX + 1);
        }
        public static float Random(float MIN, float MAX)
        {
            var rand = new Random();
            return (float)rand.NextDouble() * (MAX - MIN) + MIN;
        }
        public static bool gt(float A, float B)
        {
            return A >= B * BiasRelative + A * BiasAbsolute;
        }
    }
}
