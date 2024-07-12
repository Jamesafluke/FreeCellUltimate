using FreeCellLibrary;
using System.Net.Security;
using System.Runtime.InteropServices;

namespace FreeCellLibrary;
public class GameBoard
{

    private Card[]? _cards;
    private readonly History _history;
    private Move _lastMove;
    private int numberOfEmptyFreeCells  = 0;
    private int numberOfEmptyColumns  = 0;
    private int redAutoHomeThreshold = 2;
    private int blackAutoHomeThreshold = 2;
    private int movability;
    private int emptyColumnMovability;

    public Column[]? Columns { get; set; }
    public Freecell[]? Freecells { get; set; }
    public Home[]? Homes { get; set; }
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
         Card[] cards = new Card[]
         {
            new Card("s1", null, null),
            new Card("s2", "h3", "d3"),
            new Card("s3", "h4", "d4"),
            new Card("s4", "h5", "d5"),
            new Card("s5", "h6", "d6"),
            new Card("s6", "h7", "d7"),
            new Card("s7", "h8", "d8"),
            new Card("s8", "h9", "d9"),
            new Card("s9", "h10", "d10"),
            new Card("s10", "h11", "d11"),
            new Card("s11", "h12", "d12"),
            new Card("s12", "h13", "d13"),
            new Card("s13", null, null),
            new Card("h1", null, null),
            new Card("h2", "s3", "c3"),
            new Card("h3", "s4", "c4"),
            new Card("h4", "s5", "c5"),
            new Card("h5", "s6", "c6"),
            new Card("h6", "s7", "c7"),
            new Card("h7", "s8", "c8"),
            new Card("h8", "s9", "c9"),
            new Card("h9", "s10", "c10"),
            new Card("h10", "s11", "c11"),
            new Card("h11", "s12", "c12"),
            new Card("h12", "s13", "c13"),
            new Card("h13", null, null),
            new Card("d1", null, null),
            new Card("d2", "s3", "c3"),
            new Card("d3", "s4", "c4"),
            new Card("d4", "s5", "c5"),
            new Card("d5", "s6", "c6"),
            new Card("d6", "s7", "c7"),
            new Card("d7", "s8", "c8"),
            new Card("d8", "s9", "c9"),
            new Card("d9", "s10", "c10"),
            new Card("d10", "s11", "c11"),
            new Card("d11", "s12", "c12"),
            new Card("d12", "s13", "c13"),
            new Card("d13", null, null),
            new Card("c1", null, null),
            new Card("c2", "h3", "d3"),
            new Card("c3", "h4", "d4"),
            new Card("c4", "h5", "d5"),
            new Card("c5", "h6", "d6"),
            new Card("c6", "h7", "d7"),
            new Card("c7", "h8", "d8"),
            new Card("c8", "h9", "d9"),
            new Card("c9", "h10", "d10"),
            new Card("c10", "h11", "d11"),
            new Card("c11", "h12", "d12"),
            new Card("c12", "h13", "d13"),
            new Card("c13", "null", "null"),
         };
        foreach (var card in cards)
        {
            card.Suit = char.Parse(card.Name.Substring(0, 1));

            card.Value = int.Parse(card.Name.Substring(1, card.Name.Length - 1));
        }
        return cards;
    }
    
    private Card[]? RandomizeCards(Card[]? cards)
    {
        //Seed = Random.Shared.Next(6767766);
        Seed = 3789932;

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
        if(_history.Moves.Count == 0)
        {
            return;
        }
        Move lastMove = _history.Moves.Pop();
        lastMove.Destination.Remove(lastMove.card);
        lastMove.Source.Add(lastMove.card);

        PostAction();
    }

    public void PostAction(Move move = null)
    {
        CalculateIsOrdered();
        CheckWinCondition();
        numberOfEmptyFreeCells = CalculateNumberOfEmptyFreecells();
        numberOfEmptyColumns = CalculateNumberOfEmptyColumns();
        movability = CalculateMovability();
        emptyColumnMovability = CalulateEmptyColumnMovability();
        redAutoHomeThreshold = CalculateAutoHomeThreshold("red"); 
        blackAutoHomeThreshold = CalculateAutoHomeThreshold("black"); 
        if (move is not null) { _history.Moves.Push(move); }
        bool sentSomethingHome = AutoHomeStuff();
        if (sentSomethingHome) { PostAction(); }
    }

    private int CalculateNumberOfEmptyColumns()
    {
        int number = 0;
        for (int i = 0; i < GlobalConfig.NumberOfColumns; i++)
        {
            if (Columns[i].Top == null)
            {
                number++;
            }
        }
        return number;
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

        if (card is null) { return null; }

        var destinationData = Prioritize(card);
        int? destination = destinationData.Item1;
        card = destinationData.Item2;


        
        move.card = card;
        move.Source = cardList;

        RemoveFromCardList(source, card);
        card.Down = null;

        //FreeCells
        if(destination == 8)
        {
            move.Destination = AddToFreecells(card);
        }
        //Homes.
        else if (destination == 9)
        {
            switch (card.Suit)
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
        if (card is not null && numberOfEmptyFreeCells > 0)
        {
            priority[(int)Priority.DestinationIndex.Freecell].Value = 1200;
            priority[(int)Priority.DestinationIndex.Freecell].Card = card;
            priority[(int)Priority.DestinationIndex.Freecell].Destination = Priority.DestinationIndex.Freecell;

        }

        //I think this is obselete because of AutoHome functionality.
        if (card.Value == 1)
        {
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
        for (int cardIndex = 0; cardIndex < movability; cardIndex++)
        {
            for (int i = 0; i < Columns.Length; i++)
            {
                if (IsStackable(tempCard, Columns[i].Top) && Columns[i].Top is not null){
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
            if (tempCard.IsStacked)
            {
                tempCard = tempCard.Down;
            }
            else { break; }
        }

        //Prioritize the to-emptycolumn Movability stack moves
        tempCard = card;
        if (numberOfEmptyColumns > 0)
        {
            for (int cardIndex = 0; cardIndex < emptyColumnMovability; cardIndex++)
            {
                for (int i = 0; i < GlobalConfig.NumberOfColumns; i++)
                {
                    if (Columns[i].Bottom is null)
                    {
                        //King to column case.
                        if (card.Value == 13)
                        {
                            int kingPriority = 200;
                            if (priority[i].Value > kingPriority)
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
                            if (priority[i].Value > tempPriority)
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


    private bool AutoHomeStuff()
    {
        bool sentSomethingHome = false;
        for (int i = 0; i < Freecells.Length; i++)
        {
            Card card = Freecells[i].Top;
            if (card is null) { break; }

            int autoHomeThreshold = redAutoHomeThreshold;
            if(card.Suit == 's' || card.Suit == 'c') { autoHomeThreshold = blackAutoHomeThreshold; }
            
            int value = card.Value;
            if (value <= autoHomeThreshold && IsHomeable(card))
            {
                SendHome(Freecells[i]);
                sentSomethingHome = true;
            }
        }

        for (int i = 0; i < Columns.Length; i++)
        {
            Card card = Columns[i].Top;
            if (card is null) { break; }

            int autoHomeThreshold = redAutoHomeThreshold;
            if(card.Suit == 's' || card.Suit == 'c') { autoHomeThreshold = blackAutoHomeThreshold; }

            int value = card.Value;
            if ((value == 1 || value <= autoHomeThreshold) && IsHomeable(card))
            {
                SendHome(Columns[i]);
                sentSomethingHome = true;
            }
        }
        return sentSomethingHome;
    }


    private bool IsHomeable(Card? card)
    {
        //Aces.
        if (card.Value == 1) return true;

        switch (card.Suit)
        {
            case 's':
                if (Homes[0].Bottom is null) { return false; }
                if (card.Value == Homes[0].Top.Value + 1) return true;
                break;
            case 'h':
                if (Homes[1].Bottom is null) { return false; }
                if (card.Value == Homes[1].Top.Value + 1) return true;
                break;
            case 'd':
                if (Homes[2].Bottom is null) { return false; }
                if (card.Value == Homes[2].Top.Value + 1) return true;
                break;
            case 'c':
                if (Homes[3].Bottom is null) { return false; }
                if (card.Value == Homes[3].Top.Value + 1) return true;
                break;
        }
        return false;
    }

    private void SendHome(CardList cardList)
    {
        Card card = cardList.Top;
        switch (card.Suit)
        {
            case 's':
                cardList.Remove(card);
                Homes[0].Add(card);
                break;
            case 'h':
                cardList.Remove(card);
                Homes[1].Add(card);
                break;
            case 'd':
                cardList.Remove(card);
                Homes[2].Add(card);
                break;
            case 'c':
                cardList.Remove(card);
                Homes[3].Add(card);
                break;
        }

    }


    private int CalculateNumberOfEmptyFreecells()
    {
        int numberOfEmptyFreecells = 0;
        for (int i = 0; i < GlobalConfig.NumberOfFreecells; i++)
        {
            if (Freecells[i].Top is null) { numberOfEmptyFreecells++; }
        }
        return numberOfEmptyFreecells;
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

    private void RemoveFromCardList(char source, Card card)
    {
        switch (source)
        {
            case 'a':
                Columns[0].Remove(card);
                break;
            case 's':
                Columns[1].Remove(card);
                break;
            case 'd':
                Columns[2].Remove(card);
                break;
            case 'f':
                Columns[3].Remove(card);
                break;
            case 'j':
                Columns[4].Remove(card);
                break;
            case 'k':
                Columns[5].Remove(card);
                break;
            case 'l':
                Columns[6].Remove(card);
                break;
            case ';':
                Columns[7].Remove(card);
                break;

            case 'q':
                Freecells[0].Remove(Freecells[0].Top);
                break;
            case 'w':
                Freecells[1].Remove(Freecells[1].Top);
                break;
            case 'e':
                Freecells[2].Remove(Freecells[2].Top);
                break;
            case 'r':
                Freecells[3].Remove(Freecells[3].Top);
                break;

            case 'u':
                Homes[0].Remove(Homes[0].Top);
                break;
            case 'i':
                Homes[1].Remove(Homes[2].Top);
                break;
            case 'o':
                Homes[2].Remove(Homes[2].Top);
                break;
            case 'p':
                Homes[3].Remove(Homes[3].Top);
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

    private int CalculateMovability()
    {
        return (1 + numberOfEmptyFreeCells) * (int)(Math.Pow(2, numberOfEmptyColumns));
    }

    private int CalulateEmptyColumnMovability()
    {
        return (1 + numberOfEmptyFreeCells) * (int)(Math.Pow(2, (numberOfEmptyColumns - 1)));
    }



    private int CalculateAutoHomeThreshold(string suit)
    {
        int autoHomeThreshold = 13;
        int[] values = new int[2];

        int column1;
        int column2;
        if(suit == "red") { column1 = 0; column2 = 3; }
        else { column1 = 1; column2 = 2; }

        int value1 = 0;
        int value2= 0;
        
        if (Homes[column1].Top is not null)
        {
            value1 = Homes[column1].Top.Value;
        }
        if (Homes[column2].Top is not null)
        {
            value2 = Homes[column2].Top.Value;
        }

        int lowestCardValue = 13;
        if (value1 < value2)
        {
            lowestCardValue = value1;
        }
        else
        {
            lowestCardValue = value2;
        }

        return lowestCardValue + 2;
    }

    private bool IsStackable(Card? card, Card? ToStackOn)
    {
        if(ToStackOn is null) { return true; }
        if (ToStackOn.Name == card.CanStackOn1 || ToStackOn.Name == card.CanStackOn2) { return true; }
        return false;
    }
}
