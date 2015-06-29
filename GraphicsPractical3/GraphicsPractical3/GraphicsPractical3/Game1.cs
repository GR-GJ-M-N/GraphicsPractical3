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
        Normal, White
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
        private Model model;

        // Stuff for switching scenes
        private bool sceneHasChanged = true;
        private Scenes sceneState = Scenes.Normal;
        private Scene scene;
        private KeyboardState kState;

        RenderTarget2D E5RenderTarget2D; //Framebuffer to render the image to and then apply postprocessing
        RenderTarget2D E5Frame;

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
            // Initialize the camera
            this.camera = new Camera(new Vector3(0, 0.5f, 2), new Vector3(0, 0, -1), new Vector3(0, 1, 0));

            this.IsMouseVisible = true;

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
                }

                this.sceneHasChanged = false;
            }

            base.Update(gameTime);
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

            switch (this.sceneState)
            {
                case Scenes.Normal:
                    NormalScene normalScene = (NormalScene)this.scene;

                    normalScene.world = Matrix.CreateScale(3.0f);

                    normalScene.mesh = normalScene.model.Meshes[0];
                    Effect effect = normalScene.mesh.Effects[0];

                    this.camera.SetEffectParameters(effect);
                    effect.CurrentTechnique = effect.Techniques["Normal"];

                    effect.Parameters["Light"].SetValue(new Vector3(50.0f, 50.0f, 50.0f));
                    effect.Parameters["Camera"].SetValue(this.camera.Eye);
                    effect.Parameters["World"].SetValue(normalScene.world);
                    normalScene.mesh.Draw();
                    break;
                case Scenes.White:
                    WhiteScene whiteScene = (WhiteScene)this.scene;

                    whiteScene.world = Matrix.CreateScale(3.0f);

                    whiteScene.mesh = whiteScene.model.Meshes[0];
                    Effect sEffect = whiteScene.mesh.Effects[0];

                    this.camera.SetEffectParameters(sEffect);
                    sEffect.CurrentTechnique = sEffect.Techniques["White"];

                    sEffect.Parameters["Light"].SetValue(new Vector3(50.0f, 50.0f, 50.0f));
                    sEffect.Parameters["Camera"].SetValue(this.camera.Eye);
                    sEffect.Parameters["World"].SetValue(whiteScene.world);
                    whiteScene.mesh.Draw();
                    break;
            }

           

            //E5 begin
            /*graphics.GraphicsDevice.SetRenderTarget(null);
            //E5Frame = E5RenderTarget2D;
            effect.CurrentTechnique = effect.Techniques["E5PostProcessing"];
            spriteBatch.Begin(0, BlendState.Opaque, null, null, null, effect);
            spriteBatch.Draw(E5RenderTarget2D, new Rectangle(0, 0, 800, 600), Color.DeepSkyBlue);
            spriteBatch.End();*/
            //E5 einde

            base.Draw(gameTime);
        }
    }
}
