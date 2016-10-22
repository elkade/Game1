﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Robot
    {
        public Color Color { get; set; }

        public Model model;

        float angle;

        float _scale;

        Vector3 Position;

        public Robot(float scale, Vector3 position)
        {
            _scale = scale;
            Position = position;
        }
        public void Initialize(Model model)
        {
            this.model = model;
        }

        public void Update(GameTime gameTime)
        {
            // TotalSeconds is a double so we need to cast to float
            //angle += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(Camera camera, Lighting lighting)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                    part.Effect = lighting.UpdateEffect(GetWorldMatrix(), camera, Color);
                mesh.Draw();
            }
        }
        public Matrix GetWorldMatrix()
        {
            return Matrix.CreateRotationX(MathHelper.PiOver4 * 2) * Matrix.CreateScale(_scale) * Matrix.CreateTranslation(Position);

            const float circleRadius = 0.5f;
            const float heightOffGround = 0;

            // this matrix moves the model "out" from the origin
            Matrix translationMatrix = Matrix.CreateRotationX(MathHelper.PiOver4 * 2) * Matrix.CreateTranslation(
                circleRadius, 0, heightOffGround);;

            // this matrix rotates everything around the origin
            Matrix rotationMatrix = Matrix.CreateRotationZ(angle);

            // We combine the two to have the model move in a circle:
            Matrix combined = translationMatrix * rotationMatrix;

            return combined;
        }

    }
}
