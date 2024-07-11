using FreeCellLibrary;
using System.Runtime.InteropServices;

namespace FreeCellLibrary;
public class GameBoard
{

    private Card[]? _cards;
    private readonly History _history;
    private Move _lastMove;

    public Column[]? Columns { get; set; }
    public Freecell[]? Freecells { get; set; }
    public Home[]? Homes { get; set; }
    public int Movability { get; set; }
    public int EmptyColumnMovability { get; set; }
    public int numOfEmptyFreeCells { get; set; } = 0;
    public int numOfEmptyColumns { get; set; } = 0;
    public int AutoHomeThreshold { get; set; } = 2;
    public bool GameIsWon { get; set; } = false;
    public int Seed { get; set; }
    
    public GameBoard(History history)
    {
        _cards = CreateCards();
        _cards = RandomizeCards(_cards);
        PopulateColumns(_cards);
        PopulateFreecells();
        PopulateHomes();
        _history = history;
    }

    private Card[]? CreateCards()
    {
         return new Card[]
         {
            new Card("s1", null, null, null),
            new Card("s2", "h3", "d3", "s1"),
            new Card("s3", "h4", "d4", "s2"),
            new Card("s4", "h5", "d5", "s3"),
            new Card("s5", "h6", "d6", "s4"),
            new Card("s6", "h7", "d7", "s5"),
            new Card("s7", "h8", "d8", "s6"),
            new Card("s8", "h9", "d9", "s7"),
            new Card("s9", "h10", "d10", "s8"),
            new Card("s10", "h11", "d11", "s9"),
            new Card("s11", "h12", "d12", "s10"),
            new Card("s12", "h13", "d13", "s11"),
            new Card("s13", null, null, "s12"),
            new Card("h1", null, null, null),
            new Card("h2", "s3", "c3", "h1"),
            new Card("h3", "s4", "c4", "h2"),
            new Card("h4", "s5", "c5", "h3"),
            new Card("h5", "s6", "c6", "h4"),
            new Card("h6", "s7", "c7", "h5"),
            new Card("h7", "s8", "c8", "h6"),
            new Card("h8", "s9", "c9", "h7"),
            new Card("h9", "s10", "c10", "h8"),
            new Card("h10", "s11", "c11", "h9"),
            new Card("h11", "s12", "c12", "h10"),
            new Card("h12", "s13", "c13", "h11"),
            new Card("h13", null, null, "h12"),
            new Card("d1", null, null, null),
            new Card("d2", "s3", "c3", "d1"),
            new Card("d3", "s4", "c4", "d2"),
            new Card("d4", "s5", "c5", "d3"),
            new Card("d5", "s6", "c6", "d4"),
            new Card("d6", "s7", "c7", "d5"),
            new Card("d7", "s8", "c8", "d6"),
            new Card("d8", "s9", "c9", "d7"),
            new Card("d9", "s10", "c10", "d8"),
            new Card("d10", "s11", "c11", "d9"),
            new Card("d11", "s12", "c12", "d10"),
            new Card("d12", "s13", "c13", "d11"),
            new Card("d13", null, null, "d12"),
            new Card("c1", null, null, null),
            new Card("c2", "h3", "d3", "c1"),
            new Card("c3", "h4", "d4", "c2"),
            new Card("c4", "h5", "d5", "c3"),
            new Card("c5", "h6", "d6", "c4"),
            new Card("c6", "h7", "d7", "c5"),
            new Card("c7", "h8", "d8", "c6"),
            new Card("c8", "h9", "d9", "c7"),
            new Card("c9", "h10", "d10", "c8"),
            new Card("c10", "h11", "d11", "c9"),
            new Card("c11", "h12", "d12", "c10"),
            new Card("c12", "h13", "d13", "c11"),
            new Card("c13", "null", "null", "c12"),
        };
    }
    
    private Card[]? RandomizeCards(Card[]? cards)
    {
        //Seed = Random.Shared.Next(6767766);
        Seed = 6767766;

        Random random = new Random(Seed);
        for (int i = 0; i < cards.Length; i++) { int randomInt = random.Next(cards.Length); var temp = cards[i];
            cards[i] = cards[randomInt];
            cards[randomInt] = temp;
        }
        return cards;
    }

