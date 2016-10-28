using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    public class Camera
    {
        GraphicsDevice graphicsDevice;

        public Vector3 Position = new Vector3(0, 0, -20);

        public Matrix RotationMatrix;

        Vector3 lookAtVector
        {
            get
            {
                var lookat = Vector3.Transform(new Vector3(0, 0, 1), RotationMatrix);
                lookat.Normalize();
                return lookat;
            }
        }

        Vector3 rotation = new Vector3(0, 0, 0);

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
                float aspectRatio = graphicsDevice.Viewport.Width / (float)graphicsDevice.Viewport.Height;

                return Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
            }
        }

        public Camera(GraphicsDevice graphicsDevice)
        {
            previousScrollValue = Mouse.GetState().ScrollWheelValue;
            this.graphicsDevice = graphicsDevice;
            RotationMatrix = Matrix.Identity;
        }

        public void Update(GameTime gameTime)
        {
            int positionX = 0;
            int positionY = 0;
            int positionZ = 0;

            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();

            if (mouseState.ScrollWheelValue < previousScrollValue)
            {
                positionZ -= 3;
            }
            else if (mouseState.ScrollWheelValue > previousScrollValue)
            {
                positionZ += 3;
            }
            previousScrollValue = mouseState.ScrollWheelValue;



            if (keyboardState.IsKeyDown(Keys.Right))
                positionX -= 1;
            if (keyboardState.IsKeyDown(Keys.Left))
                positionX += 1;
            if (keyboardState.IsKeyDown(Keys.Up))
                positionY += 1;
            if (keyboardState.IsKeyDown(Keys.Down))
                positionY -= 1;

            var forwardVector = new Vector3(positionX, positionY, positionZ);


            if (mouseState.LeftButton == ButtonState.Pressed)
            {

                var xPosition = mouseState.X;
                float xRatio = xPosition / (float)graphicsDevice.Viewport.Width;

                if (xRatio < 1 / 3.0f)
                {
                    rotation.Y += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (xRatio < 2 / 3.0f)
                {
                }
                else
                {
                    rotation.Y += -(float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                var yPosition = mouseState.Y;
                float yRatio = yPosition / (float)graphicsDevice.Viewport.Height;

                if (yRatio < 1 / 3.0f)
                {
                    rotation.X += -(float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (yRatio < 2 / 3.0f)
                {
                }
                else
                {
                    rotation.X += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            RotationMatrix = Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y);

            forwardVector = Vector3.Transform(forwardVector, RotationMatrix);

            Position += forwardVector * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;

        }
        private int previousScrollValue;
    }
}
