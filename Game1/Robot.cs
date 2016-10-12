using Microsoft.Xna.Framework;
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
        public Model model;

        float angle;

        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("witcher");
        }

        public void Update(GameTime gameTime)
        {
            // TotalSeconds is a double so we need to cast to float
            //angle += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(Camera camera, Effect effect)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * GetWorldMatrix()));
                    effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                    effect.Parameters["World"].SetValue(GetWorldMatrix());
                }

                mesh.Draw();
            }
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    var world = GetWorldMatrix();

                    effect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform);
                    //effect.Parameters["View"].SetValue(camera.ViewMatrix);
                    //effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);

                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
                    effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                }
                mesh.Draw();
            }
        }
        public Matrix GetWorldMatrix()
        {
            return Matrix.CreateRotationX(MathHelper.PiOver4 * 2) * Matrix.CreateTranslation(0, 0, 0) * Matrix.CreateScale(0.1f);
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