    private void PopulateColumns(Card[]? cards)
    {
        Column[] columns = new Column[]
        {
            new Column(),
            new Column(),
            new Column(),
            new Column(),
            new Column(),
            new Column(),
            new Column(),
            new Column()
        };

        int counter = 0;
        int columnCounter = 0;
        while (counter < 28)
        {
            int stoppedCounter = counter;
            for (int i = counter; i < stoppedCounter + 7; i++)
            {
                columns[columnCounter].Add(cards[i]);
                counter++;
            }
            columnCounter++;
        }
        while (counter < 52)
        {
            int stoppedCounter = counter;
            for (int i = counter; i < stoppedCounter + 6; i++)
            {
                columns[columnCounter].Add(cards[i]);
                counter++;
            }
            columnCounter++;
        }
        Columns = columns;
    }

    private void PopulateFreecells()
    {
        Freecells = new Freecell[]
        {
            new Freecell(),
            new Freecell(),
            new Freecell(),
            new Freecell()
        };

    }

    private void PopulateHomes()
    {
        Homes = new Home[]
        {
            new Home(),
            new Home(),
            new Home(),
            new Home()
        };
    }


    public void Undo()
    {
        Move lastMove = _history.Moves.Pop();
        lastMove.Destination.Remove(lastMove.card);
        lastMove.Source.Add(lastMove.card);

        PostAction();
    }
    public void MoveStarter(char source, bool primary = true)
    {
        if (primary) { _lastMove = PrimaryMove(source); }
        else { _lastMove = SecondaryMove(source); }

        PostAction(_lastMove);
    }

    public Move PrimaryMove(char source)
    {
        CardList cardList = GetCardList(source);
        Card? card = cardList.Top;
        Move move = new Move();

        var destinationData = Prioritize(card);
        int? destination = destinationData.Item1;
        card = destinationData.Item2;

        //No possible move after prioritizing.
        //COME BACK AND TEST THIS SOON.
        if (card is null) { return null; }

        
        move.card = card;
        move.Source = cardList;

        RemoveFromColumn(source, card);
        card.Down = null;

        //FreeCells
        if(destination == 8)
        {
            move.Destination = AddToFreecells(card);
        }
        //Homes.
        else if (destination == 9)
        {
            char suit = char.Parse(card.Name.Substring(0, 1));
            switch (suit)
            {
                case 's':
                    Homes[0].Add(card);
                    move.Destination = Homes[0];
                    break;
                case 'h':
                    Homes[1].Add(card);
                    move.Destination = Homes[1];
                    break;
                case 'd':
                    Homes[2].Add(card);
                    move.Destination = Homes[2];
                    break;
                case 'c':
                    Homes[3].Add(card);
                    move.Destination = Homes[3];
                    break;
            }
        }
        //Columns
        else if (destination >= 0 && destination < 8)
        {
            Columns[(int)destination].Add(card);
            move.Destination = Columns[(int)destination];
        }

        return move;

    }

    private Move SecondaryMove(char source)
    {
        //TODO Write this sometime.
        return new Move();
    }


    private CardList AddToFreecells(Card card)
    {
        for (int i = 0; i < GlobalConfig.NumberOfFreecells; i++)
        {
            if (Freecells[i].Top is null)
            {
                Freecells[i].Top = card;
                return Freecells[i];
            }
        }
        return null;
    }

    private void RemoveFromColumn(char source, Card card)
    {
        switch (source)
        {
            case 'a':
                if (card.Down is null) break;
                Columns[0].Top = card.Down;
                card.Down.Up = null;
                break;
            case 's':
                Columns[1].Top = card.Down;
                card.Down.Up = null;
                break;
            case 'd': 
                Columns[2].Top = card.Down;
                card.Down.Up = null;
                break;
            case 'f':
                Columns[3].Top = card.Down;
                card.Down.Up = null;
                break;
            case 'j':
                Columns[4].Top = card.Down;
                card.Down.Up = null;
                break;
            case 'k':
                Columns[5].Top = card.Down;
                card.Down.Up = null;
                break;
            case 'l':
                Columns[6].Top = card.Down;
                card.Down.Up = null;
                break;
            case ';':
                Columns[7].Top = card.Down;
                card.Down.Up = null;
                break;
            default:
                return;
        }
    }

