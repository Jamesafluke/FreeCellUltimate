using System.Drawing;
using System.Resources;

namespace FreeCellLibrary;
public class Card
{
    public string? Name { get; set; }
    public char Suit { get; set; }
    public int Value { get; set; }
    public int StackSize { get; set; }
    public Card? Up { get; set; }
    public Card? Down { get; set; }
    public bool IsStacked { get; set; }
    public string? CanStackOn1 { get; set; }
    public string? CanStackOn2 { get; set; }
    public Card(string name, string? stackable1, string? stackable2)
    {
        Name = name;
        CanStackOn1 = stackable1;
        CanStackOn2 = stackable2;
    }
}
