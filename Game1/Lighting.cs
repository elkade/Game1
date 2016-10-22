﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class Lighting
    {
        public Effect Effect;
        public Vector3 Position;
        public float Angle;
        public Lighting(Effect effect, Vector3 position, float angle)
        {
            Effect = effect;
            Position = position;
            Angle = angle;
        }

        public Effect UpdateEffect(Matrix world, Camera camera, Color color)
        {
            Effect.Parameters["World"].SetValue(world);
            Effect.Parameters["View"].SetValue(camera.ViewMatrix);
            Effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
            Effect.Parameters["CameraPosition"].SetValue(camera.Position);
            Effect.Parameters["LightPosition"].SetValue(Position);
            Effect.Parameters["SurfaceColor"].SetValue(color.ToVector3());

            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world));
            //Effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
            return Effect;
        }

    }
}
