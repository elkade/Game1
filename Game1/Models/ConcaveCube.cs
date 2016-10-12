using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Models
{
    class ConcaveCube : CustomModel
    {
        public ConcaveCube(Vector3 position, int size)
        {
            Position = position;

            Vertices = new VertexPositionNormalTexture[36];

            size /= 2;

            // Calculate the position of the vertices on the top face.
            Vector3 topLeftFront = position + new Vector3(-1.0f, 1.0f, -1.0f) * size;
            Vector3 topLeftBack = position + new Vector3(-1.0f, 1.0f, 1.0f) * size;
            Vector3 topRightFront = position + new Vector3(1.0f, 1.0f, -1.0f) * size;
            Vector3 topRightBack = position + new Vector3(1.0f, 1.0f, 1.0f) * size;

            // Calculate the position of the vertices on the bottom face.
            Vector3 btmLeftFront = position + new Vector3(-1.0f, -1.0f, -1.0f) * size;
            Vector3 btmLeftBack = position + new Vector3(-1.0f, -1.0f, 1.0f) * size;
            Vector3 btmRightFront = position + new Vector3(1.0f, -1.0f, -1.0f) * size;
            Vector3 btmRightBack = position + new Vector3(1.0f, -1.0f, 1.0f) * size;

            // Normal vectors for each face (needed for lighting / display)
            Vector3 normalFront = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 normalBack = new Vector3(0.0f, 0.0f, -1.0f);
            Vector3 normalTop = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 normalBottom = new Vector3(0.0f, -1.0f, 0.0f);
            Vector3 normalLeft = new Vector3(-1.0f, 0.0f, 0.0f);
            Vector3 normalRight = new Vector3(1.0f, 0.0f, 0.0f);

            // UV texture coordinates
            Vector2 textureTopLeft = new Vector2(1.0f * size, 0.0f * size);
            Vector2 textureTopRight = new Vector2(0.0f * size, 0.0f * size);
            Vector2 textureBottomLeft = new Vector2(1.0f * size, 1.0f * size);
            Vector2 textureBottomRight = new Vector2(0.0f * size, 1.0f * size);

            // Add the vertices for the FRONT face.
            Vertices[2] = new VertexPositionNormalTexture(topLeftFront, normalFront, textureTopLeft);
            Vertices[1] = new VertexPositionNormalTexture(btmLeftFront, normalFront, textureBottomLeft);
            Vertices[0] = new VertexPositionNormalTexture(topRightFront, normalFront, textureTopRight);

            Vertices[5] = new VertexPositionNormalTexture(btmLeftFront, normalFront, textureBottomLeft);
            Vertices[4] = new VertexPositionNormalTexture(btmRightFront, normalFront, textureBottomRight);
            Vertices[3] = new VertexPositionNormalTexture(topRightFront, normalFront, textureTopRight);

            // Add the vertices for the BACK face.
            Vertices[8] = new VertexPositionNormalTexture(topLeftBack, normalBack, textureTopRight);
            Vertices[7] = new VertexPositionNormalTexture(topRightBack, normalBack, textureTopLeft);
            Vertices[6] = new VertexPositionNormalTexture(btmLeftBack, normalBack, textureBottomRight);

            Vertices[11] = new VertexPositionNormalTexture(btmLeftBack, normalBack, textureBottomRight);
            Vertices[10] = new VertexPositionNormalTexture(topRightBack, normalBack, textureTopLeft);
            Vertices[9] = new VertexPositionNormalTexture(btmRightBack, normalBack, textureBottomLeft);

            // Add the vertices for the TOP face.
            Vertices[14] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureBottomLeft);
            Vertices[13] = new VertexPositionNormalTexture(topRightBack, normalTop, textureTopRight);
            Vertices[12] = new VertexPositionNormalTexture(topLeftBack, normalTop, textureTopLeft);

            Vertices[17] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureBottomLeft);
            Vertices[16] = new VertexPositionNormalTexture(topRightFront, normalTop, textureBottomRight);
            Vertices[15] = new VertexPositionNormalTexture(topRightBack, normalTop, textureTopRight);

            // Add the vertices for the BOTTOM face. 
            Vertices[20] = new VertexPositionNormalTexture(btmLeftFront, normalBottom, textureTopLeft);
            Vertices[19] = new VertexPositionNormalTexture(btmLeftBack, normalBottom, textureBottomLeft);
            Vertices[18] = new VertexPositionNormalTexture(btmRightBack, normalBottom, textureBottomRight);

            Vertices[23] = new VertexPositionNormalTexture(btmLeftFront, normalBottom, textureTopLeft);
            Vertices[22] = new VertexPositionNormalTexture(btmRightBack, normalBottom, textureBottomRight);
            Vertices[21] = new VertexPositionNormalTexture(btmRightFront, normalBottom, textureTopRight);

            // Add the vertices for the LEFT face.
            Vertices[26] = new VertexPositionNormalTexture(topLeftFront, normalLeft, textureTopRight);
            Vertices[25] = new VertexPositionNormalTexture(btmLeftBack, normalLeft, textureBottomLeft);
            Vertices[24] = new VertexPositionNormalTexture(btmLeftFront, normalLeft, textureBottomRight);

            Vertices[29] = new VertexPositionNormalTexture(topLeftBack, normalLeft, textureTopLeft);
            Vertices[28] = new VertexPositionNormalTexture(btmLeftBack, normalLeft, textureBottomLeft);
            Vertices[27] = new VertexPositionNormalTexture(topLeftFront, normalLeft, textureTopRight);

            // Add the vertices for the RIGHT face. 
            Vertices[32] = new VertexPositionNormalTexture(topRightFront, normalRight, textureTopLeft);
            Vertices[31] = new VertexPositionNormalTexture(btmRightFront, normalRight, textureBottomLeft);
            Vertices[30] = new VertexPositionNormalTexture(btmRightBack, normalRight, textureBottomRight);

            Vertices[35] = new VertexPositionNormalTexture(topRightBack, normalRight, textureTopRight);
            Vertices[34] = new VertexPositionNormalTexture(topRightFront, normalRight, textureTopLeft);
            Vertices[33] = new VertexPositionNormalTexture(btmRightBack, normalRight, textureBottomRight);
        }
    }
}
