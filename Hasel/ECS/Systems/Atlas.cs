#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Collections;
#endregion


namespace Hasel
{
    /// <summary>
    /// COLLISION DETECTION
    /// Atlas is responsible for detecting if a collision happens, working with colliders and shapes.
    /// </summary>
    public static class Atlas
    {
        public delegate bool CollisionCheck(Manifold MANIFOLD, Shape A, Shape B);
        public delegate bool PointPrimitiveCheck(Vector2 POINT, Shape SHAPE);
        public static CollisionCheck[,] Collisions;
        public static PointPrimitiveCheck[] PointPrimitives;

        public static void Initialise()
        {
            //0 is Circle
            //1 is Box
            //2 is Poly

            Collisions = new CollisionCheck[3,3];
            Collisions[0, 0] = new CollisionCheck(CircleCircle);
            Collisions[0, 1] = new CollisionCheck(CircleBox);
            Collisions[1, 0] = new CollisionCheck(BoxCircle);
            Collisions[1, 1] = new CollisionCheck(BoxBox);

            PointPrimitives = new PointPrimitiveCheck[3];
            PointPrimitives[0] = new PointPrimitiveCheck(PointCircle);
            PointPrimitives[1] = new PointPrimitiveCheck(PointBox);
            PointPrimitives[2] = new PointPrimitiveCheck(PointPolygon);
        }
        #region Point vs Primitives
        public static bool PointLine(Vector2 POINT, Line LINE)
        {
            float deltaY = LINE.End.Y - LINE.Start.Y;
            float deltaX = LINE.End.X - LINE.Start.X;
            float gradient = deltaY / deltaX;

            float yInt = LINE.End.Y - (gradient * LINE.End.X);

            return POINT.Y == (gradient * POINT.X) + yInt;
        }
        public static bool PointCircle(Vector2 POINT, Shape CIRCLE)
        {
            
            Vector2 circlePointDist = POINT - CIRCLE.Position;
            return circlePointDist.LengthSquared() <= CIRCLE.Radius * CIRCLE.Radius;
        }
        public static bool PointBox(Vector2 POINT, Shape BOX)
        {
            Box box = (Box)BOX;
            if (POINT.X < box.Left || POINT.X > box.Right) return false;
            if (POINT.Y < box.Top || POINT.Y > box.Bottom) return false;

            return true;
        }
        public static bool PointPolygon(Vector2 POINT, Shape POLYGON)
        {
            Polygon polygon = (Polygon)POLYGON;

            Vector2 point1 = polygon.Vertices[polygon.VertexCount - 1] - polygon.Vertices[0];
            Vector2 point2 = polygon.Vertices[1] - polygon.Vertices[0];
            Vector2 pointR = POINT - polygon.Vertices[0];

            if (!(point1.CrossProduct(pointR) <= 0 && point2.CrossProduct(pointR) >= 0)) return false;

            int left = 0, right = polygon.VertexCount;
            while (right - left > 1)
            {
                int mid = (left + right) / 2;
                Vector2 current = polygon.Vertices[mid] - POINT;
                if (current.CrossProduct(pointR) < 0)
                    right = mid;
                else
                    left = mid;
            }

            if (left == polygon.VertexCount - 1) {
                return Vector2.DistanceSquared(polygon.Vertices[0], POINT) <= Vector2.DistanceSquared(polygon.Vertices[0], polygon.Vertices[1]);
            } else {
                Vector2 pointL = polygon.Vertices[left + 1] - polygon.Vertices[left];
                Vector2 pointQ = POINT - polygon.Vertices[left];
                return pointL.CrossProduct(pointQ) >= 0;
            }
        }
        #endregion
        #region Checks
        public static bool BoxBox(Box BOXA, Box BOXB)
        {
            //Only do this if both are AABB (Rotation = 0)

            if (BOXA.Right < BOXB.Left || BOXA.Left > BOXB.Right) return false;
            if (BOXA.Bottom < BOXB.Top || BOXA.Top > BOXB.Bottom) return false;

            //No separating axis, therefore this is atleast one overlapping axis
            return true;
        }
        public static bool BoxCircle(Box BOX, Circle CIRCLE)
        {
            return false;
        }
        public static bool CircleCircle(Circle A, Circle B)
        {
            float radius = A.Radius + B.Radius;
            radius *= radius;
            return radius < A.Position.DistanceSquared(B.Position);
        }
        public static bool PolygonPolygon(Polygon POLYGONA, Polygon POLYONB)
        {
            //SAT
            return false;
        }
        #endregion
        #region Generate Manifolds
        public static bool CircleCircle(Manifold MANIFOLD, Shape A, Shape B)
        {
            //radius is sum radius of circles
            float radius = A.Radius + B.Radius;
            Vector2 normal = B.Position - A.Position;

            //Check if colliding, if not, exit
            if (normal.LengthSquared() > radius*radius)
                return false;

            //True distance is calculated once we know they are colliding
            float distance = normal.Length();

            //We want to make sure that we don't divide by zero
            //Distance is zero when they are on top of eachother.
            if (distance != 0)
            {
                MANIFOLD.Penetration = radius - distance;
                MANIFOLD.Normal = normal / distance;
            }
            else
            {
                MANIFOLD.Penetration = A.Radius;
                MANIFOLD.Normal = new Vector2(1, 0);
            }
            return true;
        }
        public static bool CircleBox(Manifold MANIFOLD, Shape CIRCLE, Shape BOX)
        {
            bool check = BoxCircle(MANIFOLD, BOX, CIRCLE);
            MANIFOLD.Normal = -MANIFOLD.Normal;
            return check;
        }
        public static bool BoxCircle(Manifold MANIFOLD, Shape BOX, Shape CIRCLE)
        {
            Box box = (Box)BOX;
            Circle circle = (Circle)CIRCLE;

            Vector2 n = circle.Position - box.Center;
            Vector2 closest = n;

            //Find the closest point to the circle
            closest.X = Math.Clamp(closest.X, -box.HalfDimensions.X, box.HalfDimensions.X);
            closest.Y = Math.Clamp(closest.Y, -box.HalfDimensions.Y, box.HalfDimensions.Y);

            bool inside = false;

            //If the closest point wasn't changed by being clamped within the box, then it was already inside
            if (n == closest)
            {
                inside = true;

                //Find closest axis
                if (Math.Abs(n.X) > Math.Abs(n.Y)) {
                    if (closest.X > 0)
                        closest.X = box.HalfDimensions.X;
                    else
                        closest.X = -box.HalfDimensions.X;
                } else {
                    if (closest.Y > 0)
                        closest.Y = box.HalfDimensions.Y;
                    else
                        closest.Y = -box.HalfDimensions.Y;
                }
            }

            Vector2 normal = n - closest;
            float distance = normal.LengthSquared();

            if (distance > (circle.Radius * circle.Radius) && !inside)
                return false;

            distance = (float)Math.Sqrt(distance);

            if (distance != 0)
            {
                if (inside)
                    MANIFOLD.Normal = -normal / distance;
                else
                    MANIFOLD.Normal = normal / distance;
                MANIFOLD.Penetration = circle.Radius - distance;
            }
            else
            {
                MANIFOLD.Penetration = circle.Radius;
                MANIFOLD.Normal = new Vector2(1, 0);
            }
            return true;
        }
        public static bool BoxBox(Manifold MANIFOLD, Shape BOXA, Shape BOXB) {
            Box A = (Box)BOXA;
            Box B = (Box)BOXB;

            Vector2 normal = B.Center - A.Center;

            float XOverlap = A.HalfDimensions.X + B.HalfDimensions.X - Math.Abs(normal.X);

            //SAT on X axis
            if (XOverlap > 0)
            {
                float YOverlap = A.HalfDimensions.Y + B.HalfDimensions.Y - Math.Abs(normal.Y);

                //SAT on Y axis 
                if (YOverlap > 0)
                {
                    //Find axis of least penetration
                    if (XOverlap < YOverlap)
                    {
                        if (normal.X < 0)
                            MANIFOLD.Normal = new Vector2(-1, 0);
                        else
                            MANIFOLD.Normal = new Vector2(1, 0);
                        MANIFOLD.Penetration = XOverlap;
                    }
                    else
                    {
                        if (normal.Y < 0)
                            MANIFOLD.Normal = new Vector2(0, -1);
                        else
                            MANIFOLD.Normal = new Vector2(0, 1);
                        MANIFOLD.Penetration = YOverlap;
                    }
                    return true;
                }
            }
            return false;
        }
        /*public static bool PolyPoly(Manifold MANIFOLD, Shape POLYA, Shape POLYB)
        {
            Polygon A = (Polygon)POLYA;
            Polygon B = (Polygon)POLYB;

            float penetrationA = FindAxisLeastPenetration(out int faceA, A, B);
            if (penetrationA >= 0) return false;

            float penetrationB = FindAxisLeastPenetration(out int faceB, B, A);
            if (penetrationB >= 0) return false;

            bool flip;
            int referenceIndex;

            Polygon RefPoly; //reference polygon
            Polygon IncPoly; //incident polygon

            if (Calc.gt(penetrationA, penetrationB))
            {
                RefPoly = B;
                IncPoly = A;
                flip = false;
                referenceIndex = faceA;
            }
            else
            {

                RefPoly = A;
                IncPoly = B;
                flip = true;
                referenceIndex = faceB;
            }

            FindIncidentFace(out List<Vector2> IncidentFace, referenceIndex, RefPoly, IncPoly);

            Vector2 v1 = RefPoly.Vertices[referenceIndex];
            referenceIndex = referenceIndex + 1 == RefPoly.VertexCount ? 0 : referenceIndex + 1;
            Vector2 v2 = RefPoly.Vertices[referenceIndex];

            Vector2 sidePlaneNormal = v2 - v1;
            sidePlaneNormal.Normalize();

            Vector2 refFaceNormal = new Vector2(sidePlaneNormal.Y, sidePlaneNormal.X);
            float refC = Vector2.Dot(refFaceNormal, v1);
            float negativeSide = -Vector2.Dot(sidePlaneNormal, v1);
            float positiveSide = Vector2.Dot(sidePlaneNormal, v2);

        } */
        public static float FindAxisLeastPenetration(out int FACEINDEX, Polygon A, Polygon B)
        {
            float bestDistance = -float.MaxValue;
            int bestIndex = 0;

            for (int i = 0; i < A.VertexCount; i++)
            {
                Vector2 nw = A.u * A.Normals[i];
                Matrix bMatrixTransposed = B.u.Transpose();
                Vector2 normal = bMatrixTransposed * nw;

                Vector2 supportPoint = B.GetSupport(-normal);
                Vector2 vertex = (bMatrixTransposed * A.Vertices[i]) + A.Position - B.Position;

                float penetrationDistance = Vector2.Dot(normal, supportPoint - vertex);

                if (penetrationDistance > bestDistance)
                {
                    bestDistance = penetrationDistance;
                    bestIndex = i;
                }
            }

            FACEINDEX = bestIndex;
            return bestDistance;
        }
        public static void FindIncidentFace(out List<Vector2> INCIDENTFACE, int REFERENCEINDEX, Polygon REFPOLY, Polygon INCPOLY)
        {

            Vector2 referenceNormal = REFPOLY.Normals[REFERENCEINDEX];
            referenceNormal = REFPOLY.u * referenceNormal;
            referenceNormal = INCPOLY.u.Transpose() * referenceNormal;

            int incidentFace = 0;
            float minDot = float.MaxValue;
            for (int i = 0; i < INCPOLY.VertexCount; i++)
            {
                float dot = Vector2.Dot(referenceNormal, INCPOLY.Normals[i]);

                if (dot < minDot)
                {
                    minDot = dot;
                    incidentFace = i;
                }
            }

            INCIDENTFACE = new List<Vector2>();
            INCIDENTFACE[0] = INCPOLY.Vertices[incidentFace];
            incidentFace = incidentFace + 1 >= (int)INCPOLY.VertexCount ? 0 : incidentFace + 1;
            INCIDENTFACE[1] = INCPOLY.Vertices[incidentFace];
        }
        public static void Clip(Vector2 NORMAL, float C, Vector2 FACE)
        {
            int sp = 0;

        }
        #endregion
        /* Old Method
        #region Methods
        public static PolygonCollisionData PolygonToPolygon(Polygon POLYGONA, Polygon POLYGONB, Vector2 VELOCITY)
        {
            PolygonCollisionData result = new PolygonCollisionData();
            result.intersects = true;
            result.willIntersect = true;

            int edgeCountA = POLYGONA.Edges.Count;
            int edgeCountB = POLYGONB.Edges.Count;
            float minIntervalDistance = float.PositiveInfinity;
            Vector2 translationAxis = new Vector2();
            Vector2 edge;

            // Loop through all the edges of both polygons
            for (int edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++)
            {
                if (edgeIndex < edgeCountA)
                {
                    edge = POLYGONA.Edges[edgeIndex];
                }
                else
                {
                    edge = POLYGONB.Edges[edgeIndex - edgeCountA];
                }

                // ===== 1. Find if the polygons are currently intersecting =====

                // Find the axis perpendicular to the current edge
                Vector2 axis = new Vector2(-edge.Y, edge.X);
                axis.Normalize();

                // Find the projection of the polygon on the current axis
                float minA = 0; float minB = 0; float maxA = 0; float maxB = 0;
                ProjectPolygon(axis, POLYGONA, ref minA, ref maxA);
                ProjectPolygon(axis, POLYGONB, ref minB, ref maxB);

                // Check if the polygon projections are currentlty intersecting
                if (IntervalDistance(minA, maxA, minB, maxB) > 0) result.intersects = false;

                // ===== 2. Now find if the polygons *will* intersect =====

                // Project the VELOCITY on the current axis
                float VELOCITYProjection = DotProduct(axis, VELOCITY);

                // Get the projection of polygon A during the movement
                if (VELOCITYProjection < 0)
                {
                    minA += VELOCITYProjection;
                }
                else
                {
                    maxA += VELOCITYProjection;
                }

                // Do the same test as above for the new projection
                float intervalDistance = IntervalDistance(minA, maxA, minB, maxB);
                if (intervalDistance > 0) result.willIntersect = false;

                // If the polygons are not intersecting and won't intersect, exit the loop
                if (!result.intersects && !result.willIntersect) break;

                // Check if the current interval distance is the minimum one. If so store
                // the interval distance and the current distance.
                // This will be used to calculate the minimum translation vector
                intervalDistance = Math.Abs(intervalDistance);
                if (intervalDistance < minIntervalDistance)
                {
                    minIntervalDistance = intervalDistance;
                    translationAxis = axis;

                    Vector2 d = POLYGONA.Center - POLYGONB.Center;
                    if (DotProduct(d, translationAxis) < 0)
                        translationAxis = -translationAxis;
                }
            }

            // The minimum translation vector
            // can be used to push the polygons appart.
            if (result.willIntersect) result.minimumDisplacement = translationAxis * minIntervalDistance;

            return result;
        }
        public static void ProjectPolygon(Vector2 axis, Polygon polygon, ref float min, ref float max)
        {
            float dotProduct = DotProduct(axis, polygon.Points[0]);
            min = dotProduct;
            max = dotProduct;

            for (int i = 0; i < polygon.Points.Count; i++)
            {
                dotProduct = DotProduct(polygon.Points[i], axis);
                if (dotProduct < min)
                {
                    min = dotProduct;
                }
                else
                {
                    if (dotProduct > max)
                    {
                        max = dotProduct;
                    }
                }
            }
        }
        public static float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }
            else
            {
                return minA - maxB;
            }
        }
        public static float DotProduct(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
        public static float DistanceTo(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }
        public static Vector2 Normalise(Vector2 point)
        {
            float magnitude = (float)Math.Sqrt(point.X * point.X + point.Y * point.Y);
            return new Vector2(point.X / magnitude, point.Y / magnitude);
        }
        #endregion
        #region Structs
        public struct PolygonCollisionData
        {
            public bool intersects;
            public bool willIntersect;
            public Vector2 minimumDisplacement;
        }
        #endregion
        */
    }
}
