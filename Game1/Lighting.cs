using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Game1
{
    public class Lighting
    {
        public Effect Effect;

        Light[] _lights;

        public Lighting(Effect effect, params Light[] lights)
        {
            Effect = effect;
            _lights = lights;
        }

        public BasicEffect UpdateBasicEffect(BasicEffect effect, Matrix world, Camera camera, Color color, Texture texture = null)
        {
            effect.LightingEnabled = true; // Turn on the lighting subsystem.

            effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0.2f, 0.2f); // a reddish light
            effect.DirectionalLight0.Direction = new Vector3(1, 0, 0);  // coming along the x-axis
            effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights

            effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f); // Add some overall ambient light.
                                                                      //effect.EmissiveColor = new Vector3(1, 0, 0); // Sets some strange emmissive lighting.  This just looks weird.

            effect.World = world;
            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;

            effect.EnableDefaultLighting();
            return effect;
        }

        public Effect UpdateEffect(Matrix world, Camera camera, Color color, Texture texture = null)
        {
            Effect.Parameters["World"].SetValue(world);
            Effect.Parameters["View"].SetValue(camera.ViewMatrix);
            Effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
            Effect.Parameters["CameraPosition"].SetValue(camera.Position);
            Effect.Parameters["LightPosition"].SetValue(_lights.Select(l => l.Position).ToArray());
            Effect.Parameters["LightDirection"].SetValue(_lights.Select(l=>l.Direction).ToArray());
            Effect.Parameters["LightPhi"].SetValue(_lights.Select(l=>l.Phi).ToArray());
            Effect.Parameters["LightTheta"].SetValue(_lights.Select(l=>l.Theta).ToArray());
            Effect.Parameters["SurfaceColor"].SetValue(color.ToVector3());
            Effect.Parameters["DiffuseColor"].SetValue(_lights.Select(l => l.DiffuseColor.ToVector3()).ToArray());
            Effect.Parameters["SpecularColor"].SetValue(_lights.Select(l => l.SpecularColor.ToVector3()).ToArray());
            if (texture != null)
            {
                Effect.Parameters["TextureEnabled"].SetValue(true);
                Effect.Parameters["BasicTextureA"].SetValue(texture);
            }
            else
                Effect.Parameters["TextureEnabled"].SetValue(false);

            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world));
            Effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);

            Effect.Parameters["pView"].SetValue(_lights[0].ViewMatrix);
            Effect.Parameters["pProjection"].SetValue(_lights[0].ProjectionMatrix);

            return Effect;
        }

    }

    public class Light
    {
        public Light()
        {
            Phi = MathHelper.TwoPi;
            Theta = MathHelper.TwoPi;
            Direction = new Vector3(0, 1.1f, 0);
            RotationMatrix = Matrix.Identity * Matrix.CreateRotationX(MathHelper.PiOver2 + 0.1f);
        }
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        public Color DiffuseColor { get; set; }
        public Color SpecularColor { get; set; }
        public float Phi { get; set; }
        public float Theta { get; set; }

        Vector3 lookAtVector
        {
            get
            {
                var lookat = Vector3.Transform(new Vector3(0, 0, 1), RotationMatrix);
                lookat.Normalize();
                return lookat;
            }
        }

        public Matrix RotationMatrix
        {
            get; set;
        }

        public Matrix ViewMatrix
        {
            get
            {
                return Matrix.CreateLookAt(Position, lookAtVector + Position, Vector3.UnitY);
            }
        }


        public Matrix ProjectionMatrix
        {
            get
            {
                float fieldOfView = MathHelper.PiOver4;
                float nearClipPlane = 1;
                float farClipPlane = 200;
                float aspectRatio = 1;

                return Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
            }
        }
    }
}
