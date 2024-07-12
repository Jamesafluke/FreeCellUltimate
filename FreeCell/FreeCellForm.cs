
using FreeCellLibrary;
using System.Reflection;
using System.Resources;

namespace FreeCell;

public partial class FreeCellForm : Form
{
    int cardWidth = 125;
    int cardHeight = 174;

    int freecellX = 77;
    int freecellY = 37;
    int freecellDistance = 164;
    int homeX = 782;
    int homeY = 37;
    int homeDistance = 164;
    int columnX = 77;
    int columnY = 343;
    int columnHorizontalDistance = 171;
    int cardVerticalDistance = 46;
    bool spaceModifier = false;

    ResourceManager _resourceManager = Properties.Resources.ResourceManager;
    public GameBoard gameBoard { get; set; }
    public History history { get; set; }
    public FreeCellForm()
    {
        InitializeComponent();
        history = new History();
        gameBoard = new GameBoard(history);

        this.Text = $"Free Cell Ultimate {gameBoard.Seed}";

        CheckForAces();
        DrawBoard();
    }

    private void CheckForAces()
    {
        for (int i = 0; i < GlobalConfig.NumberOfColumns; i++)
        {
            if (gameBoard.Columns[i].Top.Value == 1)
            {
                switch (i)
                {
                    case 0:
                        gameBoard.MoveStarter('a');
                        break;
                    case 1:
                        gameBoard.MoveStarter('s');
                        break;
                    case 2:
                        gameBoard.MoveStarter('d');
                        break;
                    case 3:
                        gameBoard.MoveStarter('f');
                        break;
                    case 4:
                        gameBoard.MoveStarter('j');
                        break;
                    case 5:
                        gameBoard.MoveStarter('k');
                        break;
                    case 6:
                        gameBoard.MoveStarter('l');
                        break;
                    case 7:
                        gameBoard.MoveStarter(';');
                        break;
                }
            }
        }
    }

    public void DrawBoard()
    {
        backgroundPictureBox.BringToFront();
        //Columns
        int xPos = columnX;
        for (int i = 0; i < GlobalConfig.NumberOfColumns; i++)
        {
            Card card = gameBoard.Columns[i].PeekBottom();
            DrawColumn(i, xPos, columnY, card);
            xPos += columnHorizontalDistance;
        }


        int freecellXPos = freecellX;
        for (int i = 0; i < 4; i++)
        {
            if (gameBoard.Freecells[i].Top is not null)
            {
                Card card = gameBoard.Freecells[i].Top;
                DrawFreecell(freecellXPos, card);
            }
            freecellXPos += freecellDistance;
        }

        int homeXPos = homeX;
        for (int i = 0; i < 4; i++)
        {
            if (gameBoard.Homes[i].Top is not null)
            {
                Card card = gameBoard.Homes[i].Top;
                DrawHome(homeXPos, card);
            }
            homeXPos += homeDistance;
        }
        gameBoard.PostAction();
    }

    private void DrawHome(int xPos, Card? card)
    {
        Bitmap image = (Bitmap)_resourceManager.GetObject(card.Name);
        PictureBox testBox = new PictureBox();
        testBox.Image = image;
        testBox.Width = cardWidth;
        testBox.Height = cardHeight;
        testBox.Name = card.Name;
        testBox.Location = new Point(xPos, homeY);
        this.Controls.Add(testBox);
        testBox.BringToFront();
    }

    private void DrawFreecell(int xPos, Card? card)
    {
        Bitmap image = (Bitmap)_resourceManager.GetObject(card.Name);
        PictureBox testBox = new PictureBox();
        testBox.Image = image;
        testBox.Width = cardWidth;
        testBox.Height = cardHeight;
        testBox.Name = card.Name;
        testBox.Location = new Point(xPos, freecellY);
        this.Controls.Add(testBox);
        testBox.BringToFront();
    }

    public void DrawColumn(int columnNumber, int xPos, int yPos, Card card)
    {
        if(card is null){ return; }
        Bitmap image = (Bitmap)_resourceManager.GetObject(card.Name);
        PictureBox testBox = new PictureBox();
        testBox.Image = image;
        testBox.Width = cardWidth;
        testBox.Height = cardHeight;
        testBox.Name = card.Name;
        testBox.Location = new Point(xPos, yPos);
        this.Controls.Add(testBox);
        testBox.BringToFront();

        if (card.Up is not null)
        {
            card = card.Up;
            DrawColumn(columnNumber, xPos, yPos + cardVerticalDistance, card);
        }
    }

    private void FreeCellForm_KeyPress(object sender, KeyPressEventArgs e)
    {
        switch (e.KeyChar)
        {
            case 'q':
                gameBoard.MoveStarter('q');
                break;
            case 'w':
                gameBoard.MoveStarter('w');
                break;
            case 'e':
                gameBoard.MoveStarter('e');
                break;
            case 'r':
                gameBoard.MoveStarter('r');
                break;

            case 'u':
                gameBoard.MoveStarter('u');
                break;
            case 'i':
                gameBoard.MoveStarter('i');
                break;
            case 'o':
                gameBoard.MoveStarter('o');
                break;
            case 'p':
                gameBoard.MoveStarter('p');
                break;

            case 'a':
                gameBoard.MoveStarter('a');
                break;
            case 's':
                gameBoard.MoveStarter('s');
                break;
            case 'd':
                gameBoard.MoveStarter('d');
                break;
            case 'f':
                gameBoard.MoveStarter('f');
                break;
            case 'j':
                gameBoard.MoveStarter('j');
                break;
            case 'k':
                gameBoard.MoveStarter('k');
                break;
            case 'l':
                gameBoard.MoveStarter('l');
                break;
            case ';':
                gameBoard.MoveStarter(';');

                break;
            case 'g':
                break;
            case 'h':
                break;
            case 'z':
                gameBoard.Undo();
                break;
            case 'x':
                break;

        }
        DrawBoard();
        if (gameBoard.GameIsWon) { MessageBox.Show("You win!"); }
    }

    private void FreeCellForm_KeyDown(object sender, KeyEventArgs e)
    {
        //if(e.KeyCode == "space")

    }


    private void FreeCellForm_KeyUp(object sender, KeyEventArgs e)
    {

    }
}
