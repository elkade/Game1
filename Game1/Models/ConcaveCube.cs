using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Game1.Models
{
    class ConcaveCube : CustomModel
    {
        public ConcaveCube(Vector3 position, int size)
        {
            Position = position;

            List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();

            size /= 2;

            int n = 100;

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
            Vector3 normalTop = new Vector3(0.0f, -1.0f, 0.0f);
            Vector3 normalBottom = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 normalLeft = new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 normalRight = new Vector3(-1.0f, 0.0f, 0.0f);

            Vector2 textureTopLeft = Vector2.Zero;//= new Vector2(1.0f * size, 0.0f * size);
            Vector2 textureTopRight = Vector2.Zero;// = new Vector2(0.0f * size, 0.0f * size);
            Vector2 textureBottomLeft = Vector2.Zero;// = new Vector2(1.0f * size, 1.0f * size);
            Vector2 textureBottomRight = Vector2.Zero;// = new Vector2(0.0f * size, 1.0f * size);


            vertices.AddRange(GetWall(topLeftFront, topRightFront, btmRightFront, btmLeftFront, n, size, normalFront));
            vertices.AddRange(GetWall(btmLeftBack, btmRightBack, topRightBack, topLeftBack, n, size, normalBack));
            vertices.AddRange(GetWall(topLeftBack, topRightBack, topRightFront, topLeftFront, n, size, normalTop));
            vertices.AddRange(GetWall(btmRightFront,  btmRightBack, btmLeftBack, btmLeftFront, n, size, normalBottom));
            vertices.AddRange(GetWall(topLeftFront, btmLeftFront, btmLeftBack, topLeftBack, n, size, normalLeft));
            vertices.AddRange(GetWall(btmRightBack, btmRightFront, topRightFront, topRightBack, n, size, normalRight));

            Vertices = vertices.ToArray();
        }

        private IEnumerable<VertexPositionNormalTexture> GetWall(Vector3 btmLeft, Vector3 topLeft, Vector3 topRight, Vector3 btmRight, int n, int size, Vector3 normal)
        {
            Vector2 textureTopLeft = Vector2.Zero;//= new Vector2(1.0f * size, 0.0f * size);
            Vector2 textureTopRight = Vector2.Zero;// = new Vector2(0.0f * size, 0.0f * size);
            Vector2 textureBottomLeft = Vector2.Zero;// = new Vector2(1.0f * size, 1.0f * size);
            Vector2 textureBottomRight = Vector2.Zero;// = new Vector2(0.0f * size, 1.0f * size);

            var x = (btmRight - btmLeft) / n;
            var y = (topLeft - btmLeft) / n;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    yield return new VertexPositionNormalTexture(btmLeft + (i+1) * x + (j+1) * y, normal, textureTopRight);
                    yield return new VertexPositionNormalTexture(btmLeft + i * x + j * y, normal, textureBottomLeft);
                    yield return new VertexPositionNormalTexture(btmLeft + i * x + (j+1) * y, normal, textureTopLeft);

                    yield return new VertexPositionNormalTexture(btmLeft + (i + 1) * x + (j + 1) * y, normal, textureTopRight);
                    yield return new VertexPositionNormalTexture(btmLeft + (i+1) * x + j * y, normal, textureBottomRight);
                    yield return new VertexPositionNormalTexture(btmLeft + i * x + j * y, normal, textureBottomLeft);

                }
            }
        }

    }
}
