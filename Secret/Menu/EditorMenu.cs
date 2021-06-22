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

using System.Collections;
using Hasel;
using System.Diagnostics;
#endregion

namespace Secret
{
    public class EditorMenu : Menu
    {
        HInterface InfoText, AboutText;
        Dropdown New, Help;
        Toggle Shape;
        Button Exit, Restart, PlaceBox, PlaceCircle, Info, About;
        Slider Density, Restitution;
        Window InfoScreen, AboutScreen;
        int size = 30;

        Entity DragEntity;
        bool ShapePressed;
        Vector2 Drag, StartDrag;

        public EditorMenu() : base(Vector2.Zero, new HTexture(Engine.Width, 24, new Color(10, 10, 10, 100)), null)
        {
            New = new Dropdown(new Vector2(0, 0), new HTexture(160, 24, new Color(10, 10, 10, 100)), new HText("Hack", "New", new Vector2(10, 5)));
            New.HighlightTarget = 0.03f;
            New.TextLeanTarget = new Vector2(7, 0);

            Help = new Dropdown(new Vector2(Engine.Width-96, 0), new HTexture(96, 24, new Color(10, 10, 10, 100)), new HText("Hack", "Help", new Vector2(10, 5)));
            Help.HighlightTarget = 0.03f;
            Help.TextLeanTarget = new Vector2(7, 0);

            About = new Button(new Vector2(-64, 0), new HTexture(160, 24, new Color(10, 10, 10, 100)), new HText("Hack", "About", new Vector2(10, 5)), OpenAboutScreen);
            About.HighlightTarget = 0.03f;
            About.TextLeanTarget = new Vector2(7, 0);
            Help.Add(About);

            Info = new Button(new Vector2(-64, 0), new HTexture(160, 24, new Color(10, 10, 10, 100)), new HText("Hack", "Instructions", new Vector2(10, 5)), OpenInfoScreen);
            Info.HighlightTarget = 0.03f;
            Info.TextLeanTarget = new Vector2(7, 0);
            Help.Add(Info);

            InfoScreen = new Window(new Vector2(100, 50), new HTexture(480, 320, new Color(10, 10, 10, 255)), new HText("Hack", "Instructions", new Vector2(10, 5), Color.White, 0.12f, 480), 24);
            InfoScreen.Visible = false;

            InfoText = new HInterface(new Vector2(24, 0), null, new HText(
                "Hack", "Start by using the [WASD] keys to move the player box around. " +
                "You can also press the [Z] key to teleport the player box to your mouse cursor. You can place other boxes and circles by " +
                "pressing the \"New\" dropdown menu, pressing \"Shape\" and then selecting your choice of \"Box\" or \"Circle\". Now, by " +
                "left clicking on the canvas, a box or circle appears at your mouse cursor. Also in the \"New\" dropdown menu, are sliders " +
                "for the Density and Restitution of newly created boxes and circles. Density impacts how heavy an object is, and is represented " +
                "by low density objects being more transparent, and high density objects being opaque. Restitution is how bouncy an object is, " +
                "and is represented by low restitution objects being blue, becoming purple and then red as restitution increases. To change the size " +
                "of newly - created objects, use the scroll wheel. There is a visual in the bottom left of the screen which indicates the size of " +
                "newly-created objects. Finally, by clicking and dragging on objects, you can shoot them into eachother. Enjoy!", null, Color.White, 0.12f, 480));
            InfoText.ShowArrow = false;
            InfoScreen.Add(InfoText);


            AboutScreen = new Window(new Vector2(100, 50), new HTexture(480, 320, new Color(10, 10, 10, 255)), new HText("Hack", "About this Project", new Vector2(10, 5), Color.White, 0.12f, 480), 24);
            AboutScreen.Visible = false;

            AboutText = new HInterface(new Vector2(24, 0), null, new HText(
                "Hack", "Secret is a physics engine developed by Liam Hall as a school project. It still is to be further developed, with the " +
                "vision of the project being a multiplayer fighting game. The purpose of the project is to create a realistic physics engine with " +
                "crisp collisions. The UI has been designed to test different parameters of the physics engine, such as size, density, restitution, " +
                "and shape. In the near future, there will be polygons that you can collide together as well as angular velocity. The balls will roll.", null, Color.White, 0.12f, 480));
            AboutText.ShowArrow = false;
            AboutScreen.Add(AboutText);

            Restart = new Button(new Vector2(160, 0), new HTexture(160, 24, new Color(10, 10, 10, 100)), new HText("Hack", "Restart (\"R\")", new Vector2(10, 5)), RestartScene);
            Restart.HighlightTarget = 0.03f;
            Restart.TextLeanTarget = new Vector2(7, 0);

            Exit = new Button(new Vector2(320, 0), new HTexture(160, 24, new Color(10, 10, 10, 100)), new HText("Hack", "Exit (\"Esc\")", new Vector2(10, 5)), Engine.Instance.Exit);
            Exit.HighlightTarget = 0.03f;
            Exit.TextLeanTarget = new Vector2(7, 0);

            Shape = new Toggle(Vector2.Zero, new HTexture(160, 24, new Color(10, 10, 10, 100)), new HText("Hack", "Shape", new Vector2(10, 5)), new Vector2(160, 0));
            Shape.HighlightTarget = 0.03f;
            Shape.TextLeanTarget = new Vector2(7, 0);
            New.Add(Shape);

            PlaceBox = new Button(Vector2.Zero, new HTexture(96, 24, new Color(10, 10, 10, 100)), new HText("Hack", "Box", new Vector2(10, 5)));
            PlaceBox.HighlightTarget = 0.03f;
            PlaceBox.TextLeanTarget = new Vector2(7, 0);
            PlaceBox.Visible = false;
            Shape.Add(PlaceBox);

            PlaceCircle = new Button(Vector2.Zero, new HTexture(96, 24, new Color(10, 10, 10, 100)), new HText("Hack", "Circle", new Vector2(10, 5)));
            PlaceCircle.HighlightTarget = 0.03f;
            PlaceCircle.TextLeanTarget = new Vector2(7, 0);
            PlaceCircle.Visible = false;
            Shape.Add(PlaceCircle);

            Density = new Slider(Vector2.Zero, new HTexture(160, 32, new Color(10, 10, 10, 100)), new HText("Hack", "Density", new Vector2(10, 5)), new Vector2(30, 26), 100, 3);
            Density.LineColor = Color.White;
            Density.HighlightTarget = 0.03f;
            Density.TextLeanTarget = new Vector2(7, 0);
            Density.Visible = false;
            Density.CircleRadius = 3;
            Density.CircleThickness = 5;
            New.Add(Density);

            Restitution = new Slider(Vector2.Zero, new HTexture(160, 32, new Color(10, 10, 10, 100)), new HText("Hack", "Restitution", new Vector2(10, 5)), new Vector2(30, 26), 100, 3);
            Restitution.LineColor = Color.White;
            Restitution.HighlightTarget = 0.03f;
            Restitution.TextLeanTarget = new Vector2(7, 0);
            Restitution.Visible = false;
            Restitution.CircleRadius = 3;
            Restitution.CircleThickness = 5;
            New.Add(Restitution);

            Items.Add(New);
            Items.Add(Restart);
            Items.Add(Exit);
            Items.Add(Help);
            Items.Add(InfoScreen);
            Items.Add(AboutScreen);
        }
        public override void Update()
        {
            base.Update();

            foreach (var Entity in Engine.Scene.Forge.GetEntitiesByMask(new Mask(new List<Type>() { typeof(Collider), typeof(Dynamo), typeof(Body) })))
            {
                Shape Shape = Entity.Get<Collider>().Shape;
                if (Atlas.PointPrimitives[Convert.ToInt16(Shape.Type)](Scoop.Mouse.Position, Shape) && Scoop.Mouse.PressedLeft)
                {
                    ShapePressed = true;
                    DragEntity = Entity;
                    Engine.TimeRate = 0.6f;
                    Drag = Vector2.Zero;
                    StartDrag = Scoop.Mouse.Position;
                }
            }

            if (ShapePressed && Scoop.Mouse.CheckLeft)
            {
                Drag += Scoop.Mouse.PositionDelta;
                Engine.TimeRate = Calc.Approach(Engine.TimeRate, 0.1f, 0.001f);
            }
            if (ShapePressed && Scoop.Mouse.ReleasedLeftButton)
            {
                ShapePressed = false;
                DragEntity.Get<Body>().Force -= Drag * 10000;
                Drag = Vector2.Zero;
                Engine.TimeRate = 1;
            }

            if (Scoop.Mouse.PressedLeft && !Engine.Instance.MouseHoveringUI && !ShapePressed)
            {
                if (Shape.Selected == PlaceCircle) PhysicsCircle();
                if (Shape.Selected == PlaceBox) PhysicsBox();
            }

            if (Scoop.Keyboard.Pressed(Keys.R)) RestartScene();

            size = Math.Clamp(size + Math.Sign(Scoop.Mouse.WheelDelta), 10, 100);
        }