    private CardList GetCardList(char source)
    {
        switch (source)
        {
            case 'q':
                if (Freecells[0] is null) { return null; }
                return Freecells[0];
            case 'w':
                if (Freecells[1] is null) { return null; }
                return Freecells[1];
            case 'e':
                if (Freecells[2] is null) { return null; }
                return Freecells[2];
            case 'r':
                if (Freecells[3] is null) { return null; }
                return Freecells[3];

            case 'u':
                if (Homes[0] is null) { return null; }
                return Homes[0];
            case 'i':
                if (Homes[1] is null) { return null; }
                return Homes[1];
            case 'o':
                if (Homes[2] is null) { return null; }
                return Homes[2];
            case 'p':
                if (Homes[3] is null) { return null; }
                return Homes[3];

            case 'a':
                if (Columns[0] is null) { return null; }
                return Columns[0];
            case 's':
                if (Columns[1] is null) { return null; }
                return Columns[1];
            case 'd':
                if (Columns[2] is null) { return null; }
                return Columns[2];
            case 'f':
                if (Columns[3] is null) { return null; }
                return Columns[3];
            case 'j':
                if (Columns[4] is null) { return null; }
                return Columns[4];
            case 'k':
                if (Columns[5] is null) { return null; }
                return Columns[5];
            case 'l':
                if (Columns[6] is null) { return null; }
                return Columns[6];
            case ';':
                if (Columns[7] is null) { return null; }
                return Columns[7];
        }
        return null;
    }
    
