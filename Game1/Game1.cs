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

        CustomModel walls;
        CustomModel platform1;
        CustomModel platform2;

        Vector3 cameraPosition = new Vector3(15, 10, 10);

        Robot robot;
        Robot bench1;

        Camera camera;

        Effect specularEffect;

        Lighting lighting;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            // graphics.IsFullScreen = true;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            walls = new ConcaveCube(new Vector3 (7,7,7), 32);
            platform1 = new ConvexCube(new Vector3(4, -4, -4), 16);
            platform2 = new ConvexCube(new Vector3(4, 4, -4), 16);


            robot = new Robot(10f, new Vector3(7,7,7));
            robot.Initialize(Content.Load<Model>("witcher"));
            robot.Color = Color.Green;

            bench1 = new Robot(0.01f, new Vector3(10,10,10));
            bench1.Initialize(Content.Load<Model>("bench"));
            bench1.Color = Color.Yellow;

            camera = new Camera(graphics.GraphicsDevice);


            specularEffect = Content.Load<Effect>("Effects/Specular");
            specularEffect.Parameters["AmbientColor"].SetValue(Color.Green.ToVector3());
            specularEffect.Parameters["AmbientIntensity"].SetValue(0.1f);

            specularEffect.Parameters["DiffuseColor"].SetValue(Color.White.ToVector3());
            specularEffect.Parameters["DiffuseIntensity"].SetValue(0.75f);

            specularEffect.Parameters["SpecularPower"].SetValue(50.0f);
            specularEffect.Parameters["SpecularColor"].SetValue(Color.White.ToVector3());
            specularEffect.Parameters["SpecularIntensity"].SetValue(1.0f);



            lighting = new Lighting(specularEffect, new Vector3(0,0,0), MathHelper.PiOver4);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //checkerboardTexture = Content.Load<Texture2D>("checker");
        }

        protected override void Update(GameTime gameTime)
        {
            robot.Update(gameTime);
            bench1.Update(gameTime);
            camera.Update(gameTime);

            lighting.Position = camera.Position;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Effect effect;
            effect = lighting.UpdateEffect(walls.WorldMatrix, camera, Color.Red);
            walls.Draw(effect, graphics);
            //effect = lighting.UpdateEffect(platform1.WorldMatrix, camera);
            //platform1.Draw(effect, graphics);
            //effect = lighting.UpdateEffect(platform2.WorldMatrix, camera);
            //platform2.Draw(effect, graphics);

            robot.Draw(camera, lighting);
            bench1.Draw(camera, lighting);

            base.Draw(gameTime);
        }

    }
}