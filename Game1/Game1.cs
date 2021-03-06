﻿using Microsoft.Xna.Framework;
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
        Texture2D texture4;
        TextureCube perlin;

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
            walls = new ConcaveCube(new Vector3(0, 16, 0), 32);
            platform = new ConvexCube(new Vector3(5, -15, 0), 32);
            screen = new Screen(new Vector3(0, 7, 15.5f), 12, 8);


            robot = new Robot(4f, new Vector3(7, -10, 7), Vector3.Zero);
            robot.Initialize(Content.Load<Model>("witcher"));
            robot.Color = Color.Green;

            bench1 = new Robot(0.006f, new Vector3(10, -12.5f, 0), new Vector3(0, -MathHelper.PiOver2, 0));
            bench2 = new Robot(0.006f, new Vector3(10, -12.5f, 13), new Vector3(0, -MathHelper.PiOver2, 0));
            locomotive = new Locomotive(new Vector3(0.04f, 0.04f, 0.02f), new Vector3(-8.5f, -11, -20), new Vector3(0, 0, -MathHelper.PiOver2));

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
                Position = new Vector3(20, -8.600631f, -22),
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
            texture4 = Content.Load<Texture2D>("Textures/tex4");

            effect = Content.Load<Effect>("Effects/Specular");
            effectLoco = Content.Load<Effect>("Effects/Specular2");
            SetParams(effect);
            SetParams(effectLoco);

            textureMatrix = Matrix.Identity * Matrix.CreateScale(0.1f);
            effect.Parameters["TextureTransform"].SetValue(textureMatrix);
            effect.Parameters["ProjectionTexture"].SetValue(texture3);


            ceilingLight1 = new Light { DiffuseColor = Color.LightYellow, SpecularColor = Color.Green, Position = new Vector3(0, 15, 8), Phi = MathHelper.PiOver4 / 2, Theta = MathHelper.PiOver4 / 4 };
            ceilingLight2 = new Light { DiffuseColor = Color.LightYellow, SpecularColor = Color.Blue, Position = new Vector3(0, 15, -8), Phi = MathHelper.PiOver2, Theta = MathHelper.PiOver4 };
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

            bench1.Texture = texture;

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

            perlin = PreparePerlin(1000);

            base.Initialize();
        }

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

            SwitchProjectionSize();
            SwitchPlatformTexture();
            SwitchFog();
            SwitchTextureFilter();
            SwitchAntiAliasing();

            textureMatrix = textureMatrix * scale * Matrix.CreateRotationZ((float)gameTime.ElapsedGameTime.TotalSeconds * MathHelper.PiOver4 / 4);

            scale = Matrix.CreateScale(1);
            effect.Parameters["TextureTransform"].SetValue(textureMatrix);

            base.Update(gameTime);
        }

        private void SwitchProjectionSize()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
                scale = Matrix.CreateScale(1.1f);
            else if (Keyboard.GetState().IsKeyDown(Keys.D2))
                scale = Matrix.CreateScale(0.9f);
        }

        private void SwitchPlatformTexture()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
                platformTex = false;
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
                platformTex = true;
        }

        private void SwitchFog()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                effect.Parameters["FogEnabled"].SetValue(true);
                effectLoco.Parameters["FogEnabled"].SetValue(true);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.G))
            {
                effect.Parameters["FogEnabled"].SetValue(false);
                effectLoco.Parameters["FogEnabled"].SetValue(false);
            }
        }
        float MipMapLevelOfDetailBias = 1f;
        TextureFilter TextureFilter = TextureFilter.Point;
        private void SwitchTextureFilter()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
                MipMapLevelOfDetailBias *= 0.99f;
            else if (Keyboard.GetState().IsKeyDown(Keys.D4))
                MipMapLevelOfDetailBias *= 1.01f;

            if (MipMapLevelOfDetailBias > 8)
                MipMapLevelOfDetailBias = 8;
            else if (MipMapLevelOfDetailBias < 1)
                MipMapLevelOfDetailBias = 1;

            else if (Keyboard.GetState().IsKeyDown(Keys.D5))
                TextureFilter = TextureFilter.Point;
            else if (Keyboard.GetState().IsKeyDown(Keys.D6))
                TextureFilter = TextureFilter.PointMipLinear;
            else if (Keyboard.GetState().IsKeyDown(Keys.D7))
                TextureFilter = TextureFilter.LinearMipPoint;
            else if (Keyboard.GetState().IsKeyDown(Keys.D8))
                TextureFilter = TextureFilter.Linear;
        }

        private void SwitchAntiAliasing()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                rasterizerState = new RasterizerState { MultiSampleAntiAlias = true };
                graphics.PreferMultiSampling = true;
                graphics.ApplyChanges();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                rasterizerState = new RasterizerState { MultiSampleAntiAlias = false };
                graphics.PreferMultiSampling = false;
                graphics.ApplyChanges();
            }
        }

        RasterizerState rasterizerState = new RasterizerState { MultiSampleAntiAlias = true };

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
            var sampler = new SamplerState
            {
                MipMapLevelOfDetailBias = MipMapLevelOfDetailBias,
                MaxMipLevel = 8,
                Filter = TextureFilter,
            };
            graphics.GraphicsDevice.SamplerStates[0] = sampler;
            graphics.GraphicsDevice.SamplerStates[1] = sampler;
            graphics.GraphicsDevice.SamplerStates[2] = sampler;
            graphics.GraphicsDevice.SamplerStates[3] = sampler;

            graphics.GraphicsDevice.RasterizerState = rasterizerState;


            Effect effect;
            effect = lighting.UpdateEffect(platform.WorldMatrix, camera, Color.DarkGray); //new Color(15,10,10));

            //effect.Parameters["SkyBoxEnabled"].SetValue(true);

            walls.Draw(effect, graphics,null,null, perlin);

            platform.Draw(effect, graphics, platformTex?texture:texture4, texture2b);

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
        public TextureCube PreparePerlin(int size)
        {
            TextureCube tc = new TextureCube(GraphicsDevice, size, true, SurfaceFormat.Color);

            double[,] perlin = new double[size, size];
            Color[] col = new Color[size * size];
            Random r = new Random();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    perlin[i, j] = PerlinNoise.OctavePerlin((double)i / 20, (double)j / 20, 0, 9, 0.5);
                    //http://www.upvector.com/?section=Tutorials&subsection=Intro%20to%20Procedural%20Textures
                    perlin[i, j] = (1 + Math.Sin((i + perlin[i, j] / 2) * 50)) / 2;
                }
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    col[i + j * size] = new Color((float)perlin[i, j], (float)perlin[i, j], (float)perlin[i, j]);
                }
            }
            tc.SetData(CubeMapFace.PositiveX, col);
            tc.SetData(CubeMapFace.NegativeX, col);
            tc.SetData(CubeMapFace.PositiveY, col);
            tc.SetData(CubeMapFace.NegativeY, col);
            tc.SetData(CubeMapFace.PositiveZ, col);
            tc.SetData(CubeMapFace.NegativeZ, col);

            return tc;
        }
    }
}
