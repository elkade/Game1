using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class Robot
    {
        public Color Color { get; set; }

        public Model model;

        public Vector3 Rotation;

        public Vector3 _scale;

        public Texture Texture = null;

        public Vector3 Position;

        public Robot(Vector3 scale, Vector3 position, Vector3 rotation)
        {
            _scale = scale;
            Position = position;
            Rotation = rotation;
        }

        public Robot(float scale, Vector3 position, Vector3 rotation)
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
                {
                    part.Effect = lighting.UpdateEffect(GetWorldMatrix(), camera, Color, Texture);
                }
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
        public void DrawBasic(Camera camera, BasicEffect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    effect.LightingEnabled = true; // Turn on the lighting subsystem.

                    effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0.2f, 0.2f); // a reddish light
                    effect.DirectionalLight0.Direction = new Vector3(1, 0, 0);  // coming along the x-axis
                    effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights

                    effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f); // Add some overall ambient light.
                    //effect.EmissiveColor = new Vector3(1, 0, 0); // Sets some strange emmissive lighting.  This just looks weird.

                    effect.World = GetWorldMatrix();
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;

                    effect.EnableDefaultLighting();


                    part.Effect = effect;
                }
                mesh.Draw();
            }
        }

    }
}
