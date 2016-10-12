using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Models
{
    abstract class CustomModel
    {
        public VertexPositionNormalTexture[] Vertices { get; set; }
        public Vector3 Position { get; protected set; }

        public Matrix WorldMatrix { get { return Matrix.CreateTranslation(Position); } }

        public virtual void Draw(Effect effect, GraphicsDeviceManager graphics)
        {
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vertices, 0, Vertices.Length / 3);
            }
        }
    }
}
