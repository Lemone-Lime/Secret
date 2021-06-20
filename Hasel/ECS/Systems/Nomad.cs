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
#endregion


namespace Hasel
{
    /// <summary>
    /// Physics -> Movement & Collision Response
    /// </summary>
    public static class Nomad
    {
        public const float PenetrationAllowance = 0.05f;
        public const int Iterations = 10;

        public static Vector2 Gravity = new Vector2(0, 98f);
        public static bool UseGravity = true;
        public static float Resting = (Gravity * Engine.FixedDeltaTime).LengthSquared() + Calc.Epsilon;


        public static List<Manifold> Contacts;

        public static Body DefaultBody;
        public static Material DefaultMaterial;
        public static Dynamo DefaultDynamo;
        public static Transform DefaultTransform;

        public static void Initialise()
        {
            Contacts = new List<Manifold>();

            DefaultBody = new Body();
            DefaultDynamo = new Dynamo();
            DefaultMaterial = new Material();
            DefaultTransform = new Transform();
        }
        public static void EnactPhysics()
        {
            Contacts.Clear();

            List<Body> Bodies = Engine.Scene.Forge.ComponentList<Body>();
            List<Entity> Temp = Engine.Scene.Forge.GetEntitiesByMask(new Mask(new List<Type>() { typeof(Collider) }));

            //Generate new collision info
            //Collider, Body, Material
            for (int i = 0; i < Temp.Count; i++)
            {
                Entity A = Temp[i];
                for (int j = i + 1; j < Temp.Count; j++)
                {
                    Entity B = Temp[j];

                    Manifold MANIFOLD = new Manifold(A, B);
                    if (MANIFOLD.Solve())
                    {
                        Contacts.Add(MANIFOLD);
                    }
                }
            }

            Temp = Engine.Scene.Forge.GetEntitiesByMask(new Mask(new List<Type>() { typeof(Body), typeof(Dynamo) }));

            //Integrate Forces
            foreach (var entity in Temp)
            {
                IntegrateForces(entity);
            }

            //Initialise Collision
            foreach (var contact in Contacts)
            {
                contact.Initialise();
            }
             
            //Resolve Collisions
            for (int i = 0; i < Iterations; i++)
            {
                foreach (var contact in Contacts) {
                    contact.ApplyImpulse();
                }
            }

            List<Entity> Move = Engine.Scene.Forge.GetEntitiesByMask(new Mask(new List<Type>() { typeof(Dynamo), typeof(Transform) }));

            //Integrate Velocities
            foreach (var entity in Move)
            {
                IntegrateVelocity(entity);
            }

            foreach (var entity in Temp)
            {
                IntegrateForces(entity);
            }

            //Correct Positions
            foreach (var contact in Contacts)
            {
                contact.PositionalCorrection();
            }

            //Reset Forces
            foreach (var body in Bodies)
            {
                body.Force = Vector2.Zero;
                body.Torque = 0;
            }
        }
        public static void IntegrateForces(Entity ENTITY)
        {
            Body body = ENTITY.Get<Body>();
            Dynamo dynamo = ENTITY.Get<Dynamo>();

            dynamo.Velocity += body.InvMass * body.Force * Engine.FixedDeltaTime * 0.5f;
            dynamo.Velocity += Gravity * Engine.FixedDeltaTime * Convert.ToInt32(UseGravity) * 0.5f;
            dynamo.AngularVelocity += body.Torque * body.InvInertia * Engine.FixedDeltaTime * 0.5f;
        }
        public static void IntegrateVelocity(Entity ENTITY)
        {
            Transform transform = ENTITY.Get<Transform>();
            Dynamo dynamo = ENTITY.Get<Dynamo>();

            transform.Position += dynamo.Velocity * Engine.FixedDeltaTime;
            transform.Orientation += dynamo.AngularVelocity * Engine.FixedDeltaTime;
        }
    }
}