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
#endregion
using System.Runtime;
using System.Diagnostics;

namespace Hasel
{
    public class Engine : Game
    {
        //Properties
        public string Title;
        public Version Version;
        protected Color ClearColour;
        public static int Width { get; protected set; }
        public static int Height { get; protected set; }

        //Instance
        public static Engine Instance { get; private set; }

        //fps
        private int fpsCounter = 0;
        public static int FPS;
        private TimeSpan counterElapsed = TimeSpan.Zero;


        //fixed time step
        public static float DeltaTime;
        private float Accumulator = 0f;
        public const float FPSAim = 60;
        public const float FixedDeltaTime = 1/FPSAim;
        public static float Time = 0f;

        public static float Alpha;

        //Scene
        private Scene scene;
        private Scene nextScene;

        public static Scene Scene
        {
            get { return Instance.scene; }
            set { Instance.nextScene = value; }
        }

        public Engine(int WIDTH, int HEIGHT, string TITLE, Color CLEARCOLOUR, bool FULLSCREEN)
        {
            Instance = this;

            Width = WIDTH;
            Height = HEIGHT;
            Title = TITLE;
            ClearColour = CLEARCOLOUR;

            Globals.Graphics = new GraphicsDeviceManager(this);

            if (FULLSCREEN)
            {
                Globals.Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                Globals.Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                Globals.Graphics.IsFullScreen = true;
            }
            else
            {
                Globals.Graphics.PreferredBackBufferWidth = Width;
                Globals.Graphics.PreferredBackBufferHeight = Height;
                Globals.Graphics.IsFullScreen = false;
            }

            Globals.Graphics.SynchronizeWithVerticalRetrace = false;
            IsMouseVisible = true;
            IsFixedTimeStep = false;

            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Globals.Content = this.Content;
            Globals.Content.RootDirectory = @"Content";
            Globals.Batch = new SpriteBatch(GraphicsDevice);

            Limn.Initialise();
            Nomad.Initialise();
            Atlas.Initialise();
            Scoop.Initialise();
            Forge.Initialise();
        }

        protected override void Update(GameTime gameTime)
        {
            Scoop.Update();

            if (Scoop.Keyboard.Check(Keys.Escape))
                Exit();

            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (DeltaTime > FixedDeltaTime)
                DeltaTime = FixedDeltaTime;

            Accumulator += DeltaTime;



            while (Accumulator > FixedDeltaTime)
            {
                FixedUpdate(gameTime);
                Time += FixedDeltaTime;
                Accumulator -= FixedDeltaTime;
            }

            Alpha = Accumulator / FixedDeltaTime;


            if (scene != null)
            {
                Scene.Update();
            }

            if (scene != nextScene)
            {
                var lastScene = scene;
                if (scene != null)
                    scene.End();
                scene = nextScene;

                //do transitionnnnn
                if (scene != null)
                    scene.Begin();
            }

            //add update shit here

            base.Update(gameTime);
        }

        protected virtual void FixedUpdate(GameTime gameTime)
        {
            Nomad.EnactPhysics();
            Scene.FixedUpdate();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            
            //Render Core
            GraphicsDevice.Clear(ClearColour);

            if (Scene != null)
            {
                Limn.Begin();
                Scene.Render();
                Limn.End();
            }


            //Frame Counter
            fpsCounter++;
            counterElapsed += gameTime.ElapsedGameTime;

            if (counterElapsed >= TimeSpan.FromSeconds(1))
            {
                Window.Title = Title + " " + fpsCounter.ToString() + " fps - " + (GC.GetTotalMemory(false) / 1048576f).ToString("F") + " MB";

                FPS = fpsCounter;
                fpsCounter = 0;
                counterElapsed -= TimeSpan.FromSeconds(1);
            }
        }
    }
}
