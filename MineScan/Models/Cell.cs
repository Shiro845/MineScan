namespace MineScan.Models;

public struct Cell
{
    public bool IsMine { get; set; }
    public bool IsOpen { get; set; }
    public bool IsFlagged  { get; set; }
    public bool MinesAround  { get; set; }
}