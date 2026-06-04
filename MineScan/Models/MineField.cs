using System;
using System.Collections.Generic;

namespace MineScan.Models;

public class MineField
{
    private Cell[,] _field;

    public bool IsMinesSpawned { get; private set; }
    public bool IsExploded { private set; get; }
    public bool IsWon { private set; get; }
    public bool IsGameOver => IsExploded || IsWon;
    
    public MineField(sbyte width, sbyte height)
    {
        _field = new Cell[width, height];
        
        FillingCells();
    }

    public void FillingCells()
    {
        for (int i = 0; i < _field.GetLength(0); i++)
        {
            for (int k = 0; k < _field.GetLength(1); k++)
            {
                _field[i, k] = new Cell(){ IsFlagged = false, IsMine = false, IsOpen = false, MinesAround = 0, X = i, Y = k};
            }
        }
    }

    public void SpawnMines(int minesCount, int firstX, int firstY)
    {
        Random random = new Random();
        for (int i = 1; i <= minesCount; i++)
        {
            int x = random.Next(0, _field.GetLength(0));
            int y = random.Next(0, _field.GetLength(1));

            if (_field[x, y].IsMine || (x >= firstX-1 && x <= firstX+1 && y >= firstY-1 && y <= firstY+1 ))
            {
                i--;
                continue;
            }
            _field[x, y].IsMine = true;
        }
        CalculateMinesAround();
        IsMinesSpawned = true;
    }

    private void CalculateMinesAround()
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
                if (i >= _field.GetLength(0) || k >= _field.GetLength(1) || i < 0 || k < 0) { continue; }
                
                if (_field[i, k].IsMine)
                {
                    minesCount++;
                }
            }
        }
        return minesCount;
    }
    
    public sbyte FlagsAroundCell(int x, int y)
    {
        sbyte flagsCount = 0;
        
        for (int i = x-1; i <= x+1; i++)
        {
            for (int k = y-1; k <= y+1; k++)
            {
                if (x == i && y == k) continue;
                if (i >= _field.GetLength(0) || k >= _field.GetLength(1) || i < 0 || k < 0) { continue; }
                
                if (_field[i, k].IsFlagged)
                {
                    flagsCount++;
                }
            }
        }
        return flagsCount;
    }

    public List<Cell> ToCellList()
    {
        var flatList = new List<Cell>();
        for (int y = 0; y < _field.GetLength(1); y++)
        {
            for (int x = 0; x < _field.GetLength(0); x++)
            {
                flatList.Add(_field[x, y]);
            }
        }
        return flatList;
    }
    
    public void OpenCell(int x, int y)
    {
        if (IsGameOver) return;
        if (x < 0 || x >= _field.GetLength(0) || y < 0 || y >= _field.GetLength(1)) return;
        if (_field[x, y].IsOpen || _field[x, y].IsFlagged || _field[x, y].IsQuestioned) return;
        
        _field[x, y].IsOpen = true;
        
        if (_field[x, y].MinesAround == 0 && !_field[x, y].IsMine)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int k = y - 1; k <= y + 1; k++)
                {
                    if (i == x && k == y) continue;
                    OpenCell(i, k);
                }
            }
        }
        
        if (_field[x, y].IsMine && _field[x, y].IsOpen)
        {
            IsExploded = true;
            return;
        }
        
        int closedSafeCells = 0;
        foreach (var cell in _field)
        {
            if (!cell.IsMine && !cell.IsOpen)
            {
                closedSafeCells++;
            }
        }
        
        if (closedSafeCells == 0)
        {
            IsWon = true;
        }
    }
    public void ToggleFlag(int x, int y)
    {
        if (!IsMinesSpawned) return;
        if (_field[x, y].IsOpen) return;

        if (!_field[x, y].IsFlagged && !_field[x, y].IsQuestioned)
        {
            _field[x, y].IsFlagged = true;
        }
        else if (_field[x, y].IsFlagged)
        {
            _field[x, y].IsFlagged = false;
            _field[x, y].IsQuestioned = true;
        }
        else
        {
            _field[x, y].IsQuestioned = false;
        }
    }

    public void Chording(int x, int y)
    {
        if (MinesAroundCell(x, y) == FlagsAroundCell(x, y))
        {
            for (int i = x-1; i <= x+1; i++)
            {
                for (int k = y-1; k <= y+1; k++)
                {
                    if (x == i && y == k) continue;
                    if (i >= _field.GetLength(0) || k >= _field.GetLength(1) || i < 0 || k < 0) { continue; }

                    if (!_field[i, k].IsFlagged)
                    {
                        OpenCell(i, k);
                    }
                }
            }
        }
    }
}