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
        public static float RawDeltaTime;
        public static float DeltaTime;
        public static float TimeRate = 1f;
        private float Accumulator = 0f;
        public static float FPSAim = 60;
        public static float FixedDeltaTime {
            get { return 1 / FPSAim; }
        }
        public static float Time = 0f;

        public static float Alpha;

        //Scene
        private Scene scene;
        private Scene nextScene;

        //Menu
        private Menu menu;

        public static Menu Menu
        {
            get { return Instance.menu; }
            set { Instance.menu = value; }
        }

        public static Scene Scene
        {
            get { return Instance.scene; }
            set { Instance.nextScene = value; }
        }

        public bool MouseHoveringUI = false;

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

        protected override void Update(GameTime GAMETIME)
        {
            Scoop.Update();

            if (Scoop.Keyboard.Check(Keys.Escape))
                Exit();

            RawDeltaTime = (float)GAMETIME.ElapsedGameTime.TotalSeconds;
            DeltaTime = RawDeltaTime * TimeRate;
            if (DeltaTime > FixedDeltaTime)
                DeltaTime = FixedDeltaTime;

            Accumulator += DeltaTime;

            while (Accumulator > FixedDeltaTime)
            {
                FixedUpdate(GAMETIME);
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
                //var lastScene = scene;
                if (scene != null)
                    scene.End();
                scene = nextScene;

                //do transitionnnnn
                if (scene != null)
                    scene.Begin();
            }

            //add update shit here

            MouseHoveringUI = false;

            Menu.Update();

            base.Update(GAMETIME);
        }

        protected virtual void FixedUpdate(GameTime GAMETIME)
        {
            Nomad.EnactPhysics();
            Scene.FixedUpdate();
        }

        protected override void Draw(GameTime GAMETIME)
        {
            base.Draw(GAMETIME);
            
            //Render Core
            GraphicsDevice.Clear(ClearColour);

            if (Scene != null)
            {
                Limn.Begin();
                Scene.Render();
                Menu.Render();
                Limn.End();
            }


            //Frame Counter
            fpsCounter++;
            counterElapsed += GAMETIME.ElapsedGameTime;

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
