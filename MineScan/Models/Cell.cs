namespace MineScan.Models;

public class Cell
{
    public bool IsMine { get; set; }
    public bool IsOpen { get; set; }
    public bool IsFlagged  { get; set; }
    public sbyte MinesAround { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}