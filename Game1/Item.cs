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
        }

    }
}
