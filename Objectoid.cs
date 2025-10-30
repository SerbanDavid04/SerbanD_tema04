using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace ConsoleApp3
{
    /// <summary>
    /// Cub colorat care poate „cădea” sub acțiunea gravitației până la sol (Y = size/2).
    /// </summary>
    class Objectoid
    {
        private static readonly Random r = new Random();

        private bool visibility;
        private Color colour;
        private Vector3 position;
        private float size;
        private float verticalSpeed;
        private bool grounded;

        public Objectoid(Vector3 position, float size, Color colour)
        {
            this.position = position;
            this.size = size;
            this.colour = colour;
            visibility = true;
            verticalSpeed = 0f;
            grounded = false;
        }

        public Objectoid(Vector3 position, float size)
            : this(position, size, Color.FromArgb(r.Next(256), r.Next(256), r.Next(256))) { }

        public Objectoid() : this(new Vector3(0f, 50f, 0f), 6f) { } // implicit: apare sus, ca să cadă

        public void ToggleVisibility() => visibility = !visibility;
        public void Hide() => visibility = false;
        public void Show() => visibility = true;

        /// <summary>
        /// Actualizează poziția verticală a cubului. Dacă <paramref name="gravityOn"/> e false,
        /// nu se aplică gravitația (cubul rămâne pe loc).
        /// </summary>
        public void Update(float deltaTime, bool gravityOn)
        {
            if (!gravityOn || grounded) return;

            float gravity = -30f;
            verticalSpeed += gravity * deltaTime;
            position.Y += verticalSpeed * deltaTime;

            float groundY = size / 2f;
            if (position.Y <= groundY)
            {
                position.Y = groundY;
                verticalSpeed = 0f;
                grounded = true;
            }
        }

        /// <summary>Desenează cubul în poziția curentă.</summary>
        public void Draw()
        {
            if (!visibility) return;
            float s = size / 2f;

            GL.PushMatrix();
            GL.Translate(position);
            GL.Color3(colour);

            GL.Begin(PrimitiveType.Quads);

            // Top
            GL.Normal3(0f, 1f, 0f);
            GL.Vertex3(-s, +s, -s); GL.Vertex3(+s, +s, -s);
            GL.Vertex3(+s, +s, +s); GL.Vertex3(-s, +s, +s);
            // Bottom
            GL.Normal3(0f, -1f, 0f);
            GL.Vertex3(-s, -s, +s); GL.Vertex3(+s, -s, +s);
            GL.Vertex3(+s, -s, -s); GL.Vertex3(-s, -s, -s);
            // Front
            GL.Normal3(0f, 0f, 1f);
            GL.Vertex3(-s, -s, +s); GL.Vertex3(-s, +s, +s);
            GL.Vertex3(+s, +s, +s); GL.Vertex3(+s, -s, +s);
            // Back
            GL.Normal3(0f, 0f, -1f);
            GL.Vertex3(+s, -s, -s); GL.Vertex3(+s, +s, -s);
            GL.Vertex3(-s, +s, -s); GL.Vertex3(-s, -s, -s);
            // Left
            GL.Normal3(-1f, 0f, 0f);
            GL.Vertex3(-s, -s, -s); GL.Vertex3(-s, +s, -s);
            GL.Vertex3(-s, +s, +s); GL.Vertex3(-s, -s, +s);
            // Right
            GL.Normal3(1f, 0f, 0f);
            GL.Vertex3(+s, -s, +s); GL.Vertex3(+s, +s, +s);
            GL.Vertex3(+s, +s, -s); GL.Vertex3(+s, -s, -s);

            GL.End();
            GL.PopMatrix();
        }
    }
}
