using OpenTK;
using System;
using System.Drawing;

namespace ConsoleApp3
{
    /// <summary>
    /// Utilitar pentru generarea de valori pseudo-aleatoare folosite în scenă
    /// (culori, poziții 3D, întregi).
    /// </summary>
    class Randomizer
    {
        private readonly Random r;

        // Interval implicit pentru coordonate 3D
        private const int LOW_COORD_VAL = -25;
        private const int HIGH_COORD_VAL = 25;

        // Interval valid pentru componentele de culoare
        private const int COLOR_MIN = 0;
        private const int COLOR_MAX_EXCLUSIVE = 256; // Next() e exclusiv pe capătul superior

        // Interval implicit pentru întregi „simetrici” pe 0
        private const int LOW_INT_VAL = -25;
        private const int HIGH_INT_VAL = 26;

        public Randomizer()
        {
            r = new Random();
        }

        /// <summary>
        /// Returnează o culoare RGB aleatoare.
        /// </summary>
        public Color RandomColor()
        {
            int genR = r.Next(COLOR_MIN, COLOR_MAX_EXCLUSIVE);
            int genG = r.Next(COLOR_MIN, COLOR_MAX_EXCLUSIVE);
            int genB = r.Next(COLOR_MIN, COLOR_MAX_EXCLUSIVE);
            return Color.FromArgb(genR, genG, genB);
        }

        /// <summary>
        /// Generează un punct 3D cu coordonate întregi într-un interval rezonabil.
        /// </summary>
        public Vector3 Random3DPoint()
        {
            int genA = r.Next(LOW_COORD_VAL, HIGH_COORD_VAL + 1);
            int genB = r.Next(LOW_COORD_VAL, HIGH_COORD_VAL + 1);
            int genC = r.Next(LOW_COORD_VAL, HIGH_COORD_VAL + 1);
            return new Vector3(genA, genB, genC);
        }

        /// <summary>Întreg aleator în intervalul [-25, 25].</summary>
        public int RandomInt() => r.Next(LOW_INT_VAL, HIGH_INT_VAL);

        /// <summary>Întreg aleator în intervalul [minVal, maxVal).</summary>
        public int RandomInt(int minVal, int maxVal) => r.Next(minVal, maxVal);

        /// <summary>Întreg aleator în intervalul [0, maxVal).</summary>
        public int RandomInt(int maxVal) => r.Next(maxVal);
    }
}
