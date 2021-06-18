using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
namespace Hasel
{
    public class Matrix
    {
        public float m00, m01;
        public float m10, m11;
        public Vector2 xColumn { get { return new Vector2(m00, m10); } }
        public Vector2 yColumn { get { return new Vector2(m01, m11); } }

        public Matrix()
        {

        }
        public Matrix(float RADIANS)
        {
            Set(RADIANS);
        }
        public Matrix(float A, float B, float C, float D)
        {
            Set(A, B, C, D);
        }
        public Matrix(Vector2 A, Vector2 B)
        {
            m00 = A.X;
            m01 = A.Y;
            m10 = B.X;
            m11 = B.Y;
        }
        public void Set(float RADIANS)
        {
            float cos = (float) Math.Cos(RADIANS);
            float sin = (float)Math.Sin(RADIANS);

            m00 = cos;
            m01 = -sin;
            m10 = sin;
            m11 = cos;
        }
        public void Set(float A, float B, float C, float D)
        {
            m00 = A;
            m01 = B;
            m10 = C;
            m11 = D;
        }
        public Matrix Abs()
        {
            return new Matrix(Math.Abs(m00), Math.Abs(m01), Math.Abs(m10), Math.Abs(m11));
        }
        //Rotate A Vector
        public static Matrix operator*(Matrix A, Matrix B) 
        {
            return new Matrix(A.m00 * B.m00 + A.m01 * B.m10,
                A.m00 * B.m01 + A.m01 * B.m11, A.m10 * B.m00 + A.m11 * B.m10,
                A.m10 * B.m01 + A.m11 * B.m11);
        }
        public static Vector2 operator*(Matrix MATRIX, Vector2 VECTOR)
        {
            return new Vector2(MATRIX.m00 * VECTOR.X + MATRIX.m01 * VECTOR.Y, MATRIX.m10 * VECTOR.X + MATRIX.m11 * VECTOR.Y);
        }
        public Matrix Transpose(Matrix MATRIX)
        {
            MATRIX.m00 = m00;
            MATRIX.m01 = m10;
            MATRIX.m10 = m01;
            MATRIX.m11 = m11;
            return MATRIX;
        }
        public Matrix Transpose()
        {
            return Transpose(new Matrix());
        }
        public void Transposei()
        {
            float t = m01;
            m01 = m10;
            m10 = t;
        }
    }
}
