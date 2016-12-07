using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game1.Models
{
    class Screen : CustomModel
    {
        public Screen(Vector3 position, int sizeX, int sizeY)
        {
            //Position = position;

            //List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();

            //Vector3 topLeft = position + new Vector3(-1.0f * sizeX / 2, 1.0f * sizeY / 2, 0);
            //Vector3 topRight = position + new Vector3(1.0f * sizeX / 2, 1.0f * sizeY / 2, 0);
            //Vector3 btmLeft = position + new Vector3(-1.0f * sizeX / 2, -1.0f * sizeY / 2, 0);
            //Vector3 btmRight = position + new Vector3(1.0f * sizeX / 2, -1.0f * sizeY / 2, 0);

            //Vector3 normal = new Vector3(0.0f, 0.0f, -1.0f);

            //Vector2 textureTopLeft = new Vector2(1.0f, 0.0f);
            //Vector2 textureTopRight = new Vector2(0.0f, 0.0f);
            //Vector2 textureBottomLeft = new Vector2(1.0f, 1.0f);
            //Vector2 textureBottomRight = new Vector2(0.0f, 1.0f);

            //vertices.Add(new VertexPositionNormalTexture(topLeft, normal, textureTopLeft));
            //vertices.Add(new VertexPositionNormalTexture(topRight, normal, textureTopRight));
            //vertices.Add(new VertexPositionNormalTexture(btmRight, normal, textureBottomRight));

            //vertices.Add(new VertexPositionNormalTexture(btmRight, normal, textureBottomRight));
            //vertices.Add(new VertexPositionNormalTexture(btmLeft, normal, textureBottomLeft));
            //vertices.Add(new VertexPositionNormalTexture(topLeft, normal, textureTopLeft));

            //Vertices = vertices.ToArray();

            Position = position;

            Vertices = new VertexPositionNormalTexture[6];

            sizeX /= 2;
            sizeY /= 2;
            float sizeT = 1;

            Vector3 topLeftFront = position + new Vector3(-1.0f * sizeX, 1.0f * sizeY, 0);
            Vector3 topRightFront = position + new Vector3(1.0f * sizeX, 1.0f * sizeY, 0);

            Vector3 btmLeftFront = position + new Vector3(-1.0f * sizeX, -1.0f * sizeY, 0);
            Vector3 btmRightFront = position + new Vector3(1.0f * sizeX, -1.0f * sizeY, 0);

            Vector3 normalFront = new Vector3(0.0f, 0.0f, -1.0f);

            Vector2 textureTopLeft = new Vector2(1.0f * sizeT, 0.0f * sizeT);
            Vector2 textureTopRight = new Vector2(0.0f * sizeT, 0.0f * sizeT);
            Vector2 textureBottomLeft = new Vector2(1.0f * sizeT, 1.0f * sizeT);
            Vector2 textureBottomRight = new Vector2(0.0f * sizeT, 1.0f * sizeT);

            Vertices[0] = new VertexPositionNormalTexture(topLeftFront, normalFront, textureTopLeft);
            Vertices[1] = new VertexPositionNormalTexture(btmLeftFront, normalFront, textureBottomLeft);
            Vertices[2] = new VertexPositionNormalTexture(topRightFront, normalFront, textureTopRight);

            Vertices[3] = new VertexPositionNormalTexture(btmLeftFront, normalFront, textureBottomLeft);
            Vertices[4] = new VertexPositionNormalTexture(btmRightFront, normalFront, textureBottomRight);
            Vertices[5] = new VertexPositionNormalTexture(topRightFront, normalFront, textureTopRight);

        }
        public override void Draw(Effect effect, GraphicsDeviceManager graphics, Texture texture = null, Texture texture2 = null, TextureCube text = null)
        {
            effect.Parameters["ScreenMode"].SetValue(true);
            base.Draw(effect, graphics, texture);
            effect.Parameters["ScreenMode"].SetValue(false);

        }
    }
}
