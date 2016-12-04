using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game1.Models;
using System;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
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
        Camera camera2;

        Effect effect;
        Effect effectLoco;

        BasicEffect basicEffect;

        Light ceilingLight1;
        Light ceilingLight2;
        Light trainLight;
        Light witcherLight;

        Lighting lighting;
        Lighting lightingLoco;

        Texture2D texture;
        Texture2D texture2;
        Texture2D texture2b;
        Texture2D texture3;

        TextureCube skyBox;

        SpriteBatch spriteBatch;

        RenderTarget2D mainRenderTarget;
        RenderTarget2D screenRenderTarget;

        Texture2D recentScreen;

        CustomModel screen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            // graphics.IsFullScreen = true;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferMultiSampling = true;


            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            walls = new ConcaveCube(new Vector3(0, 0, 0), 32);
            platform = new ConvexCube(new Vector3(5, -15, 0), 32);
            screen = new Screen(new Vector3(0, 7, 15.5f), 12, 8);


            robot = new Robot(4f, new Vector3(7, -10, 7), Vector3.Zero);
            robot.Initialize(Content.Load<Model>("witcher"));
            robot.Color = Color.Green;

            bench1 = new Robot(0.006f, new Vector3(10, -12.5f, 0), new Vector3(0, -MathHelper.PiOver2, 0));
            bench2 = new Robot(0.006f, new Vector3(10, -12.5f, 13), new Vector3(0, -MathHelper.PiOver2, 0));
            locomotive = new Locomotive(new Vector3(0.04f, 0.04f, 0.02f), new Vector3(-10, -11, -20), new Vector3(0, 0, -MathHelper.PiOver2));

            var benchContent = Content.Load<Model>("bench");

            bench1.Initialize(benchContent);
            bench1.Color = Color.Yellow;

            bench2.Initialize(benchContent);
            bench2.Color = Color.Violet;

            locomotive.Initialize(Content.Load<Model>("locomotive"));
            locomotive.Color = Color.SaddleBrown;

            camera = new Camera(graphics.GraphicsDevice);
            camera2 = new Camera(graphics.GraphicsDevice)
            {
                Position = new Vector3(20, -5.600631f, -18.18444f),
                RotationMatrix = new Matrix(
                    new Vector4(0.8854502f, 0, 0.4647342f, 0),
                    new Vector4(0.07709774f, 0.9861432f, -0.146893f, 0),
                    new Vector4(-0.4582944f, 0.1658964f, 0.8731807f, 0),
                    new Vector4(0, 0, 0, 1)
                )
            };

            texture = Content.Load<Texture2D>("Textures/tex1");
            texture2 = Content.Load<Texture2D>("Textures/tex2");
            texture2b = Content.Load<Texture2D>("Textures/tex2b");
            texture3 = Content.Load<Texture2D>("Textures/tex3");

            effect = Content.Load<Effect>("Effects/Specular");
            effectLoco = Content.Load<Effect>("Effects/Specular2");
            SetParams(effect);
            SetParams(effectLoco);

            textureMatrix = Matrix.Identity * Matrix.CreateScale(0.1f);
            effect.Parameters["TextureTransform"].SetValue(textureMatrix);
            effect.Parameters["ProjectionTexture"].SetValue(texture3);


            ceilingLight1 = new Light { DiffuseColor = Color.LightYellow, SpecularColor = Color.Green, Position = new Vector3(0, 15, 8), Phi = MathHelper.PiOver4 / 2, Theta = MathHelper.PiOver4 / 4 };
            ceilingLight2 = new Light { DiffuseColor = Color.LightYellow, SpecularColor = Color.Blue, Position = new Vector3(0, 15, -8), Phi = MathHelper.PiOver4, Theta = MathHelper.PiOver4 / 2 };
            trainLight = new Light { DiffuseColor = Color.Yellow, SpecularColor = Color.Yellow, Position = Vector3.Zero, Direction = new Vector3(0, 0, -1), Phi = MathHelper.PiOver4, Theta = MathHelper.PiOver4 / 2 };
            witcherLight = new Light { DiffuseColor = Color.Red, SpecularColor = Color.Red, Position = new Vector3(7, -5, 7), Direction = new Vector3(0, 0, -1), Phi = MathHelper.PiOver4, Theta = MathHelper.PiOver4 / 2 };
            lighting = new Lighting(effect, ceilingLight1, ceilingLight2, trainLight, witcherLight);
            lightingLoco = new Lighting(effectLoco, ceilingLight1, ceilingLight2, trainLight, witcherLight);


            basicEffect = new BasicEffect(graphics.GraphicsDevice);
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = texture;


            basicEffect.FogEnabled = false;
            basicEffect.FogColor = Color.Wheat.ToVector3();
            basicEffect.FogStart = 0;
            basicEffect.FogEnd = 30;

            robot.Texture = texture;

            skyBox = Content.Load<TextureCube>("Textures/OutputCube");
            //effect.Parameters["SkyBoxTexture"].SetValue(skyBox);

            mainRenderTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            screenRenderTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            base.Initialize();
        }

        /// <summary>
        /// Draws the entire scene in the given render target.
        /// </summary>
        /// <returns>A texture2D with the scene drawn in it.</returns>
        protected void DrawSceneToTexture(RenderTarget2D renderTarget, Camera camera)
        {
            // Set the render target
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            // Draw the scene
            Draw(camera);

            // Drop the render target
            GraphicsDevice.SetRenderTarget(null);
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphics.GraphicsDevice.PresentationParameters.MultiSampleCount = 16;
        }

        Matrix scale = Matrix.CreateScale(1);
        bool platformTex = true;

        protected override void Update(GameTime gameTime)
        {
            robot.Update(gameTime);
            bench1.Update(gameTime);
            bench2.Update(gameTime);
            locomotive.Update(gameTime);
            camera.Update(gameTime);

            //cameraLight.Position = camera.Position;
            trainLight.Position = locomotive.Position + new Vector3(0, 0, 10);

            UpdateWitcherLight(gameTime);

            UpdateParameters();

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
                scale = Matrix.CreateScale(1.1f);
            else if (Keyboard.GetState().IsKeyDown(Keys.D2))
                scale = Matrix.CreateScale(0.9f);
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
                platformTex = false;
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
                platformTex = true;

            textureMatrix = textureMatrix * scale * Matrix.CreateRotationZ((float)gameTime.ElapsedGameTime.TotalSeconds * MathHelper.PiOver4 / 4);

            scale = Matrix.CreateScale(1);
            effect.Parameters["TextureTransform"].SetValue(textureMatrix);

            base.Update(gameTime);
        }

        Matrix textureMatrix;

        private void UpdateParameters()
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.M))
                Settings.Multisampling = !Settings.Multisampling;

        }

        Vector3 colVec = new Vector3(1, 1, 1);

        double timeInterval = 0f;
        Random r = new Random();

        private void UpdateWitcherLight(GameTime gameTime)
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

        protected void Draw(Camera camera)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            for (int i = 0; i < 12; i++)
            {
                graphics.GraphicsDevice.SamplerStates[i] = new SamplerState
                {
                    MipMapLevelOfDetailBias = 0.001f,
                    MaxMipLevel = 8,
                    Filter = TextureFilter.Linear,
                };
            }

            RasterizerState rasterizerState1 = new RasterizerState { MultiSampleAntiAlias = true, };
            graphics.GraphicsDevice.RasterizerState = rasterizerState1;


            Effect effect;
            effect = lighting.UpdateEffect(platform.WorldMatrix, camera, Color.DarkGray); //new Color(15,10,10));

            walls.Draw(effect, graphics, texture);
            platform.Draw(effect, graphics, texture, platformTex ? texture2b : texture2);

            screen.Draw(effect, graphics, recentScreen);

            locomotive.Draw(camera, lightingLoco);

            robot.Draw(camera, lighting);

            bench2.Draw(camera, lighting);

            bench1.Draw(camera, lighting);


        }
        protected override void Draw(GameTime gameTime)
        {
            DrawSceneToTexture(mainRenderTarget, camera);
            DrawSceneToTexture(screenRenderTarget, camera2);

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                        SamplerState.LinearClamp, DepthStencilState.Default,
                        RasterizerState.CullNone);
            ScreenToTexture();

            spriteBatch.Draw(mainRenderTarget, new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ScreenToTexture()
        {
            if (recentScreen != null)
                recentScreen.Dispose();
            recentScreen = new Texture2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight);
            Color[] content = new Color[GraphicsDevice.PresentationParameters.BackBufferWidth *
                GraphicsDevice.PresentationParameters.BackBufferHeight];
            screenRenderTarget.GetData(content);
            recentScreen.SetData(content);
        }
    }
}
