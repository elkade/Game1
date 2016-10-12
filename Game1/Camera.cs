using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    public class Camera
    {
        // We need this to calculate the aspectRatio
        // in the ProjectionMatrix property.
        GraphicsDevice graphicsDevice;

        public Vector3 Position = new Vector3(0, 3, 3);
        public Camera()
        {
            previousScrollValue = Mouse.GetState().ScrollWheelValue;
        }
        float angleZ;
        float angleX;

        public Matrix ViewMatrix
        {
            get
            {
                var lookAtVector = new Vector3(0, -1, -.5f);
                // We'll create a rotation matrix using our angle
                var rotationMatrix = Matrix.CreateRotationZ(angleZ) * Matrix.CreateRotationX(angleX);
                // Then we'll modify the vector using this matrix:
                lookAtVector = Vector3.Transform(lookAtVector, rotationMatrix);
                lookAtVector += Position;

                var upVector = Vector3.UnitZ;

                return Matrix.CreateLookAt(
                    Position, lookAtVector, upVector);
            }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
                float nearClipPlane = 1;
                float farClipPlane = 200;
                float aspectRatio = graphicsDevice.Viewport.Width / (float)graphicsDevice.Viewport.Height;

                return Matrix.CreatePerspectiveFieldOfView(
                    fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
            }
        }

        public Camera(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void Update(GameTime gameTime)
        {
            int positionX = 0;
            int positionY = 0;
            int positionZ = 0;

            var mouseState = Mouse.GetState();
            if (mouseState.ScrollWheelValue < previousScrollValue)
            {
                positionY += 3;
            }
            else if (mouseState.ScrollWheelValue > previousScrollValue)
            {
                positionY -= 3;
            }
            previousScrollValue = mouseState.ScrollWheelValue;

            KeyboardState keyboardState = Keyboard.GetState();


            if (keyboardState.IsKeyDown(Keys.Right))
                positionX -= 1;
            if (keyboardState.IsKeyDown(Keys.Left))
                positionX += 1;
            if (keyboardState.IsKeyDown(Keys.Up))
                positionZ += 1;
            if (keyboardState.IsKeyDown(Keys.Down))
                positionZ -= 1;

            var forwardVector = new Vector3(positionX, positionY, positionZ);

            var rotationMatrix = Matrix.CreateRotationZ(angleZ);
            forwardVector = Vector3.Transform(forwardVector, rotationMatrix);

            const float unitsPerSecond = 10;

            Position += forwardVector * unitsPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (mouseState.LeftButton != ButtonState.Pressed)
                return;
            var xPosition = mouseState.X;
            float xRatio = xPosition / (float)graphicsDevice.Viewport.Width;

            if (xRatio < 1 / 2.0f)
            {
                angleZ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (xRatio < 2 / 3.0f)
            {
            }
            else
            {
                angleZ -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            var yPosition = mouseState.Y;
            float yRatio = yPosition / (float)graphicsDevice.Viewport.Height;

            if (yRatio < 1 / 2.0f)
            {
                angleX -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (yRatio < 2 / 3.0f)
            {
            }
            else
            {
                angleX += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

        }
        private int previousScrollValue;
    }
}
