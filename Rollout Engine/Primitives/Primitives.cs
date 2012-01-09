using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rollout.Collision;
using Rollout.Core;

namespace Rollout.Primitives
{
    /// <summary>
    /// A class to make primitive 2D objects out of lines.
    /// </summary>
    public class PrimitiveLine : DrawableGameObject
    {
        readonly Texture2D pixel;
        readonly ArrayList vectors;

        /// <summary>
        /// Gets/sets the colour of the primitive line object.
        /// </summary>
        public Color Colour;

        /// <summary>
        /// Gets/sets the position of the primitive line object.
        /// </summary>
       // public Vector2 Position;

        /// <summary>
        /// Gets/sets the render depth of the primitive line object (0 = front, 1 = back)
        /// </summary>
        public float Depth;

        /// <summary>
        /// Gets the number of vectors which make up the primtive line object.
        /// </summary>
        public int CountVectors
        {
            get
            {
                return vectors.Count;
            }
        }

        /// <summary>
        /// Creates a new primitive line object.
        /// </summary>
        public PrimitiveLine()
        {
            // create pixels
            pixel = new Texture2D(G.Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            var pixels = new Color[1];
            pixels[0] = Color.White;
            pixel.SetData(pixels);

            Colour = Color.White;
            Position = new Vector2(0, 0);
            Depth = 0;

            vectors = new ArrayList();
        }

        /// <summary>
        /// Adds a vector to the primive live object.
        /// </summary>
        /// <param name="vector">The vector to add.</param>
        public void AddVector(Vector2 vector)
        {
            vectors.Add(vector);
        }

        /// <summary>
        /// Insers a vector into the primitive line object.
        /// </summary>
        /// <param name="index">The index to insert it at.</param>
        /// <param name="vector">The vector to insert.</param>
        public void InsertVector(int index, Vector2 vector)
        {
            vectors.Insert(index, vectors);
        }

        /// <summary>
        /// Removes a vector from the primitive line object.
        /// </summary>
        /// <param name="vector">The vector to remove.</param>
        public void RemoveVector(Vector2 vector)
        {
            vectors.Remove(vector);
        }

        /// <summary>
        /// Removes a vector from the primitive line object.
        /// </summary>
        /// <param name="index">The index of the vector to remove.</param>
        public void RemoveVector(int index)
        {
            vectors.RemoveAt(index);
        }

        /// <summary>
        /// Clears all vectors from the primitive line object.
        /// </summary>
        public void ClearVectors()
        {
            vectors.Clear();
        }

        /// <summary>
        /// Renders the primtive line object.
        /// </summary>
        public override void Draw(GameTime gametime)
        {
            base.Draw(gametime);
            if (vectors.Count < 2)
                return;

            for (int i = 1; i < vectors.Count; i++)
            {
                var vector1 = (Vector2)vectors[i - 1];
                var vector2 = (Vector2)vectors[i];

                // calculate the distance between the two vectors
                var distance = Vector2.Distance(vector1, vector2);

                // calculate the angle between the two vectors
                var angle = (float)Math.Atan2((vector2.Y - vector1.Y), (vector2.X - vector1.X));

                // stretch the pixel between the two vectors
                G.SpriteBatch.Draw(pixel,
                    Position + vector1,
                    null,
                    Colour,
                    angle,
                    Vector2.Zero,
                    new Vector2(distance, 1),
                    SpriteEffects.None,
                    Depth);
            }
        }

        /// <summary>
        /// Creates a circle starting from 0, 0.
        /// </summary>
        /// <param name="circle">Circle Object</param>
        public void CreateCircle(Circle circle)
        {
            vectors.Clear();

            const float max = 2 * (float)Math.PI;
            const float step = max / 200;

            for (float theta = 0; theta < max; theta += step)
            {
                vectors.Add(new Vector2((float)circle.R * (float)Math.Cos(theta), (float)circle.R * (float)Math.Sin(theta)));
            }

            // then add the first vector again so it's a complete loop
            vectors.Add(new Vector2((float)circle.R * (float)Math.Cos(0), (float)circle.R * (float)Math.Sin(0)));
            Position = new Vector2((float)circle.X, (float)circle.Y);
        }

        /// <summary>
        /// Creates a rectangle starting from 0, 0.
        /// </summary>
        /// <param name="rectangle">Rectangle Object</param>
        public void CreateRectangle(Collision.Rectangle rectangle)
        {
            vectors.Clear();

            vectors.Add(new Vector2(0, 0));
            vectors.Add(new Vector2((float)rectangle.W, 0));
            vectors.Add(new Vector2((float)rectangle.W, (float)rectangle.H));
            vectors.Add(new Vector2(0, (float)rectangle.H));
            vectors.Add(new Vector2(0, 0));
            Position = new Vector2((float)rectangle.X, (float)rectangle.Y);
        }
    }
}