    private (int?, Card?) Prioritize(Card? card)
    {
        Priority[] priority = new Priority[]
        {
            new Priority() {Value = 10000},
            new Priority() {Value = 10000},
            new Priority() {Value = 10000},
            new Priority() {Value = 10000},
            new Priority() {Value = 10000},
            new Priority() {Value = 10000},
            new Priority() {Value = 10000},
            new Priority() {Value = 10000},
            new Priority() {Value = 10000},
            new Priority() {Value = 10000},
        };

        //Prioritize Freecell and Home (for singles).
        if (card is not null && numOfEmptyFreeCells > 0)
        {
            priority[(int)Priority.DestinationIndex.Freecell].Value = 1200;
            priority[(int)Priority.DestinationIndex.Freecell].Card = card;
            priority[(int)Priority.DestinationIndex.Freecell].Destination = Priority.DestinationIndex.Freecell;

        }
        
        if(int.Parse(card.Name.Substring(1,card.Name.Length - 1)) == 1){
            priority[(int)Priority.DestinationIndex.Home].Value = 100;
            priority[(int)Priority.DestinationIndex.Home].Card = card;
            priority[(int)Priority.DestinationIndex.Home].Destination = Priority.DestinationIndex.Home;

        }
        else if (IsHomeable(card))
        {
            priority[(int)Priority.DestinationIndex.Home].Value = 100;
            priority[(int)Priority.DestinationIndex.Home].Card = card;
            priority[(int)Priority.DestinationIndex.Home].Destination = Priority.DestinationIndex.Home;
        }

        //Stack moves.
        //Prioritize the non-emptycolumn Movability stack moves
        Card tempCard = card;
        for (int cardIndex = 0; cardIndex < Movability; cardIndex++)
        {
            for (int i = 0; i < GlobalConfig.NumberOfColumns; i++)
            {
                if (IsStackable(tempCard, Columns[i].Top)){
                    int tempPriority = 500 - 10 * Columns[i].Top.StackSize;
                    if (priority[i].Value > tempPriority)
                    {
                        priority[i].Value = tempPriority;
                        priority[i].Card = tempCard;
                        priority[i].Destination = (Priority.DestinationIndex)i;
                    }
                }
            }
            //Progress to next card in the stack if it exists.
            if (tempCard.IsStacked) { tempCard = tempCard.Down; }
            else { break; }
        }

        //Prioritize the to-emptycolumn Movability stack moves
        tempCard = card;
        if (numOfEmptyColumns > 0)
        {
            for (int cardIndex = 0; cardIndex > EmptyColumnMovability; cardIndex++)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (Columns[i].Bottom is null)
                    {
                        //King to column case.
                        if(int.Parse(card.Name.Substring(1,card.Name.Length)) == 13)
                        {
                            int kingPriority = 200;
                            if (priority[i].Value <kingPriority)
                            {
                                priority[i].Value = kingPriority;
                                priority[i].Card = card;
                                priority[i].Destination = (Priority.DestinationIndex)i;
                            }
                        }
                        //Non-king to column case.
                        else
                        {
                            int tempPriority = 600;
                            if (priority[i].Value < tempPriority)
                            {
                                priority[i].Value = tempPriority;
                                priority[i].Card = card;
                                priority[i].Destination = (Priority.DestinationIndex)i;
                            } 
                        }
                    }
                }
            }
        }

        //Return the highest priority.
        Priority highest = new Priority();
        highest.Value = 10000;
        for (int i = 0; i < priority.Length; i++)
        {
            if (priority[i].Value < highest.Value)
            {
                highest.Value = priority[i].Value;
                highest.Destination = (Priority.DestinationIndex)i;
                highest.Card = priority[i].Card;
            }
        }
        return ((int)highest.Destination, highest.Card);
    }

    public void PostAction(Move move = null)
    {
        numOfEmptyFreeCells = 4;
        for (int i = 0; i < 4; i++)
        {
            if (Freecells[i].Top is not null) { numOfEmptyFreeCells++; }
        }

        CalculateIsOrdered();
        CheckWinCondition();
        CalculateMovability(); //TODO return the values instead.
        CalculateAutoHomeThreshold(); //TODO return the value instead
        if (move is not null) { _history.Moves.Push(move); }
        AutoHomeStuff();
    }

    private void CheckWinCondition()
    {
        int numberOfOrdered = 0;
        for (int i = 0; i < Columns.Length; i++)
        {
            if (Columns[i].IsOrdered) { numberOfOrdered++; }
        }

        if(numberOfOrdered > 6) { GameIsWon = true; }
    }

    private void CalculateIsOrdered()
    {
        for (int i = 0; i < Columns.Length; i++)
        {
            if (Columns[i].Bottom is null) { Columns[i].IsOrdered = true; }
            else if (Columns[i].Top.IsStacked)
            {
                Columns[i].IsOrdered = IsOrdered(Columns[i].Top);
            } 
        }
    }

    private bool IsOrdered(Card? card)
    {
        if(card.Down is null)
        {
            return true;
        }
        if (card.IsStacked)
        {
            return IsOrdered(card.Down);
        }
        else { return false; }
    }

    public void CalculateMovability()
    {
        Movability = (1 + numOfEmptyFreeCells) * (int)(Math.Pow(2, numOfEmptyColumns));
        EmptyColumnMovability = (1 + numOfEmptyFreeCells) * (int)(Math.Pow(2, (numOfEmptyColumns - 1)));
    }

    private void AutoHomeStuff()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Freecells[i].Top is null) { break; }
            int number = int.Parse(Freecells[i].Top.Name.Substring(1, Freecells[i].Top.Name.Length - 1));
            if(number >= AutoHomeThreshold)
            {
                switch (number)
                {
                    case 0:
                    PrimaryMove('q');
                        break;
                    case 1:
                    PrimaryMove('w');
                        break;
                    case 2:
                    PrimaryMove('e');
                        break;
                    case 3:
                    PrimaryMove('r');
                        break;
                }
            }
        }

        for (int i = 0; i < 8; i++)
        {
            int number = int.Parse(Columns[i].Top.Name.Substring(1, Columns[i].Top.Name.Length - 1));
            if (number <= AutoHomeThreshold)
            {
                switch (i)
                {
                    case 0:
                        PrimaryMove('a');
                        break;
                    case 1:
                        PrimaryMove('s');
                        break;
                    case 2:
                        PrimaryMove('d');
                        break;
                    case 3:
                        PrimaryMove('f');
                        break;
                    case 4:
                        PrimaryMove('j');
                        break;
                    case 5:
                        PrimaryMove('k');
                        break;
                    case 6:
                        PrimaryMove('l');
                        break;
                    case 7:
                        PrimaryMove(';');
                        break;
                }
            }
        }
    }

    private void CalculateAutoHomeThreshold()
    {
        AutoHomeThreshold = 0;
        for (int i = 0; i < 4; i++)
        {
            if (Homes[i].Top is null)
            {
                AutoHomeThreshold = 1;
            }
            else
            {
                int numberOnCard = int.Parse(Homes[i].Top.Name.Substring(1, Homes[i].Top.Name.Length - 1));
                if (numberOnCard > AutoHomeThreshold)
                {
                    AutoHomeThreshold = numberOnCard;
                }
            }
        }
    }

    private bool IsHomeable(Card? card)
    {
        char suit = char.Parse(card.Name.Substring(0, 1));
        switch (suit)
        {
            case 's':
                if (Homes[0].Bottom is null) { return false; }
                if(card.Homable == Homes[0].Bottom.Name) { return true; }
                break;
            case 'h':
                if (Homes[1].Bottom is null) { return false; }
                if(card.Homable == Homes[1].Bottom.Name) { return true; }
                break;
            case 'd':
                if (Homes[2].Bottom is null) { return false; }
                if(card.Homable == Homes[2].Bottom.Name) { return true; }
                break;
            case 'c':
                if (Homes[3].Bottom is null) { return false; }
                if(card.Homable == Homes[3].Bottom.Name) { return true; }
                break;
        }
        return false;
    }

    private bool IsStackable(Card? card, Card? ToStackOn)
    {
        if (ToStackOn.Name == card.CanStackOn1 || ToStackOn.Name == card.CanStackOn2) { return true; }
        return false;
    }
}
