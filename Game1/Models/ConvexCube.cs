using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Models
{
    class ConvexCube : CustomModel
    {
        public ConvexCube(Vector3 position, int size)
        {
            Position = position;

            Vertices = new VertexPositionNormalTexture[36];

            size /= 2;

            float sizeT = 5f;

            Vector3 topLeftFront = position + new Vector3(-1.0f, 1.0f, -1.0f) * size;
            Vector3 topLeftBack = position + new Vector3(-1.0f, 1.0f, 1.0f) * size;
            Vector3 topRightFront = position + new Vector3(1.0f, 1.0f, -1.0f) * size;
            Vector3 topRightBack = position + new Vector3(1.0f, 1.0f, 1.0f) * size;

            Vector3 btmLeftFront = position + new Vector3(-1.0f, -1.0f, -1.0f) * size;
            Vector3 btmLeftBack = position + new Vector3(-1.0f, -1.0f, 1.0f) * size;
            Vector3 btmRightFront = position + new Vector3(1.0f, -1.0f, -1.0f) * size;
            Vector3 btmRightBack = position + new Vector3(1.0f, -1.0f, 1.0f) * size;

            Vector3 normalFront = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 normalBack = new Vector3(0.0f, 0.0f, -1.0f);
            Vector3 normalTop = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 normalBottom = new Vector3(0.0f, -1.0f, 0.0f);
            Vector3 normalLeft = new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 normalRight = new Vector3(-1.0f, 0.0f, 0.0f);

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

            Vertices[6] = new VertexPositionNormalTexture(topLeftBack, normalBack, textureTopRight);
            Vertices[7] = new VertexPositionNormalTexture(topRightBack, normalBack, textureTopLeft);
            Vertices[8] = new VertexPositionNormalTexture(btmLeftBack, normalBack, textureBottomRight);

            Vertices[9] = new VertexPositionNormalTexture(btmLeftBack, normalBack, textureBottomRight);
            Vertices[10] = new VertexPositionNormalTexture(topRightBack, normalBack, textureTopLeft);
            Vertices[11] = new VertexPositionNormalTexture(btmRightBack, normalBack, textureBottomLeft);

            Vertices[12] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureBottomLeft);
            Vertices[13] = new VertexPositionNormalTexture(topRightBack, normalTop, textureTopRight);
            Vertices[14] = new VertexPositionNormalTexture(topLeftBack, normalTop, textureTopLeft);

            Vertices[15] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureBottomLeft);
            Vertices[16] = new VertexPositionNormalTexture(topRightFront, normalTop, textureBottomRight);
            Vertices[17] = new VertexPositionNormalTexture(topRightBack, normalTop, textureTopRight);

            Vertices[18] = new VertexPositionNormalTexture(btmLeftFront, normalBottom, textureTopLeft);
            Vertices[19] = new VertexPositionNormalTexture(btmLeftBack, normalBottom, textureBottomLeft);
            Vertices[20] = new VertexPositionNormalTexture(btmRightBack, normalBottom, textureBottomRight);

            Vertices[21] = new VertexPositionNormalTexture(btmLeftFront, normalBottom, textureTopLeft);
            Vertices[22] = new VertexPositionNormalTexture(btmRightBack, normalBottom, textureBottomRight);
            Vertices[23] = new VertexPositionNormalTexture(btmRightFront, normalBottom, textureTopRight);

            Vertices[24] = new VertexPositionNormalTexture(topLeftFront, normalLeft, textureTopRight);
            Vertices[25] = new VertexPositionNormalTexture(btmLeftBack, normalLeft, textureBottomLeft);
            Vertices[26] = new VertexPositionNormalTexture(btmLeftFront, normalLeft, textureBottomRight);

            Vertices[27] = new VertexPositionNormalTexture(topLeftBack, normalLeft, textureTopLeft);
            Vertices[28] = new VertexPositionNormalTexture(btmLeftBack, normalLeft, textureBottomLeft);
            Vertices[29] = new VertexPositionNormalTexture(topLeftFront, normalLeft, textureTopRight);

            Vertices[30] = new VertexPositionNormalTexture(topRightFront, normalRight, textureTopLeft);
            Vertices[31] = new VertexPositionNormalTexture(btmRightFront, normalRight, textureBottomLeft);
            Vertices[32] = new VertexPositionNormalTexture(btmRightBack, normalRight, textureBottomRight);

            Vertices[33] = new VertexPositionNormalTexture(topRightBack, normalRight, textureTopRight);
            Vertices[34] = new VertexPositionNormalTexture(topRightFront, normalRight, textureTopLeft);
            Vertices[35] = new VertexPositionNormalTexture(btmRightBack, normalRight, textureBottomRight);
        }
    }
}
