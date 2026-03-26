using static System.Math;

namespace MineScan
{
    public class Gridinfo
    {
        public struct Cell
        {
            public int X { get; set; }
            public int Y { get; set; }
            public bool IsMine { get; set; }
            public int MinesAround { get; set; }
            public bool IsFlagged { get; set; }
            public bool IsRevealed { get; set; }
        }

        public Cell[,] GridData;
        private int Size { get; }
        private int MinesCount { get; }
        public Gridinfo(int size, int minesCount)
        {
            Size = size;
            MinesCount = minesCount;
            GridData = new Cell[Size, Size];

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    GridData[x, y] = new Cell { X = x, Y = y, IsFlagged = false, IsMine = false, IsRevealed = false, MinesAround = 0};
                }
            }
                
        }
        public void PlaceMines(int firstX, int firstY)
        {
            for (int i = 0; i < 20; i++)
            {
                int x = Random.Shared.Next(Size);
                int y = Random.Shared.Next(Size);
                bool safeZone = Abs(x - firstX) <= 1 && Abs(y - firstY) <= 1;
                if (GridData[x, y].IsMine || safeZone) { i--; continue; }
                GridData[x, y].IsMine = true;
            }
            MinesAround();
        }

        public void MinesAround()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (!GridData[x, y].IsMine)
                    {
                        GridData[x, y].MinesAround = CalculateMinesAround(x, y);
                    }
                }
            }
        }
        private int CalculateMinesAround(int x, int y)
        {
            int count = 0;
            for (int cx = x - 1; cx <= x + 1; cx++)
            {
                for (int cy = y - 1; cy <= y + 1; cy++)
                {
                    if (cx == x && cy == y) { continue; }
                    ;
                    if (cx >= 0 && cy >= 0 && cx < Size && cy < Size)
                    {
                        if (GridData[cx, cy].IsMine) { count++; }
                    }
                }
            }
            return count;
        }
    }
}
