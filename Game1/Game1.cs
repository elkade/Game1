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
        CustomModel platform;

        Vector3 cameraPosition = new Vector3(15, 10, 10);

        Robot robot;
        Robot bench1;
        Robot bench2;
        Robot locomotive;

        Camera camera;

        Effect specularEffect;

        Light ceilingLight1;
        Light ceilingLight2;
        Light trainLight;

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
            walls = new ConcaveCube(new Vector3 (0,0,0), 32);
            platform = new ConvexCube(new Vector3(5, -15, 0), 32);


            robot = new Robot(4f, new Vector3(7,-10,7), Vector3.Zero);
            robot.Initialize(Content.Load<Model>("witcher"));
            robot.Color = Color.Green;

            bench1 = new Robot(0.006f, new Vector3(10,-12.5f,0), new Vector3(0, -MathHelper.PiOver2, 0));
            bench2 = new Robot(0.006f, new Vector3(10,-12.5f, 13), new Vector3(0, -MathHelper.PiOver2, 0));
            locomotive = new Locomotive(new Vector3(0.04f, 0.04f, 0.02f), new Vector3(-10, -11, -20), new Vector3(0,0,-MathHelper.PiOver2));

            var benchContent = Content.Load<Model>("bench");

            bench1.Initialize(benchContent);
            bench1.Color = Color.Yellow;

            bench2.Initialize(benchContent);
            bench2.Color = Color.Violet;

            locomotive.Initialize(Content.Load<Model>("locomotive"));
            locomotive.Color = Color.SaddleBrown;

            camera = new Camera(graphics.GraphicsDevice);


            specularEffect = Content.Load<Effect>("Effects/Specular");
            specularEffect.Parameters["AmbientColor"].SetValue(Color.White.ToVector3());
            specularEffect.Parameters["AmbientIntensity"].SetValue(0.1f);

            specularEffect.Parameters["DiffuseIntensity"].SetValue(1f);

            specularEffect.Parameters["SpecularPower"].SetValue(100f);
            specularEffect.Parameters["SpecularIntensity"].SetValue(1.0f);

            ceilingLight1 = new Light { DiffuseColor = Color.LightYellow, SpecularColor = Color.Green, Position = new Vector3(0, 15, 8) };
            ceilingLight2 = new Light { DiffuseColor = Color.LightYellow, SpecularColor = Color.Blue, Position = new Vector3(0, 15, -8) };
            trainLight = new Light { DiffuseColor = Color.Yellow, SpecularColor = Color.Yellow, Position = Vector3.Zero, Direction = new Vector3(0, 0, -1), Phi = MathHelper.PiOver4, Theta = MathHelper.PiOver4/2 };

            lighting = new Lighting(specularEffect, ceilingLight1, ceilingLight2, trainLight);

            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            robot.Update(gameTime);
            bench1.Update(gameTime);
            bench2.Update(gameTime);
            locomotive.Update(gameTime);
            camera.Update(gameTime);

            //cameraLight.Position = camera.Position;
            trainLight.Position = locomotive.Position + new Vector3(0,0, 10);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Effect effect;
            effect = lighting.UpdateEffect(walls.WorldMatrix, camera, Color.Gray);
            walls.Draw(effect, graphics);
            effect = lighting.UpdateEffect(platform.WorldMatrix, camera, Color.DarkGray); //new Color(15,10,10));
            platform.Draw(effect, graphics);


            robot.Draw(camera, lighting);
            bench1.Draw(camera, lighting);
            bench2.Draw(camera, lighting);
            locomotive.Draw(camera, lighting);

            base.Draw(gameTime);
        }

    }
}