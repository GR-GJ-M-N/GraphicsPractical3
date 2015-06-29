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
    abstract class Scene
    {
        public Matrix world;
        public Effect effect;
        public Camera camera;
        public String name;

        public abstract void Draw();
    }

    class NormalScene : Scene
    {
        public ModelMesh mesh;
        public Model model;

        public NormalScene()
        {
            this.name = "Normal";
        }

        public override void Draw()
        {
            this.mesh = this.model.Meshes[0];
            Effect effect = this.mesh.Effects[0];

            this.camera.SetEffectParameters(effect);
            effect.CurrentTechnique = effect.Techniques["Normal"];

            effect.Parameters["Light"].SetValue(new Vector3(50.0f, 50.0f, 50.0f));
            effect.Parameters["Camera"].SetValue(this.camera.Eye);
            effect.Parameters["World"].SetValue(this.world);
            this.mesh.Draw();
        }
    }

    class WhiteScene : Scene
    {
        public ModelMesh mesh;
        public Model model;

        public WhiteScene()
        {
            this.name = "White";
        }

        public override void Draw()
        {
            this.mesh = this.model.Meshes[0];
            Effect sEffect = this.mesh.Effects[0];

            this.camera.SetEffectParameters(sEffect);
            sEffect.CurrentTechnique = sEffect.Techniques["White"];

            sEffect.Parameters["Light"].SetValue(new Vector3(50.0f, 50.0f, 50.0f));
            sEffect.Parameters["Camera"].SetValue(this.camera.Eye);
            sEffect.Parameters["World"].SetValue(this.world);
            this.mesh.Draw();
        }
    }

    class E1Scene : Scene
    {
        //public ModelMesh mesh;
        //public Model model;

        public E1Scene()
        {
            this.name = "E1";
        }

        public override void Draw()
        {
            //
        }
    }

    class E5Scene : Scene
    {
        public ModelMesh mesh;
        public Model model;

        RenderTarget2D target;
        bool test;
         
        public E5Scene()
        {
            name = "E5";
            test = false;

            #if DEBUG
                test = true;
            #endif
        }

        public override void Draw() {}

        public void DrawBefore(GraphicsDeviceManager graphics)
        {
            if (test)
            {
                target = new RenderTarget2D(graphics.GraphicsDevice, 800, 600);
                graphics.GraphicsDevice.SetRenderTarget(target);
            }

            this.mesh = this.model.Meshes[0];
            Effect effect = this.mesh.Effects[0];

            this.camera.SetEffectParameters(effect);
            effect.CurrentTechnique = effect.Techniques["Normal"];

            effect.Parameters["Light"].SetValue(new Vector3(50.0f, 50.0f, 50.0f));
            effect.Parameters["Camera"].SetValue(this.camera.Eye);
            effect.Parameters["World"].SetValue(this.world);
            this.mesh.Draw();
        }

        public void DrawAfter(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            if (test)
            {
                graphics.GraphicsDevice.SetRenderTarget(null);
                //E5Frame = E5RenderTarget2D;
                effect.CurrentTechnique = effect.Techniques["E5PostProcessing"];
                spriteBatch.Begin(0, BlendState.Opaque, null, null, null, effect);
                spriteBatch.Draw(target, new Rectangle(0, 0, 800, 600), Color.DeepSkyBlue);
                spriteBatch.End();
            }
        }
    }
}
