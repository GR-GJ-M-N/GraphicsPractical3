﻿using System;
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
            effect.Parameters["ShadesCount"].SetValue(3);
            this.mesh.Draw();
        }
    }
}
