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

        public Vector3 Position = new Vector3(0, 25, 0);

        public Vector3 Target
        {
            get {
                Vector3 lookAtVector = new Vector3(0, -1, 0);

                // Then we'll modify the vector using this matrix:
                lookAtVector = Vector3.Transform(lookAtVector, RotationMatrix);
                lookAtVector += Position;
                lookAtVector.Normalize();
                return lookAtVector;
            }
        }

        public Matrix RotationMatrix;
        public Matrix ViewMatrix
        {
            get
            {
                Vector3 lookAtVector = new Vector3(0, -1, 0);

                // Then we'll modify the vector using this matrix:
                lookAtVector = Vector3.Transform(lookAtVector, RotationMatrix);
                lookAtVector += Position;

                var upVector = Vector3.UnitZ;

                return Matrix.CreateLookAt(Position, lookAtVector, upVector);
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
            Vector3 rotation = new Vector3(0, 0, 0);

            int positionX = 0;
            int positionY = 0;
            int positionZ = 0;

            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();

            if (mouseState.ScrollWheelValue < previousScrollValue)
            {
                positionY += 3;
            }
            else if (mouseState.ScrollWheelValue > previousScrollValue)
            {
                positionY -= 3;
            }
            previousScrollValue = mouseState.ScrollWheelValue;



            if (keyboardState.IsKeyDown(Keys.Right))
                positionX -= 1;
            if (keyboardState.IsKeyDown(Keys.Left))
                positionX += 1;
            if (keyboardState.IsKeyDown(Keys.Up))
                positionZ += 1;
            if (keyboardState.IsKeyDown(Keys.Down))
                positionZ -= 1;

            var forwardVector = new Vector3(positionX, positionY, positionZ);



            const float unitsPerSecond = 10;


            if (mouseState.LeftButton == ButtonState.Pressed)
            {

                var xPosition = mouseState.X;
                float xRatio = xPosition / (float)graphicsDevice.Viewport.Width;

                if (xRatio < 1 / 2.0f)
                {
                    rotation.Z = (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (xRatio < 2 / 3.0f)
                {
                }
                else
                {
                    rotation.Z = -(float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                var yPosition = mouseState.Y;
                float yRatio = yPosition / (float)graphicsDevice.Viewport.Height;

                if (yRatio < 1 / 2.0f)
                {
                    rotation.X = -(float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (yRatio < 2 / 3.0f)
                {
                }
                else
                {
                    rotation.X = (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            RotationMatrix *= Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationZ(rotation.Z);

            forwardVector = Vector3.Transform(forwardVector, RotationMatrix);

            Position += forwardVector * unitsPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;


        }
        private int previousScrollValue;
    }
}
