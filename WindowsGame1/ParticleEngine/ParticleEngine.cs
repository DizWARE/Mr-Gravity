using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GravityShift
{
    public class ParticleEngine
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private List<Texture2D> textures;
        public int colorScheme;

        public ParticleEngine(List<Texture2D> textures, Vector2 location, int scheme)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
            colorScheme = scheme;
        }

        // Set variables. This is the guts of the whole system.
        private Particle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            
            // Set velocity randomly from [-1.0, 1.0] is both x and y.
            Vector2 velocity = new Vector2(
                    1f * (float)(random.NextDouble() * 2 - 1),
                    1f * (float)(random.NextDouble() * 2 - 1));
            
            // Generate random spin
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            
            // Random RGB color values
            Color color = new Color();
            //Color color = new Color(
            //        (float)random.NextDouble(),
            //        (float)random.NextDouble(),
            //        (float)random.NextDouble());

            int whichScheme = random.Next(2);
            int whichColor = random.Next(3);
            switch (colorScheme)
            {
                // Color scheme 1
                case 0:
                    switch (whichColor)
                    {
                        case 0:
                            color = new Color(255, 86, 76);
                            break;
                        case 1:
                            color = new Color(76, 36, 33);
                            break;
                        case 2:
                            color = new Color(255, 14, 0);
                            break;
                        default:
                            color = new Color(0, 0, 0);
                            break;
                    }
                    break;
                
                // Color scheme 2
                case 1:
                    switch (whichColor)
                    {
                        case 0:
                            color = new Color(76, 86, 255);
                            break;
                        case 1:
                            color = new Color(0, 14, 255);
                            break;
                        case 2:
                            color = new Color(33, 36, 76);
                            break;
                        default:
                            color = new Color(0, 0, 0);
                            break;
                    }
                    break;

                // Color scheme 3
                case 2:
                    switch (whichColor)
                    {
                        case 0:
                            color = new Color(15, 255, 0);
                            break;
                        case 1:
                            color = new Color(36, 76, 33);
                            break;
                        case 2:
                            color = new Color(87, 255, 76);
                            break;
                        default:
                            color = new Color(0, 0, 0);
                            break;
                    }
                    break;

                // Color scheme 4
                case 3:
                    switch (whichColor)
                    {
                        case 0:
                            color = new Color(255, 115, 0);
                            break;
                        case 1:
                            color = new Color(255, 157, 76);
                            break;
                        case 2:
                            color = new Color(76, 53, 33);
                            break;
                        default:
                            color = new Color(0, 0, 0);
                            break;
                    }
                    break;

                // Color scheme 5
                case 4:
                    switch (whichColor)
                    {
                        case 0:
                            color = new Color(0, 236, 255);
                            break;
                        case 1:
                            color = new Color(76, 241, 255);
                            break;
                        case 2:
                            color = new Color(33, 73, 76);
                            break;
                        default:
                            color = new Color(0, 0, 0);
                            break;
                    }
                    break;

                // Color scheme 6
                case 5:
                    switch (whichColor)
                    {
                        case 0:
                            color = new Color(159, 4, 255);
                            break;
                        case 1:
                            color = new Color(60, 35, 76);
                            break;
                        case 2:
                            color = new Color(188, 81, 255);
                            break;
                        default:
                            color = new Color(0, 0, 0);
                            break;
                    }
                    break;

                default:
                    break;
            }
            
            // Random size
            float size = (float)random.NextDouble();
            
            // Every particle lives for at least 10 updates but as many as 25 (time-to-live)
            int ttl = 10 + random.Next(15);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public void Update(int particleCount)
        {
            // Add particleCount particles each update (based on velocity from Level.cs)
            for (int i = 0; i < particleCount; i++)
                particles.Add(GenerateNewParticle());

            // Loop through all particles and call their dependent Update functions. Remove dead particles
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        // Call the particle's dependent draw function
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
                particles[index].Draw(spriteBatch);
        }

    }
}
