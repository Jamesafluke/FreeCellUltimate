namespace FreeCellLibrary;
public class Column : CardList
{
    public CardList? Cards { get; set; } = null;
    public bool IsOrdered { get; set; } = false;
}
