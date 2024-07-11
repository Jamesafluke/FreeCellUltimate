namespace FreeCellLibrary;
public class History
{
    public Stack<Move> Moves{ get; set; }
    public History()
    {
        Moves = new Stack<Move>();
    }
}
