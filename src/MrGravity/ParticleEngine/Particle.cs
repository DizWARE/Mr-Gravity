using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGravity.ParticleEngine
{
    public class Particle
    {
        public Texture2D Texture { get; set; }      // The texture that will be drawn to represent the particle
        public Vector2 Position { get; set; }       // The current position of the particle        
        public Vector2 Velocity { get; set; }       // The speed of the particle at the current instance
        public float Angle { get; set; }            // The current angle of rotation of the particle
        public float AngularVelocity { get; set; }  // The speed that the angle is changing
        public Color Color { get; set; }            // The color of the particle
        public float Size { get; set; }             // The size of the particle
        public int Ttl { get; set; }                // The 'time to live' of the particle
        private readonly bool _sizeOverride;
        public Vector2 Randomness;

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, float size, int ttl)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            Ttl = ttl;
            _sizeOverride = false;
            Randomness = new Vector2(0, 0);
        }

        public Particle(Texture2D texture, Vector2 position, Random random)
        {
            var whichColor = random.Next(3);

            Texture = texture;
            Position = position;
            Velocity = new Vector2(0,0);
            Angle = 0;
            AngularVelocity = 0.05f * (float)(random.NextDouble() * 2 - 1);
            Size = (float)random.NextDouble() / 2;
            Ttl = 1000;

            switch (whichColor)
            {
                case 0:
                    Color = new Color(20, 20, 20);
                    break;
                case 1:
                    Color = new Color(40, 40, 40);
                    break;
                case 2:
                    Color = new Color(60, 60, 60);
                    break;
                default:
                    Color = Color.Black;
                    break;
            }

            _sizeOverride = true;
            Randomness = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1));
        }

        // Update velocity, spin, size, life
        public void Update()
        {
            Ttl--;
            Position += Velocity;
            Angle += AngularVelocity;
            if (!_sizeOverride)
                Size *= 0.99f;
        }

        // Draw particle
        public void Draw(SpriteBatch spriteBatch)
        {
            var sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            var origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            spriteBatch.Draw(Texture, Position, sourceRectangle, Color,
                Angle, origin, Size, SpriteEffects.None, 0f);
        }


    }
}
