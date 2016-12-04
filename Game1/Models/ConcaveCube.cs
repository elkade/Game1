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

            int n = 1;

            Vector3 topLeftFront = position + new Vector3(-1.0f, 1.0f, -1.0f) * size;
            Vector3 topLeftBack = position + new Vector3(-1.0f, 1.0f, 1.0f) * size;
            Vector3 topRightFront = position + new Vector3(1.0f, 1.0f, -1.0f) * size;
            Vector3 topRightBack = position + new Vector3(1.0f, 1.0f, 1.0f) * size;

            Vector3 btmLeftFront = position + new Vector3(-1.0f, -1.0f, -1.0f) * size;
            Vector3 btmLeftBack = position + new Vector3(-1.0f, -1.0f, 1.0f) * size;
            Vector3 btmRightFront = position + new Vector3(1.0f, -1.0f, -1.0f) * size;
            Vector3 btmRightBack = position + new Vector3(1.0f, -1.0f, 1.0f) * size;

            vertices.AddRange(GetWall(topLeftFront, topRightFront, btmRightFront, btmLeftFront, n, size));
            vertices.AddRange(GetWall(btmLeftBack, btmRightBack, topRightBack, topLeftBack, n, size));
            vertices.AddRange(GetWall(topLeftBack, topRightBack, topRightFront, topLeftFront, n, size));
            vertices.AddRange(GetWall(btmRightFront, btmRightBack, btmLeftBack, btmLeftFront, n, size));
            vertices.AddRange(GetWall(topLeftFront, btmLeftFront, btmLeftBack, topLeftBack, n, size));
            vertices.AddRange(GetWall(btmRightBack, btmRightFront, topRightFront, topRightBack, n, size));

            Vertices = vertices.ToArray();
        }

        private IEnumerable<VertexPositionNormalTexture> GetWall(Vector3 btmLeft, Vector3 topLeft, Vector3 topRight, Vector3 btmRight, int n, int size)
        {
            var sizeT = 1f;
            Vector2 textureTopLeft = new Vector2(1.0f * sizeT, 0.0f * sizeT);
            Vector2 textureTopRight = new Vector2(0.0f * sizeT, 0.0f * sizeT);
            Vector2 textureBottomLeft = new Vector2(1.0f * sizeT, 1.0f * sizeT);
            Vector2 textureBottomRight = new Vector2(0.0f * sizeT, 1.0f * sizeT);


            Vector3 middle = (btmLeft + topLeft + topRight + btmRight) / 4;

            Vector3 normal =  Position - middle;
            normal.Normalize();

            var x = (btmRight - btmLeft) / n;
            var y = (topLeft - btmLeft) / n;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    yield return new VertexPositionNormalTexture(btmLeft + (i + 1) * x + (j + 1) * y, normal, textureTopRight);
                    yield return new VertexPositionNormalTexture(btmLeft + i * x + j * y, normal, textureBottomLeft);
                    yield return new VertexPositionNormalTexture(btmLeft + i * x + (j + 1) * y, normal, textureTopLeft);
                    yield return new VertexPositionNormalTexture(btmLeft + (i + 1) * x + (j + 1) * y, normal, textureTopRight);
                    yield return new VertexPositionNormalTexture(btmLeft + (i + 1) * x + j * y, normal, textureBottomRight);
                    yield return new VertexPositionNormalTexture(btmLeft + i * x + j * y, normal, textureBottomLeft);

                }
            }
        }

    }
}
