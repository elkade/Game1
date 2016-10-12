using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Lighting
    {
        public Effect Effect;
        public Vector3 Position;
        public Lighting(Effect effect, Vector3 position)
        {
            Effect = effect;
            Position = position;
        }

        public Effect UpdateEffect(Matrix world, Camera camera)
        {
            Effect.Parameters["World"].SetValue(world);
            Effect.Parameters["View"].SetValue(camera.ViewMatrix);
            Effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
            Effect.Parameters["ViewVector"].SetValue(camera.ViewVector);
            Effect.Parameters["LightPosition"].SetValue(Position);

            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world));
            Effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
            return Effect;
        }

    }
}
