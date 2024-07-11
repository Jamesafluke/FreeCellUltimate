
namespace FreeCellLibrary;
public abstract class CardList
{
    public bool IsEmpty { get; set; }
    public Card? Top { get; set; }
    public Card? Bottom { get; set; }
    public CardList()
    {
        Top = null;
        Bottom = null;
    }
    public void Add(Card card)
    {
        if (Top == null) { Top = card; Bottom = card; }
        else
        {
            card.Down = Top;
            card.Down.Up = card;
            Top = card;
            if(IsStackable(card, card.Down))
            {
                card.IsStacked = true;
                card.StackSize = card.Down.StackSize + 1;
            }
        }
    }
    public void Remove(Card card)
    {
        if(card.Down is null)
        {
            this.Top = null;
            this.Bottom = null;
            card.Down = null;
        }
        else
        {
            card.Down.Up = null;
            this.Top = card.Down;
            card.Down = null;
            card.IsStacked = false;
        }
    }
    public Card PeekTop()
    {
        return Top;
    }
    public Card PeekBottom()
    {
        return Bottom;
    }
    private bool IsStackable(Card card, Card top)
    {
        if (top.Name == card.CanStackOn1 || top.Name == card.CanStackOn2) { return true; }
        return false;
    }
}
