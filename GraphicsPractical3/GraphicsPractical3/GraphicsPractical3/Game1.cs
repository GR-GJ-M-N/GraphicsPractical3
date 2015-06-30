using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GraphicsPractical3
{
    public enum Scenes
    {
        Normal, White, E1, E3, E5, E6, M4
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Often used XNA objects
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private FrameRateCounter frameRateCounter;

        // Game objects and variables
        private Camera camera;

        // Stuff for switching scenes
        private bool sceneHasChanged = true;
        private Scenes sceneState = Scenes.Normal;
        private Scene scene;
        private KeyboardState kState;
        private MouseState mouseState;

        //Text
        private SpriteFont font;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            // Create and add a frame rate counter
            this.frameRateCounter = new FrameRateCounter(this);
            this.Components.Add(this.frameRateCounter);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Copy over the device's rasterizer state to change the current fillMode
            this.GraphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.None };
            // Set up the window
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;
            this.graphics.IsFullScreen = false;
            // Let the renderer draw and update as often as possible
            this.graphics.SynchronizeWithVerticalRetrace = true;
            this.IsFixedTimeStep = false;
            // Flush the changes to the device parameters to the graphics card
            this.graphics.ApplyChanges();
            this.kState = Keyboard.GetState();
            this.mouseState = Mouse.GetState();
            // Initialize the camera
            this.camera = new Camera(new Vector3(0, 0.5f, 2), new Vector3(0, 0, -1), new Vector3(0, 1, 0));

            //Set mouse for mouselook
            this.IsMouseVisible = false;
            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            this.mouseState = Mouse.GetState();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.font = this.Content.Load<SpriteFont>("Font");
            //Effect effect = this.Content.Load<Effect>("Effects/Effect1");
            //// Load the model and let it use the "Simple" effect
            //this.model = this.Content.Load<Model>("Models/bunny");
            //this.model.Meshes[0].MeshParts[0].Effect = effect;
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float timeStep = (float)gameTime.ElapsedGameTime.TotalSeconds * 60.0f;

            // Update the window title
            this.Window.Title = "XNA Renderer | FPS: " + this.frameRateCounter.FrameRate;

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (this.kState.IsKeyUp(Keys.Space))
                    this.sceneHasChanged = true;
            }

            //Camera movement stuff
            //this.UpdateCamera(timeStep);

            this.kState = Keyboard.GetState();

            //If scene has changed, unload everything and load everything needed for the particular scene
            if (this.sceneHasChanged)
            {
                //Reset everything
                //TODO: RESET
                this.scene = null;
                //Do switching
                switch (this.sceneState)
                {
                    case Scenes.Normal:
                        this.sceneState = Scenes.White;
                        break;
                    case Scenes.White:
                        this.sceneState = Scenes.E1;
                        break;
                    case Scenes.E1:
                        this.sceneState = Scenes.E3;
                        break;
                    case Scenes.E3:
                        this.sceneState = Scenes.E5;
                        break;
                    case Scenes.E5:
                        this.sceneState = Scenes.E6;
                        break;
                    case Scenes.E6:
                        this.sceneState = Scenes.M4;
                        break;
                    case Scenes.M4:
                        this.sceneState = Scenes.Normal;
                        break;
                }

                switch (this.sceneState)
                {
                    case Scenes.Normal:
                        this.scene = new NormalScene();
                        NormalScene normalScene = (NormalScene)this.scene;

                        normalScene.effect = this.Content.Load<Effect>("Effects/Effect1");
                        normalScene.model = this.Content.Load<Model>("Models/bunny");
                        normalScene.model.Meshes[0].MeshParts[0].Effect = normalScene.effect;
                        break;
                    case Scenes.White:
                        this.scene = new WhiteScene();
                        WhiteScene whiteScene = (WhiteScene)this.scene;

                        whiteScene.effect = this.Content.Load<Effect>("Effects/Effect1");
                        whiteScene.model = this.Content.Load<Model>("Models/bunny");
                        whiteScene.model.Meshes[0].MeshParts[0].Effect = whiteScene.effect;
                        break;
                    case Scenes.E1:
                        this.scene = new E1Scene();
                        break;
                    case Scenes.E3:
                        this.scene = new E3Scene();
                        E3Scene e3Scene = (E3Scene)this.scene;

                        e3Scene.effect = this.Content.Load<Effect>("Effects/CellShading");
                        e3Scene.model = this.Content.Load<Model>("Models/bunny");
                        e3Scene.model.Meshes[0].MeshParts[0].Effect = e3Scene.effect;
                        break;
                    case Scenes.E5:
                        this.scene = new E5Scene();
                        E5Scene e5Scene = (E5Scene)this.scene;
                        e5Scene.effect = this.Content.Load<Effect>("Effects/Effect1");
                        e5Scene.model = this.Content.Load<Model>("Models/bunny");
                        e5Scene.model.Meshes[0].MeshParts[0].Effect = e5Scene.effect;
                        e5Scene.target = new RenderTarget2D(graphics.GraphicsDevice, 800, 600);
                        break;

                    case Scenes.E6:
                        this.scene = new E6Scene();
                        E6Scene e6Scene = (E6Scene)this.scene;
                        e6Scene.effect = this.Content.Load<Effect>("Effects/Effect1");
                        e6Scene.model = this.Content.Load<Model>("Models/bunny");
                        e6Scene.model.Meshes[0].MeshParts[0].Effect = e6Scene.effect;
                        e6Scene.target = new RenderTarget2D(graphics.GraphicsDevice, 800, 600);
                        break;
                    case Scenes.M4:
                        this.scene = new M4Scene();
                        M4Scene m4Scene = (M4Scene)this.scene;
                        m4Scene.effect = this.Content.Load<Effect>("Effects/M5");
                        m4Scene.model = this.Content.Load<Model>("Models/teapot");
                        m4Scene.texture = this.Content.Load<Texture2D>("Textures/uffizi_cross");
                        m4Scene.model.Meshes[0].MeshParts[0].Effect = m4Scene.effect;
                        break;
                }
                this.sceneHasChanged = false;
            }

            base.Update(gameTime);
        }

        void UpdateCamera(float timeStep)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                //Translate camera in direction of Eye
                Vector3 focusVector = Vector3.Normalize(this.camera.Focus - this.camera.Eye);
                float translationFactor = timeStep * 0.025f;
                focusVector = translationFactor * focusVector;
                this.camera.Focus += focusVector;
                this.camera.Eye = this.camera.Eye + focusVector;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                //Translate camera in opposite direction of Eye
                Vector3 focusVector = Vector3.Normalize(this.camera.Focus - this.camera.Eye);
                float translationFactor = timeStep * 0.025f;
                focusVector = translationFactor * focusVector;
                this.camera.Focus -= focusVector;
                this.camera.Eye = this.camera.Eye - focusVector;
            }

            MouseState newState = Mouse.GetState();

            if (newState != this.mouseState)
            {
                float dX = newState.X - mouseState.X;
                float dY = newState.Y - mouseState.Y;
                float horRot = dX * -0.003f;
                float verRot = dY * -0.003f;

                Matrix horRotMatrix = Matrix.CreateRotationY(horRot);
                Matrix verRotMatrix = Matrix.CreateRotationX(verRot);
                this.camera.Focus = Vector3.Transform(this.camera.Focus - this.camera.Eye, horRotMatrix) + this.camera.Eye;
                this.camera.Focus = Vector3.Transform(this.camera.Focus - this.camera.Eye, verRotMatrix) + this.camera.Eye;
            }
            //Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            this.mouseState = newState;

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //E5 begin
            //E5RenderTarget2D = new RenderTarget2D(graphics.GraphicsDevice, 800, 600);
            //graphics.GraphicsDevice.SetRenderTarget(E5RenderTarget2D);
            //E5 einde
            GraphicsDevice.Clear(Color.DeepSkyBlue);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            

            switch (this.sceneState)
            {
                case Scenes.Normal:
                    NormalScene normalScene = (NormalScene)this.scene;

                    normalScene.world = Matrix.CreateScale(3.0f);
                    normalScene.camera = this.camera;
                    normalScene.Draw();
                    break;
                case Scenes.White:
                    WhiteScene whiteScene = (WhiteScene)this.scene;
                    whiteScene.camera = this.camera;
                    whiteScene.world = Matrix.CreateScale(3.0f);

                    whiteScene.Draw();
                    break;
                case Scenes.E1:
                    //...
                    break;
                case Scenes.E5:
                    E5Scene e5Scene = (E5Scene)this.scene;
                    e5Scene.world = Matrix.CreateScale(3.0f);
                    e5Scene.camera = this.camera;
                    
                    e5Scene.DrawBefore(graphics);
                    break;
                case Scenes.E6:
                    E6Scene e6Scene = (E6Scene)this.scene;
                    e6Scene.world = Matrix.CreateScale(3.0f);
                    e6Scene.camera = this.camera;

                    e6Scene.DrawBefore(graphics);
                    break;
                case Scenes.E3:
                    E3Scene e3Scene = (E3Scene)this.scene;

                    e3Scene.world = Matrix.CreateScale(3.0f);
                    e3Scene.camera = this.camera;
                    e3Scene.Draw();
                    break;
                case Scenes.M4:
                    M4Scene m4Scene = (M4Scene)this.scene;

                    m4Scene.world = Matrix.CreateScale(0.25f);
                    m4Scene.camera = this.camera;
                    m4Scene.Draw();
                    break;
            }

            this.spriteBatch.Begin();
            this.spriteBatch.DrawString(this.font, this.scene.name, new Vector2(10, 10), Color.White);
            this.spriteBatch.End();
            base.Draw(gameTime);

            if (sceneState == Scenes.E5) 
            {
                ((E5Scene)this.scene).DrawAfter(graphics, spriteBatch);
            } else if (sceneState == Scenes.E6)
            {
                ((E6Scene)this.scene).DrawAfter(graphics, spriteBatch);
            }
        }
    }
}
