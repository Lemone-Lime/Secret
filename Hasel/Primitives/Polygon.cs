using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Hasel
{
    public class Polygon : Shape
    {
        public const int MaxVertices = 64;

        public List<Vector2> RawVertices;

        public List<Vector2> Vertices
        {
            get { if(Transform != null) return RawVertices.Offset(Offset + Transform.Position); return RawVertices.Offset(Offset); }
        }

        public List<Vector2> Normals;
        public int VertexCount;

        public Matrix u;

        public override float Top
        {
            get
            {
                float lowestX = Vertices[0].Y;
                foreach (var vertex in Vertices)
                {
                    if (vertex.Y < lowestX) lowestX = vertex.Y;
                }
                return lowestX;
            }
        }
        public override float Bottom
        {
            get
            {
                float highestY = Vertices[0].Y;
                foreach (var vertex in Vertices)
                {
                    if (vertex.Y > highestY) highestY = vertex.Y;
                }
                return highestY;
            }
        }
        public override float Left
        {
            get
            {
                float lowestX = Vertices[0].X;
                foreach (var vertex in Vertices)
                {
                    if (vertex.X < lowestX) lowestX = vertex.X;
                }
                return lowestX;
            }
        }
        public override float Right
        {
            get
            {
                float highestX = Vertices[0].X;
                foreach (var vertex in Vertices)
                {
                    if (vertex.X > highestX) highestX = vertex.X;
                }
                return highestX;
            }
        }
        public override Vector2 Center
        {
            get
            {
                float xSum = 0;
                float ySum = 0;
                foreach (var vertex in Vertices)
                {
                    xSum += vertex.X;
                    ySum += vertex.Y;
                }
                return new Vector2(xSum / VertexCount, ySum / VertexCount);
            }
        }

        public Polygon(List<Vector2> VERTICES, Vector2 OFFSET)
        {
            RawVertices = new List<Vector2>(MaxVertices);
            Offset = OFFSET;
            Type = PType.Poly;

            Set(VERTICES);
        }

        public void Set(List<Vector2> VERTICES)
        {
            //Get Rightmost Vertex

            int rightVertex = 0;
            float highestX = VERTICES[0].X;

            for (int i = 1; i > VERTICES.Count; i++)
            {
                float x = VERTICES[i].X;

                if (x > highestX)
                {
                    highestX = x;
                    rightVertex = i;
                } else if (x == highestX){
                    if (VERTICES[i].Y < VERTICES[rightVertex].Y)
                    {
                        rightVertex = i;
                    }
                }
            }


            List<int> hull = new List<int>();
            int outCount = 0;
            int indexHull = rightVertex;

            for (; ; )
            {
                hull.Insert(outCount, indexHull);
                int nextHullIndex = 0;

                for (int i = 1; i < VERTICES.Count; i++)
                {
                    if (nextHullIndex == indexHull)
                    {
                        nextHullIndex = i;
                        continue;
                    }

                    Vector2 edge1 = VERTICES[nextHullIndex] - VERTICES[hull[outCount]];
                    Vector2 edge2 = VERTICES[i] - VERTICES[hull[outCount]];
                    float cross = edge1.CrossProduct(edge2);
                    if ((cross < 0.0f) || (cross == 0.0f && edge2.LengthSquared() > edge1.LengthSquared()))
                    {
                        nextHullIndex = i;
                    }
                }

                outCount++;
                indexHull = nextHullIndex;

                //conclude algorithm upon wrap around
                if (nextHullIndex == rightVertex)
                {
                    VertexCount = outCount;
                    break;
                }
            }

            //copy VERTICES into shape VERTICES
            for (int i = 0; i < VertexCount; i++)
            {
                RawVertices.Add(VERTICES[hull[i]]);
            }

            //compute face normals
            for (int i = 0; i < VertexCount; i++)
            {
                Normals.Add(RawVertices[(i + 1) % VertexCount] - RawVertices[i]);
            }

            CalculateVolume();
        }
        public Vector2 GetSupport(Vector2 DIR)
        {
            float bestProjection = -float.MaxValue;
            Vector2 bestVertex = Vector2.Zero;

            foreach (var vertex in Vertices)
            {
                float projection = Vector2.Dot(vertex, DIR);

                if (projection > bestProjection)
                {
                    bestVertex = vertex;
                    bestProjection = projection;
                }
            }
            return bestVertex;
        }

        public override void CalculateVolume()
        {
            Vector2[] vtc = new Vector2[RawVertices.Count+1];
            RawVertices.CopyTo(vtc, 0);
            vtc[RawVertices.Count] = RawVertices[0];
            float v = 0;
            for (int i = 0; i < RawVertices.Count; i++) {
                v += (vtc[i + 1].X - vtc[i].X) * (vtc[i + 1].Y + vtc[i].Y) / 2;
            }
            Volume = Math.Abs(v);
        }

        public override void SetOrient(float RADIANS)
        {
            u.Set(RADIANS);
        }
        /* Old Code
        private List<Vector2> points;
        private List<Vector2> edges;

        public List<Vector2> Points
        {
            get
            {
                if (points == null) points = new List<Vector2>();
                return points;
            }
        }
        public List<Vector2> Edges
        {
            get
            {
                if (edges == null) edges = new List<Vector2>();
                return edges;
            }
        }
        public Vector2 Center
        {
            get
            {
                Vector2 total = Vector2.Zero;
                for (int i = 0; i < points.Count; i++)
                {
                    total.X += (float)points[i].X;
                    total.Y += (float)points[i].Y;
                }

                return new Vector2(total.X / (float)points.Count, total.Y / (float)points.Count);
            }
        }
        public Polygon(List<Vector2> POINTS)
        {
            for (int i = 0; i < POINTS.Count; i++)
            {
                Points.Add(new Vector2(POINTS[i].X, POINTS[i].Y));
            }
            BuildEdges();
        }
        public bool PointPolygon(Vector2 POINT)
        {
            int maxPoints = points.Count - 1;
            float totalAngle = GetAngle(points[maxPoints], POINT, points[0]);

            // Add the angles from the point
            // to each other pair of vertices.
            for (int i = 0; i < maxPoints; i++)
            {
                totalAngle += GetAngle(points[i], POINT, points[i + 1]);
            }

            // The total angle should be 2 * PI or -2 * PI if the point is in the polygon and close to zero if the point is outside the polygon.
            return (Math.Abs(totalAngle) > 1);
        }
        // Return the angle ABC. Return a value between PI and -PI. Note that the value is the opposite of what you might expect because Y coordinates increase downward.
        public static float GetAngle(Vector2 POINTA, Vector2 POINTB, Vector2 POINTC)
        {
            // Get the dot product.
            float dotProduct = DotProduct(POINTA, POINTB, POINTC);

            // Get the cross product.
            float crossProduct = CrossProductLength(POINTA, POINTB, POINTC);

            // Calculate the angle.
            return (float)Math.Atan2(crossProduct, dotProduct);
        }
        private static float DotProduct(Vector2 POINTA, Vector2 POINTB, Vector2 POINTC)
        {
            // Get the vectors' coordinates.
            float BAx = POINTA.X - POINTB.X;
            float BAy = POINTA.Y - POINTB.Y;
            float BCx = POINTC.X - POINTB.X;
            float BCy = POINTC.Y - POINTB.Y;

            // Calculate the dot product.
            return (BAx * BCx + BAy * BCy);
        }
        public static float CrossProductLength(Vector2 POINTA, Vector2 POINTB, Vector2 POINTC)
        {
            // Get the vectors' coordinates.
            float BAx = POINTA.X - POINTB.X;
            float BAy = POINTA.Y - POINTB.Y;
            float BCx = POINTC.X - POINTB.X;
            float BCy = POINTC.Y - POINTB.Y;

            // Calculate the Z coordinate of the cross product.
            return (BAx * BCy - BAy * BCx);
        }
        public void BuildEdges()
        {
            Vector2 point1;
            Vector2 point2;
            Edges.Clear();
            for (int i = 0; i < points.Count; i++)
            {
                point1 = Points[i];
                if (i + 1 >= points.Count)
                {
                    point2 = points[0];
                }
                else
                {
                    point2 = points[i + 1];
                }
                Edges.Add(point2 - point1);
            }
        }
        public override string ToString()
        {
            string result = "";

            for (int i = 0; i < Points.Count; i++)
            {
                if (result != "") result += " ";
                result += "{" + Points[i].ToString() + "}";
            }

            return result;
        }
        public void SetPolygon(List<Vector2> POINTS)
        {
            Points.Clear();

            for (int i = 0; i < POINTS.Count; i++)
            {
                Points.Add(new Vector2(POINTS[i].X, POINTS[i].Y));
            }
            BuildEdges();
        }
        public void Displace(Vector2 AMOUNT)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                points[i] += AMOUNT;
            }
            BuildEdges();
        }
        public List<Vector2> DisplacePoints(Vector2 AMOUNT)
        {
            List<Vector2> tempPoints = new List<Vector2>();

            for (int i = 0; i < Points.Count; i++)
            {
                tempPoints.Add(Points[i]);

                tempPoints[i] += AMOUNT;
            }
            return tempPoints;
        }
        */
    }
}
