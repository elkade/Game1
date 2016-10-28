using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class Item
    {
        public Color Color { get; set; }

        public Model model;

        public Vector3 Rotation;

        public Vector3 _scale;

        public Vector3 Position;

        public Item(Vector3 scale, Vector3 position, Vector3 rotation)
        {
            _scale = scale;
            Position = position;
            Rotation = rotation;
        }

        public Item(float scale, Vector3 position, Vector3 rotation)
        {
            _scale = new Vector3(scale,scale,scale);
            Position = position;
            Rotation = rotation;
        }
        public void Initialize(Model model)
        {
            this.model = model;
        }

        public virtual void Update(GameTime gameTime)
        {
            // TotalSeconds is a double so we need to cast to float
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
            return Matrix.CreateRotationX(Rotation.X)
                * Matrix.CreateRotationY(Rotation.Y)
                * Matrix.CreateRotationZ(Rotation.Z)
                * Matrix.CreateScale(_scale)
                * Matrix.CreateTranslation(Position);

            //const float circleRadius = 0.5f;
            //const float heightOffGround = 0;

            //// this matrix moves the model "out" from the origin
            //Matrix translationMatrix = Matrix.CreateRotationX(MathHelper.PiOver4 * 2) * Matrix.CreateTranslation(
            //    circleRadius, 0, heightOffGround);;

            //// this matrix rotates everything around the origin
            //Matrix rotationMatrix = Matrix.CreateRotationZ(angle);

            //// We combine the two to have the model move in a circle:
            //Matrix combined = translationMatrix * rotationMatrix;

            //return combined;
        }

    }
}
