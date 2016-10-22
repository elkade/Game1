using Microsoft.Xna.Framework;

namespace Game1
{
    class Locomotive : Robot
    {
        public Locomotive(Vector3 scale, Vector3 position, Vector3 rotation) : base(scale, position, rotation) { }
        public override void Update(GameTime gameTime)
        {
            Position.Z += 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Position.Z > 40)
                Position.Z = -40;
        }
    }
}
