using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp3
{
    /// <summary>
    /// Fereastra principală OpenTK: proiecție, cameră, input, randare și o listă de cuburi.
    /// - G: toggle gravitație
    /// - Click stânga: adaugă cub la înălțime aleatoare
    /// - Click dreapta: curăță toate cuburile
    /// </summary>
    class Window3D : GameWindow
    {
        private KeyboardState previousKeyboard;
        private MouseState previousMouse;

        private readonly Randomizer rando;
        private readonly Axes ax;
        private readonly Grid grid;
        private readonly Camera3DIsometric cam;
        private readonly List<Objectoid> objects = new List<Objectoid>();

        private bool gravityEnabled = true; // implicit gravitația este activă
        private readonly Color DEFAULT_BKG_COLOR = Color.FromArgb(49, 50, 51);

        public Window3D() : base(1280, 768, new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On;

            rando = new Randomizer();
            ax = new Axes();
            grid = new Grid();
            cam = new Camera3DIsometric();

            DisplayHelp();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.ClearColor(DEFAULT_BKG_COLOR);
            GL.Viewport(0, 0, Width, Height);

            Matrix4 perspectiva = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, (float)Width / Height, 1f, 512f);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiva);

            cam.SetCamera();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            KeyboardState kb = Keyboard.GetState();
            MouseState ms = Mouse.GetState();

            // —— taste meniu / opțiuni
            if (kb[Key.Escape]) Exit();
            if (kb[Key.H] && !previousKeyboard[Key.H]) DisplayHelp();

            if (kb[Key.R] && !previousKeyboard[Key.R])
            {
                GL.ClearColor(DEFAULT_BKG_COLOR);
                ax.Show();
                grid.Show();
                objects.Clear();
            }

            if (kb[Key.K] && !previousKeyboard[Key.K]) ax.ToggleVisibility();
            if (kb[Key.B] && !previousKeyboard[Key.B]) GL.ClearColor(rando.RandomColor());
            if (kb[Key.V] && !previousKeyboard[Key.V]) grid.ToggleVisibility();
            if (kb[Key.G] && !previousKeyboard[Key.G]) gravityEnabled = !gravityEnabled;

            // —— mouse: click stânga = adaugă, click dreapta = golește
            if (ms.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
                AddCubeAtRandomHeight();
            if (ms.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Released)
                objects.Clear();

            // —— camera
            if (kb[Key.W]) cam.MoveForward();
            if (kb[Key.S]) cam.MoveBackward();
            if (kb[Key.A]) cam.MoveLeft();
            if (kb[Key.D]) cam.MoveRight();
            if (kb[Key.Q]) cam.MoveUp();
            if (kb[Key.E]) cam.MoveDown();

            //fizica fiecărui cub
            for (int i = 0; i < objects.Count; i++)
                objects[i].Update((float)e.Time, gravityEnabled);

            previousKeyboard = kb;
            previousMouse = ms;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            grid.Draw();
            ax.Draw();
            for (int i = 0; i < objects.Count; i++)
                objects[i].Draw();

            SwapBuffers();
        }

        private void DisplayHelp()
        {
            Console.WriteLine();
            Console.WriteLine("           MENIU");
            Console.WriteLine(" (H) - meniu");
            Console.WriteLine(" (ESC) - parasire aplicatie");
            Console.WriteLine(" (K) - schimbare vizibilitate sistem de axe");
            Console.WriteLine(" (R) - reseteaza scena la valori implicite");
            Console.WriteLine(" (B) - schimbare culoare de fundal");
            Console.WriteLine(" (V) - schimbare vizibilitate grid");
            Console.WriteLine(" (W, A, S, D, Q, E) - deplasare camera (izometric)");
            Console.WriteLine(" (G) - manipuleaza gravitatia");
            Console.WriteLine(" (Mouse clic stanga) - genereaza un nou obiect la o inaltime aleatoare");
            Console.WriteLine(" (Mouse clic dreapta) - curata lista de obiecte");
        }

        /// <summary>Adaugă un cub la o înălțime aleatoare și poziție XZ aleatoare.</summary>
        private void AddCubeAtRandomHeight()
        {
            Vector3 pos = rando.Random3DPoint(); 
            pos.Y = rando.RandomInt(20, 100);     
            objects.Add(new Objectoid(pos, 6f));
        }
    }
}
