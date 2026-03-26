using System;
using System.Collections.Generic;
using System.Text;

namespace MineScan
{
    public class Config
    {
        public static double WindowWidth { get; set; } = 400;
        public static double WindowHeight { get; set; } = 400;

        public static int GridSize { get; set; } = 10;
        public static int buttonSize { get; set; } = 40;
        public static int MinesCount { get; set; } = 20;
    }
}
