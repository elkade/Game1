using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game1.Models;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        VertexPositionNormalTexture[] floorVerts;

        CustomModel walls;
        CustomModel platform1;
        CustomModel platform2;

        BasicEffect effect;

        Texture2D checkerboardTexture;

        Vector3 cameraPosition = new Vector3(15, 10, 10);

        Robot robot;

        Camera camera;

        Effect specularEffect;

        Vector3 viewVector;

        Effect ambientEffect;
        Effect ambientEffect2;
        Effect diffuseEffect;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            // graphics.IsFullScreen = true;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            InitGround();

            walls = new ConcaveCube(Vector3.Zero, 32);
            platform1 = new ConvexCube(new Vector3(4, -4, -4), 16);
            platform2 = new ConvexCube(new Vector3(4, 4, -4), 16);

            effect = new BasicEffect(graphics.GraphicsDevice);

            robot = new Robot();
            robot.Initialize(Content);

            camera = new Camera(graphics.GraphicsDevice);

            ambientEffect = Content.Load<Effect>("Effects/Ambient");
            ambientEffect.Parameters["AmbientColor"].SetValue(Color.Violet.ToVector4());
            ambientEffect.Parameters["AmbientIntensity"].SetValue(0.5f);

            ambientEffect2 = Content.Load<Effect>("Effects/Ambient2");
            ambientEffect2.Parameters["AmbientColor"].SetValue(Color.Red.ToVector4());
            ambientEffect2.Parameters["AmbientIntensity"].SetValue(0.5f);


            diffuseEffect = Content.Load<Effect>("Effects/Diffuse");
            diffuseEffect.Parameters["AmbientColor"].SetValue(Color.White.ToVector4());
            diffuseEffect.Parameters["AmbientIntensity"].SetValue(0.1f);
            diffuseEffect.Parameters["DiffuseLightDirection"].SetValue(new Vector3(0, 0, 1));
            diffuseEffect.Parameters["DiffuseColor"].SetValue(Color.White.ToVector4());
            diffuseEffect.Parameters["DiffuseIntensity"].SetValue(1.0f);




            specularEffect = Content.Load<Effect>("Effects/Specular");
            specularEffect.Parameters["AmbientColor"].SetValue(Color.White.ToVector4());
            specularEffect.Parameters["AmbientIntensity"].SetValue(0.1f);
            specularEffect.Parameters["DiffuseLightDirection"].SetValue(new Vector3(-1, 0, 0));
            specularEffect.Parameters["DiffuseColor"].SetValue(Color.White.ToVector4());
            specularEffect.Parameters["DiffuseIntensity"].SetValue(1.0f);
            specularEffect.Parameters["Shininess"].SetValue(200.0f);
            specularEffect.Parameters["SpecularColor"].SetValue(Color.White.ToVector4());
            specularEffect.Parameters["SpecularIntensity"].SetValue(1.0f);






            base.Initialize();
        }



        private void InitGround()
        {
            floorVerts = new VertexPositionNormalTexture[6];

            var normal = new Vector3(0.0f, 0.0f, 15.0f);

            floorVerts[0].Position = new Vector3(-20, -20, 5);
            floorVerts[0].Normal = normal;
            floorVerts[1].Position = new Vector3(-20, 20, 5);
            floorVerts[1].Normal = normal;
            floorVerts[2].Position = new Vector3(20, -20, 5);
            floorVerts[2].Normal = normal;

            floorVerts[3].Position = floorVerts[1].Position;
            floorVerts[4].Position = new Vector3(20, 20, 5);
            floorVerts[5].Position = floorVerts[2].Position;

            int repetitions = 20;

            floorVerts[0].TextureCoordinate = new Vector2(0, 0);
            floorVerts[1].TextureCoordinate = new Vector2(0, repetitions);
            floorVerts[2].TextureCoordinate = new Vector2(repetitions, 0);

            floorVerts[3].TextureCoordinate = floorVerts[1].TextureCoordinate;
            floorVerts[4].TextureCoordinate = new Vector2(repetitions, repetitions);
            floorVerts[5].TextureCoordinate = floorVerts[2].TextureCoordinate;
        }

        protected override void LoadContent()
        {
            checkerboardTexture = Content.Load<Texture2D>("checker");
        }

        protected override void Update(GameTime gameTime)
        {
            robot.Update(gameTime);
            camera.Update(gameTime);



            Vector3 cameraTarget = new Vector3(0, 0, 0);
            viewVector = Vector3.Transform(cameraTarget - camera.Position, Matrix.CreateRotationY(0));
            viewVector.Normalize();



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            diffuseEffect.Parameters["View"].SetValue(camera.ViewMatrix);
            diffuseEffect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);

            //DrawGround();

            //walls.Draw(specularEffect, camera, graphics);
            //platform1.Draw(specularEffect, camera, graphics);
            //platform2.Draw(diffuseEffect, camera, graphics);

            //robot.Draw(camera, diffuseEffect);

            DrawModelWithSpecularEffect(robot.model, robot.GetWorldMatrix(), camera.ViewMatrix, camera.ProjectionMatrix);

            base.Draw(gameTime);
        }

        void DrawGround()
        {
            effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;

            //effect.View = camera.ViewMatrix;
            //effect.Projection = camera.ProjectionMatrix;

            effect.TextureEnabled = true;
            effect.Texture = checkerboardTexture;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,  floorVerts,  0,  2);
            }
        }
        private void DrawModelWithAmbientEffect(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = ambientEffect;

                    ambientEffect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform);
                    ambientEffect.Parameters["View"].SetValue(view);
                    ambientEffect.Parameters["Projection"].SetValue(projection);
                }
                mesh.Draw();
            }
        }

        private void DrawModelWithSpecularEffect(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = specularEffect;
                    specularEffect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform);
                    specularEffect.Parameters["View"].SetValue(view);
                    specularEffect.Parameters["Projection"].SetValue(projection);
                    specularEffect.Parameters["ViewVector"].SetValue(viewVector);
                    specularEffect.Parameters["DiffuseLightDirection"].SetValue(-viewVector);

                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
                    specularEffect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                }
                mesh.Draw();
            }
        }


    }
}