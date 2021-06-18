using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hasel
{
    public class Manifold
    {
        public Entity A;
        public Entity B;

        public float Penetration;
        public Vector2 Normal;
        public Atlas.CollisionCheck CollisionCheck;
        public List<Vector2> Contacts;
        public float Restitution, StaticFriction, KineticFriction;

        public Manifold(Entity A, Entity B)
        {
            this.A = A;
            this.B = B;
        }
        
        public bool Solve()
        {
            Collider cA = A.Get<Collider>();
            Collider cB = B.Get<Collider>();
            CollisionCheck = Atlas.Collisions[(int)cA.Shape.Type, (int)cB.Shape.Type];
            return CollisionCheck.Invoke(this, cA.Shape, cB.Shape);
        }
        public void Initialise()
        {
            Material mA = A.Get<Material>() ?? Nomad.DefaultMaterial;
            Material mB = B.Get<Material>() ?? Nomad.DefaultMaterial;
            Dynamo dA = A.Get<Dynamo>() ?? Nomad.DefaultDynamo;
            Dynamo dB = B.Get<Dynamo>() ?? Nomad.DefaultDynamo;

            Restitution = Math.Min(mA.Restitution, mB.Restitution);

            StaticFriction = (float)Math.Sqrt(mA.StaticFriction * mA.StaticFriction + mB.StaticFriction * mB.StaticFriction);
            KineticFriction = (float)Math.Sqrt(mA.KineticFriction * mA.KineticFriction + mB.KineticFriction * mB.KineticFriction);

            if ((dB.Velocity - dA.Velocity).LengthSquared() < Nomad.Resting)
            {
                Restitution = 0;
            }
        }
        public void ApplyImpulse()
        {
            Body bA = A.Get<Body>() ?? Nomad.DefaultBody;
            Body bB = B.Get<Body>() ?? Nomad.DefaultBody;
            Dynamo dA = A.Get<Dynamo>() ?? Nomad.DefaultDynamo;
            Dynamo dB = B.Get<Dynamo>() ?? Nomad.DefaultDynamo;
            //Calculate Relative Velocity
            Vector2 RelativeVelocity = dB.Velocity - dA.Velocity;

            //Calculate Relative Velocity along normal.
            float VelocityAlongNormal = Vector2.Dot(RelativeVelocity, Normal);

            if (VelocityAlongNormal > 0)
                return;

            //Calculate Impulse Scalar
            float ImpulseScalar = -(1 + Restitution) * VelocityAlongNormal;
            ImpulseScalar /= bA.InvMass + bB.InvMass;

            //Apply Collision Impulse
            Vector2 CollisionImpulse = ImpulseScalar * Normal;
            dA.Velocity -= bA.InvMass * CollisionImpulse;
            dB.Velocity += bB.InvMass * CollisionImpulse;

            Vector2 Tangent = RelativeVelocity - (Normal * Vector2.Dot(RelativeVelocity, Normal));

            if (Tangent == Vector2.Zero)
                return;
            Tangent.Normalize();

            //The friction magnitude along the collision tangent.
            float FrictionMagnitude = -Vector2.Dot(RelativeVelocity, Tangent);
            FrictionMagnitude /= bA.InvMass + bB.InvMass;

            if (Math.Abs(FrictionMagnitude) <= 0.001f)
                return;

            Vector2 FrictionImpulse;
            if (Math.Abs(FrictionMagnitude) < ImpulseScalar * StaticFriction)
                FrictionImpulse = Tangent * FrictionMagnitude;
            else
                FrictionImpulse = Tangent * -ImpulseScalar * KineticFriction;

            dA.Velocity -= bA.InvMass * FrictionImpulse;
            dB.Velocity += bB.InvMass * FrictionImpulse;
        }
        public void PositionalCorrection()
        {
            Body bA = A.Get<Body>() ?? Nomad.DefaultBody;
            Body bB = B.Get<Body>() ?? Nomad.DefaultBody;
            Transform tA = A.Get<Transform>() ?? Nomad.DefaultTransform;
            Transform tB = B.Get<Transform>() ?? Nomad.DefaultTransform;

            const float Percent = 0.2f;
            //Vector2 Correction = PenetrationDepth / (A.InvMass + B.InvMass) * Percent * Vector2.One;
            Vector2 Correction = Math.Max(Penetration - Nomad.PenetrationAllowance, 0.0f) / (bA.InvMass + bB.InvMass) * Percent * Normal;
            tA.Position -= bA.InvMass * Correction;
            tB.Position += bB.InvMass * Correction;
        }
    }
}
