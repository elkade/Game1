using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game1.Models;
using System;

namespace Game1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        CustomModel walls;
        CustomModel platform;

        Vector3 cameraPosition = new Vector3(15, 10, 10);

        Item robot;
        Item bench1;
        Item bench2;
        Item locomotive;

        Camera camera;

        Effect effect;
        Effect effectLoco;

        Light ceilingLight1;
        Light ceilingLight2;
        Light trainLight;
        Light witcherLight;

        Lighting lighting;
        Lighting lightingLoco;

        Texture2D texture;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            // graphics.IsFullScreen = true;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1200;
        }

        protected override void Initialize()
        {
            walls = new ConcaveCube(new Vector3(0, 0, 0), 32);
            platform = new ConvexCube(new Vector3(5, -15, 0), 32);


            robot = new Item(4f, new Vector3(7, -10, 7), Vector3.Zero);
            robot.Initialize(Content.Load<Model>("witcher"));
            robot.Color = Color.Green;

            bench1 = new Item(0.006f, new Vector3(10, -12.5f, 0), new Vector3(0, -MathHelper.PiOver2, 0));
            bench2 = new Item(0.006f, new Vector3(10, -12.5f, 13), new Vector3(0, -MathHelper.PiOver2, 0));
            locomotive = new Locomotive(new Vector3(0.04f, 0.04f, 0.02f), new Vector3(-10, -11, -20), new Vector3(0, 0, -MathHelper.PiOver2));

            var benchContent = Content.Load<Model>("bench");

            bench1.Initialize(benchContent);
            bench1.Color = Color.Yellow;

            bench2.Initialize(benchContent);
            bench2.Color = Color.Violet;

            locomotive.Initialize(Content.Load<Model>("locomotive"));
            locomotive.Color = Color.SaddleBrown;

            camera = new Camera(graphics.GraphicsDevice);

            texture = Content.Load<Texture2D>("Textures/tex1");

            effect = Content.Load<Effect>("Effects/Specular");
            effectLoco = Content.Load<Effect>("Effects/Specular2");
            SetParams(effect);
            SetParams(effectLoco);

            ceilingLight1 = new Light { DiffuseColor = Color.LightYellow, SpecularColor = Color.Green, Position = new Vector3(0, 15, 8), Phi = MathHelper.PiOver4, Theta = MathHelper.PiOver4 / 2 };
            ceilingLight2 = new Light { DiffuseColor = Color.LightYellow, SpecularColor = Color.Blue, Position = new Vector3(0, 15, -8), Phi = MathHelper.PiOver4, Theta = MathHelper.PiOver4 / 2 };
            trainLight = new Light { DiffuseColor = Color.Yellow, SpecularColor = Color.Yellow, Position = Vector3.Zero, Direction = new Vector3(0, 0, -1), Phi = MathHelper.PiOver4, Theta = MathHelper.PiOver4 / 2 };
            witcherLight = new Light { DiffuseColor = Color.Red, SpecularColor = Color.Red, Position = new Vector3(7, -5, 7), Direction = new Vector3(0, 0, -1), Phi = MathHelper.PiOver4, Theta = MathHelper.PiOver4 / 2 };
            lighting = new Lighting(effect, ceilingLight1, ceilingLight2, trainLight, witcherLight);
            lightingLoco = new Lighting(effectLoco, ceilingLight1, ceilingLight2, trainLight, witcherLight);

            base.Initialize();
        }

        private void SetParams(Effect effect)
        {
            effect.Parameters["AmbientColor"].SetValue(Color.White.ToVector3());
            effect.Parameters["AmbientIntensity"].SetValue(0.1f);

            effect.Parameters["DiffuseIntensity"].SetValue(1.0f);

            effect.Parameters["SpecularPower"].SetValue(100f);
            effect.Parameters["SpecularIntensity"].SetValue(1.0f);

            effect.Parameters["FogEnabled"].SetValue(false);
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

            UpdateWithcerLight(gameTime);

            base.Update(gameTime);
        }

        Vector3 colVec = new Vector3(1, 1, 1);

        double timeInterval = 0f;
        Random r = new Random();

        private void UpdateWithcerLight(GameTime gameTime)
        {
            timeInterval += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeInterval < 1)
                return;
            timeInterval = 0;
            var col = new Color(r.Next(255), r.Next(255), r.Next(255));
            witcherLight.Phi = (float)(r.NextDouble() * MathHelper.PiOver2);
            witcherLight.Theta = witcherLight.Phi / 20;
            witcherLight.DiffuseColor = col;
            witcherLight.SpecularColor = col;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Effect effect;
            effect = lighting.UpdateEffect(walls.WorldMatrix, camera, Color.Gray);
            locomotive.Draw(camera, lightingLoco);

            walls.Draw(effect, graphics);
            effect = lighting.UpdateEffect(platform.WorldMatrix, camera, Color.DarkGray); //new Color(15,10,10));
            platform.Draw(effect, graphics, texture);


            robot.Draw(camera, lighting);
            bench1.Draw(camera, lighting);
            bench2.Draw(camera, lighting);

            base.Draw(gameTime);
        }

    }
}
