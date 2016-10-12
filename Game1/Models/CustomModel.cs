using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Models
{
    abstract class CustomModel
    {
        public VertexPositionNormalTexture[] Vertices { get; set; }
        public Vector3 Position { get; protected set; }

        public virtual void Draw(Effect effect, Camera camera, GraphicsDeviceManager graphics)
        {
            effect.Parameters["World"].SetValue(Matrix.CreateTranslation(Position));

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vertices, 0, Vertices.Length / 3);
            }
        }
    }
}