        public override void Render()
        {
            base.Render();

            Limn.HollowRectangle(Engine.Width - size, Engine.Height - size, size, size, Color.Yellow);
            Limn.Circle(new Vector2(Engine.Width - size / 2, Engine.Height - size / 2), size / 2, Color.Yellow, 5);

            if (Drag != Vector2.Zero) Limn.Line(StartDrag, StartDrag + Drag, Drag.Length() * 0.01f, Color.Yellow);
        }

        public void PhysicsCircle()
        {
            PhysicsEntity(new Circle(Vector2.Zero, 0.5f * size));
        }

        public void PhysicsBox()
        {
            PhysicsEntity(new Box(new Vector2(-size / 2, -size / 2), new Vector2(size, size)));
        }

        public void PhysicsEntity(Shape SHAPE)
        {
            PhysicsEntity physicsEntity = new PhysicsEntity();
            physicsEntity.Added(Engine.Scene);
            physicsEntity.Get<Transform>().Position = Scoop.Mouse.Position;
            physicsEntity.Get<Material>().Restitution = Calc.Lerp(0.1f, 0.9f, Restitution.Value);
            physicsEntity.Get<Material>().Density = Calc.Lerp(0.01f, 0.4f, Density.Value);

            physicsEntity.SetShape(SHAPE);
        }

        public void RestartScene()
        {
            Engine.Scene = new CollisionsScene();
        }

        public void OpenAboutScreen()
        {
            InfoScreen.Visible = false;
            AboutScreen.Visible = true;
        }
        public void OpenInfoScreen()
        {
            InfoScreen.Visible = true;
            AboutScreen.Visible = false;
        }
    }
}
