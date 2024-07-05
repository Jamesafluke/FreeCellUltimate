namespace FreeCellLibrary;
public class Priority
{
    //public enum DestinationIndex
    //{
    //    Column0,
    //    Column1,
    //    Column2,
    //    Column3,
    //    Column4,
    //    Column5,
    //    Column6,
    //    Column7,
    //    Freecell,
    //    Home,
    //};
    public int Value { get; set; } = 0;
    public Card? Card { get; set; } = null;
    //public DestinationIndex Destination { get; set; }
    public int Destination { get; set; }
}
