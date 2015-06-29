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
    }

    class NormalScene : Scene
    {
        public ModelMesh mesh;
        public Model model;
    }

    class WhiteScene : Scene
    {
        public ModelMesh mesh;
        public Model model;
    }
}
