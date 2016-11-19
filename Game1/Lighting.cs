﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Game1
{
    public class Lighting
    {
        public Effect Effect;

        Light[] _lights;

        public Lighting(Effect effect, params Light[] lights)
        {
            Effect = effect;
            _lights = lights;
        }

        public Effect UpdateEffect(Matrix world, Camera camera, Color color, Texture texture = null)
        {
            Effect.Parameters["World"].SetValue(world);
            Effect.Parameters["View"].SetValue(camera.ViewMatrix);
            Effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
            Effect.Parameters["CameraPosition"].SetValue(camera.Position);
            Effect.Parameters["LightPosition"].SetValue(_lights.Select(l => l.Position).ToArray());
            Effect.Parameters["LightDirection"].SetValue(_lights.Select(l=>l.Direction).ToArray());
            Effect.Parameters["LightPhi"].SetValue(_lights.Select(l=>l.Phi).ToArray());
            Effect.Parameters["LightTheta"].SetValue(_lights.Select(l=>l.Theta).ToArray());
            Effect.Parameters["SurfaceColor"].SetValue(color.ToVector3());
            Effect.Parameters["DiffuseColor"].SetValue(_lights.Select(l => l.DiffuseColor.ToVector3()).ToArray());
            Effect.Parameters["SpecularColor"].SetValue(_lights.Select(l => l.SpecularColor.ToVector3()).ToArray());
            if (texture != null)
            {
                Effect.Parameters["TextureEnabled"].SetValue(true);
                Effect.Parameters["BasicTexture"].SetValue(texture);
            }
            else
                Effect.Parameters["TextureEnabled"].SetValue(false);

            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world));
            Effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
            return Effect;
        }

    }

    public class Light
    {
        public Light()
        {
            Phi = MathHelper.TwoPi;
            Theta = MathHelper.TwoPi;
            Direction = new Vector3(0, 1, 0);
        }
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        public Color DiffuseColor { get; set; }
        public Color SpecularColor { get; set; }
        public float Phi { get; set; }
        public float Theta { get; set; }
    }
}
