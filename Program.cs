using System;

namespace ConsoleApp3
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            using (var win = new Window3D())
            {
                // ~60 FPS; mută camera cu WASD + QE
                win.Run(60.0);
            }
        }
    }
}
