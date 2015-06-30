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

        public void Init()
        {

        }
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
    class E3Scene : Scene
    {
        public ModelMesh mesh;
        public Model model;

        public E3Scene()
        {
            this.name = "E3";
        }

        public override void Draw()
        { 
            this.mesh = this.model.Meshes[0];
            Effect effect = this.mesh.Effects[0];

            this.camera.SetEffectParameters(effect);
            effect.CurrentTechnique = effect.Techniques["CellShading"];

            effect.Parameters["Light"].SetValue(new Vector3(50.0f, 50.0f, 50.0f));
            effect.Parameters["Camera"].SetValue(this.camera.Eye);
            effect.Parameters["World"].SetValue(this.world);
            effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector4());
            effect.Parameters["SpecularColor"].SetValue(Color.White.ToVector4());
            effect.Parameters["SpecularIntensity"].SetValue(0.5f);
            effect.Parameters["SpecularPower"].SetValue(20000.0f);
            effect.Parameters["ShadesCount"].SetValue(4);
            this.mesh.Draw();
        }
    }
        //////////////////////////////////////////////////////////////////////////////////
    class E5Scene : Scene
    {
        public ModelMesh mesh;
        public Model model;

        public RenderTarget2D target;
         
        public E5Scene()
        {
            name = "E5";
        }

        public override void Draw() {}

        public void DrawBefore(GraphicsDeviceManager graphics)
        { 
            graphics.GraphicsDevice.SetRenderTarget(target);

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
            graphics.GraphicsDevice.SetRenderTarget(null);
            effect.CurrentTechnique = effect.Techniques["E5PostProcessing"];
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, effect);
            spriteBatch.Draw(target, new Rectangle(0, 0, 800, 600), Color.DeepSkyBlue);
            spriteBatch.End();
        }
    }

    class E6Scene : Scene
    {
        public ModelMesh mesh;
        public Model model;

        public RenderTarget2D target;

        public E6Scene()
        {
            name = "E6";
        }

        public override void Draw() { }

        public void DrawBefore(GraphicsDeviceManager graphics)
        {
            graphics.GraphicsDevice.SetRenderTarget(target);
            this.mesh = this.model.Meshes[0];
            Effect effect = this.mesh.Effects[0];

            this.camera.SetEffectParameters(effect);
            //effect.CurrentTechnique = effect.Techniques["BlinnPhong"];
            effect.CurrentTechnique = effect.Techniques["Normal"];

            effect.Parameters["Light"].SetValue(new Vector3(50.0f, 50.0f, 50.0f));
            effect.Parameters["Camera"].SetValue(this.camera.Eye);
            effect.Parameters["World"].SetValue(this.world);
            effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector4());
            effect.Parameters["SpecularColor"].SetValue(Color.White.ToVector4());
            effect.Parameters["SpecularIntensity"].SetValue(0.5f);
            effect.Parameters["SpecularPower"].SetValue(20000.0f);
            this.mesh.Draw();
        }

        public void DrawAfter(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            float[] weights = new float[7];
            weights[0] = 0.0159284f;
            weights[1] = 0.02707784f;
            weights[2] = 0.04242319f;
            weights[3] = 0.06125479f;
            weights[4] = 0.0815125f;
            weights[5] = 0.09996679f;
            weights[6] = 0.1129886f;

            /*for (int i = 0; i < 7; i++)
            {
                weights[i] = 1 / 7.0f;
            }*/

            //Horizontal
            Vector2[] offsets = new Vector2[7];
            for (int i = 0; i < 7; i++)
            {
                offsets[i] = new Vector2((-3 + i) / 800.0f, 0);
            }

            graphics.GraphicsDevice.SetRenderTarget(null);
            effect.CurrentTechnique = effect.Techniques["E6PostProcessing"];
            effect.Parameters["offsets"].SetValue(offsets);
            effect.Parameters["weights"].SetValue(weights);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, effect);
            spriteBatch.Draw(target, new Rectangle(0, 0, 800, 600), Color.DeepSkyBlue);
            spriteBatch.End();

            //Vertical
            offsets = new Vector2[7];
            for (int i = 0; i < 7; i++)
            {
                offsets[i] = new Vector2(0, (-3 + i) / 800.0f);
            }
            effect.Parameters["offsets"].SetValue(offsets);
            effect.Parameters["weights"].SetValue(weights);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, effect);
            spriteBatch.Draw(target, new Rectangle(0, 0, 800, 600), Color.DeepSkyBlue);
            spriteBatch.End();
        }
    }
}
