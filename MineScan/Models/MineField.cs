using System;

namespace MineScan.Models;

public class MineField
{
    private Cell[,] _field;

    public MineField(int width, int height, int minesCount)
    {
        _field = new Cell[width, height];
        
        SpawnMines(minesCount);
        CalculateAllMinesAround();
    }

    public void SpawnMines(int minesCount)
    {
        Random random = new Random();
        for (int i = 1; i <= minesCount; i++)
        {
            int x = random.Next(0, _field.GetLength(0));
            int y = random.Next(0, _field.GetLength(1));

            if (_field[x, y].IsMine)
            {
                i--;
                continue;
            }
            _field[x, y].IsMine = true;
        }
    }

    private void CalculateAllMinesAround()
    {
        for (int i = 0; i < _field.GetLength(0); i++)
        {
            for (int k = 0; k < _field.GetLength(1); k++)
            {
                _field[i, k].MinesAround = MinesAroundCell(i, k);
            }
        }
    }
    
    public sbyte MinesAroundCell(int x, int y)
    {
        sbyte minesCount = 0;
        Cell cell = _field[x, y];

        if (cell.IsMine) { minesCount = -1; return minesCount; }
        
        for (int i = x-1; i <= x+1; i++)
        {
            for (int k = y-1; k <= y+1; k++)
            {
                if (x == i && y == k) continue;
                if (i > _field.GetLength(0) || k > _field.GetLength(1)) { continue; }
                
                if (_field[i, k].IsMine)
                {
                    minesCount++;
                }
            }
        }
        return minesCount;
    }
}