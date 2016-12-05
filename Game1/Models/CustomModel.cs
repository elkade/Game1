using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Models
{
    abstract class CustomModel
    {
        public VertexPositionNormalTexture[] Vertices { get; set; }
        public Vector3 Position { get; protected set; }

        public Matrix WorldMatrix { get { return Matrix.CreateTranslation(Position); } }

        public virtual void Draw(Effect effect, GraphicsDeviceManager graphics, Texture texture=null, Texture texture2 = null)
        {
            if (texture != null)
            {
                effect.Parameters["TextureEnabled"].SetValue(true);
                effect.Parameters["BasicTextureB"].SetValue(texture2);
                effect.Parameters["BasicTextureA"].SetValue(texture);

            }
            else
                effect.Parameters["TextureEnabled"].SetValue(false);



            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vertices, 0, Vertices.Length / 3);
            }
        }
    }
}
