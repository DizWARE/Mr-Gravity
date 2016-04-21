using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGravity.ParticleEngine
{
    public class ParticleEngine
    {
        private readonly Random _random;
        public Vector2 EmitterLocation { get; set; }
        private readonly List<Particle> _particles;
        private readonly List<Texture2D> _textures;
        public string ColorScheme;
        public int Ttl;

        public ParticleEngine(List<Texture2D> textures, Vector2 location, int ttl)
        {
            EmitterLocation = location;
            _textures = textures;
            _particles = new List<Particle>();
            _random = new Random();
            Ttl = ttl;
        }

        // Set variables. This is the guts of the whole system.
        private Particle GenerateNewParticle()
        {
            var texture = _textures[_random.Next(_textures.Count)];
            var position = EmitterLocation;
            
            // Set velocity randomly from [-1.0, 1.0] in both x and y.
            var velocity = new Vector2(
                    1f * (float)(_random.NextDouble() * 2 - 1),
                    1f * (float)(_random.NextDouble() * 2 - 1));
            
            // Generate random spin
            float angle = 0;
            var angularVelocity = 0.1f * (float)(_random.NextDouble() * 2 - 1);
            
            // Random RGB color values
            var color = new Color();
            //Color color = new Color(
            //        (float)random.NextDouble(),
            //        (float)random.NextDouble(),
            //        (float)random.NextDouble());

            var whichColor = _random.Next(3);
            switch (ColorScheme)
            {
                // collectible (yellow)
                case "Yellow":
                    switch (whichColor)
                    {
                        case 0:
                            color = new Color(247, 255, 0);
                            break;
                        case 1:
                            color = new Color(250, 255, 76);
                            break;
                        case 2:
                            color = new Color(239, 255, 143);
                            break;
                        default:
                            break;
                    }
                    break;

                case "Blue":
                    switch (whichColor)
                    {
                        case 0:
                            color = new Color(8, 91, 255);
                            break;
                        case 1:
                            color = new Color(85, 142, 255);
                            break;
                        case 2:
                            color = new Color(4, 45, 127);
                            break;
                        default:
                            break;
                    }
                    break;

                case "Red":
                    switch (whichColor)
                    {
                        case 0:
                            color = new Color(247, 255, 0);
                            break;
                        case 1:
                            color = new Color(250, 255, 76);
                            break;
                        case 2:
                            color = new Color(239, 255, 143);
                            break;
                        default:
                            break;
                    }
                    break;

                case "Orange":
                    switch (whichColor)
                    {
                        case 0:
                            color = new Color(255, 112, 0);
                            break;
                        case 1:
                            color = new Color(255, 155, 76);
                            break;
                        case 2:
                            color = new Color(204, 89, 0);
                            break;
                        default:
                            break;
                    }
                    break;

                case "Green":
                    switch (whichColor)
                    {
                        case 0:
                            color = new Color(0, 255, 91);
                            break;
                        case 1:
                            color = new Color(38, 127, 70);
                            break;
                        case 2:
                            color = new Color(76, 255, 140);
                            break;
                        default:
                            break;
                    }
                    break;

                case "Pink":
                    switch (whichColor)
                    {
                        case 0:
                            color = new Color(244, 0, 255);
                            break;
                        case 1:
                            color = new Color(122, 0, 127);
                            break;
                        case 2:
                            color = new Color(247, 76, 255);
                            break;
                        default:
                            break;
                    }
                    break;

                case "Purple":
                    switch (whichColor)
                    {
                        case 0:
                            color = new Color(160, 0, 255);
                            break;
                        case 1:
                            color = new Color(80, 0, 127);
                            break;
                        case 2:
                            color = new Color(189, 76, 255);
                            break;
                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }

            // Random size
            var size = (float)_random.NextDouble();

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, (Ttl + _random.Next(15)));
        }

        public void Update(int particleCount)
        {
            // Add particleCount particles each update (based on velocity from Level.cs)
            for (var i = 0; i < particleCount; i++)
                _particles.Add(GenerateNewParticle());

            // Loop through all particles and call their dependent Update functions. Remove dead particles
            for (var particle = 0; particle < _particles.Count; particle++)
            {
                _particles[particle].Update();
                if (_particles[particle].Ttl <= 0)
                {
                    _particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        // Call the particle's dependent draw function
        public void Draw(SpriteBatch spriteBatch)
        {
            for (var index = 0; index < _particles.Count; index++)
                _particles[index].Draw(spriteBatch);
        }

    }
}
